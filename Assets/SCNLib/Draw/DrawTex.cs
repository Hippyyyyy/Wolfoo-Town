using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SCN.Draw
{
    [System.Serializable]
    public class DrawTex
    {
        [Header("Custom")]
        
        [Tooltip("Brush material")]
        [SerializeField] Material blitMaterial;

        [Tooltip("image duoc ve len")]
        [SerializeField] RawImage drawImage;

        [Tooltip("Gia tri cang lon hieu nang cang tot nhung chat luong hinh anh se bi giam di")]
        [SerializeField] float dimensity = 3;

        public Material BlitMaterial { get => blitMaterial; set => blitMaterial = value; }
        public RawImage DrawImage { get => drawImage; set => drawImage = value; }
        public float Dimensity { get => dimensity; set => dimensity = value; }

        [Space(20)]

        /// <summary>
        /// texture khoi tao
        /// </summary>
        Texture2D initialTexture;

        /// <summary>
        /// la texture duoc xu ly trong khi ve
        /// </summary>
        RenderTexture renderTexture;

        RenderTexture bufferTexture;
        BrushStats brushStats;
        Vector2 lastMPos;

        Color currentColor = Color.black;
        float diameterRatio = 1;

        public Vector2Int TextureDimensions
            => new Vector2Int(renderTexture.width, renderTexture.height);

        public RenderTexture CurrentTex => renderTexture;

		// Goi a Start
		public void Setup(Vector2 imageSize, BrushStats brush)
        {
            drawImage.GetComponent<RectTransform>().sizeDelta = imageSize;

            renderTexture = new RenderTexture(
                (int)(imageSize.x / dimensity), (int)(imageSize.y / dimensity), 1);
            drawImage.texture = renderTexture;

            initialTexture = new Texture2D(renderTexture.width, renderTexture.height);
            var pixels = initialTexture.GetPixels32();
            for (int i = 0; i < TextureDimensions.x * TextureDimensions.y; i++)
            {
                pixels[i] = new Color32(255, 255, 255, 255);
            }

            initialTexture.SetPixels32(pixels);
            initialTexture.Apply();

            Graphics.Blit(initialTexture, renderTexture);

            bufferTexture = RenderTexture.GetTemporary(initialTexture.width, initialTexture.height);

            SetBrush(brush);
            SetColor(Color.black);
            SetBrushSize(1);
        }

        #region public methob
        public void ResetImage()
        {
            Graphics.Blit(initialTexture, renderTexture);
        }

        /// <summary>
        /// Goi ham nay khi bat dau dat but xuong image de ve
        /// </summary>
        public void StartDrawSession()
		{
            GetPixelByMousePosition(Input.mousePosition, out Vector2 texturePos);

            var vt = new Vector2Int(Mathf.CeilToInt(texturePos.x), Mathf.CeilToInt(texturePos.y));
            Stamp(vt);
            lastMPos = texturePos;
        }

        /// <summary>
        /// Goi ham nay khi dang drag ngon tay tren man hinh, duong ve se xuat hien theo toa do MousePosition
        /// </summary>
        public void UpdateDraw()
		{
            GetPixelByMousePosition(Input.mousePosition, out Vector2 texturePos);

            var distance = Vector2.Distance(texturePos, lastMPos);
            if (distance < brushStats.MinDis)
            {
                return;
            }
            Stamp(new Vector2Int(Mathf.CeilToInt(texturePos.x), Mathf.CeilToInt(texturePos.y)));

            if (distance >= brushStats.MaxDis)
            {
                var divs = (int)(distance / brushStats.SpawnDistance);
                for (int i = 0; i < divs; i++)
                {
                    Stamp(new Vector2Int(
                    Mathf.CeilToInt((texturePos.x - lastMPos.x) * (i + 1) / (divs + 1) + lastMPos.x),
                    Mathf.CeilToInt((texturePos.y - lastMPos.y) * (i + 1) / (divs + 1) + lastMPos.y)));
                }
            }

            lastMPos = texturePos;
        }

        public void SetBrush(BrushStats stats)
        {
            brushStats = stats;
        }

        public void SetColor(Color color)
		{
            currentColor = color;

            if (brushStats != null)
            {
                currentColor.a = brushStats.Alpha;
            }

            blitMaterial.SetColor("_color", currentColor);
        }

        public void SetBrushSize(float size)
		{
            diameterRatio = size;

            var temp = brushStats.Diameter * diameterRatio;
            blitMaterial.SetVector("_size", new Vector2(temp, temp));
		}
        #endregion

        #region Draw
        /// (0, 0) la diem o goc duoi cung ben trai
        void Stamp(Vector2Int p)
        {
            // Random brush
            if (brushStats.BrushTex.Length > 1)
            {
                blitMaterial.SetTexture("_BrushTex", brushStats.BrushTex[
                Random.Range(0, brushStats.BrushTex.Length)]);
            }

            var radius = brushStats.Diameter * diameterRatio / 2f;
            var o = new Vector2(radius, radius);

            blitMaterial.SetVector("_sPos", p - o);

            UpdateTexture();
        }

        void UpdateTexture()
        {
            Graphics.Blit(renderTexture, bufferTexture, blitMaterial);
            Graphics.Blit(bufferTexture, renderTexture);
        }

        void GetPixelByMousePosition(Vector2 mPos, out Vector2 texturePos)
        {
            var iRect = drawImage.GetComponent<RectTransform>();
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(iRect, mPos
                , Camera.main, out Vector2 localCursor))
            {
                texturePos = Vector2.zero;
                return;
            }

            var mappedSize = localCursor / iRect.rect.size;
            texturePos = TextureDimensions * mappedSize;
        }
        #endregion
    }

    [System.Serializable]
    public class BrushStats
    {
        [SerializeField] Texture2D[] brushTex;
        [SerializeField] float alpha;

        [Space]
        [SerializeField] float minDis;
        [SerializeField] float maxDis;
        [SerializeField] float spawnDistance;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brushTex"></param>
        /// <param name="alpha">alpha trong khoang tu 0-1</param>
        /// <param name="minDis">khoang cach toi thieu giua 2 diem</param>
        /// <param name="maxDis">lon hon khoang cach nay se spawn them diem</param>
        /// <param name="spawnDistance">khoang cach giua cac diem spawn them</param>
		public BrushStats(Texture2D[] brushTex, float alpha = 1
            , float minDis = 2, float maxDis = 5, float spawnDistance = 3)
		{
			this.brushTex = brushTex;
			this.alpha = alpha;
			this.minDis = minDis;
			this.maxDis = maxDis;
			this.spawnDistance = spawnDistance;
		}

		public Texture2D[] BrushTex => brushTex;
        public float Alpha => alpha;
        public int Diameter => brushTex[0].width;

        // Khoang cach toi thieu giua 2 diem
        public float MinDis => minDis;

        // Lon hon khoang cach nay se spawn them diem
        public float MaxDis => maxDis;
        // Khoang cach giua cac diem spawn them
        public float SpawnDistance => spawnDistance;
    }
}