using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Rendering;
using System;

namespace _WolfooShoppingMall
{
    public class CameraItem : BackItem
    {
        [SerializeField] Image captureImg;
        [SerializeField] Image previewImg;
        [SerializeField] Image printedImg;
        [SerializeField] CapturePicture capturePicturePb;
        [SerializeField] Transform pictureOutZone;
        [SerializeField] Image contentImg;
        [SerializeField] Button captureBtn;

        private bool isCapturing;
        private RenderTexture textureRenderer;
        private Texture2D textureCapture;
        private Sprite spriteCapture;
        private Tweener fadeTween;
        private Tweener fadeTween2;
        Camera myCamera;

        protected override void InitItem()
        {
            canDrag = true;
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
            captureBtn.onClick.AddListener(OnCaptureClick);
            //  captureImg.type = Image.Type.Tiled;
            myCamera = GUIManager.instance.MainCamera;
            previewImg.type = Image.Type.Simple;
            captureImg.DOFade(0, 0);

            textureCapture = new Texture2D(
                Screen.width,
                Screen.height);
            StartCoroutine(GetPhoto());
        }

        private void OnCaptureClick()
        {
            if (!canClick) return;
            canClick = false;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Capture);
            fadeTween = captureImg.DOFade(1, 0.5f).OnComplete(() =>
            {
                fadeTween2 = captureImg.DOFade(0, 0.5f);
                UISetupManager.Instance.maskBg.gameObject.SetActive(true);
                fadeTween = UISetupManager.Instance.maskBg.DOFade(0.8f, 0.25f).SetEase(Ease.Flash).OnComplete(() =>
                {
                    fadeTween = UISetupManager.Instance.maskBg.DOFade(0, 0.25f).SetEase(Ease.Flash).OnComplete(() =>
                    {
                        var picture = Instantiate(capturePicturePb, transform);
                        picture.transform.position = previewImg.transform.position;
                        picture.AssginItem(previewImg.sprite);
                        picture.OnCaptured(pictureOutZone.localPosition, () =>
                        {
                            picture.transform.SetParent(Content.transform);
                            UISetupManager.Instance.maskBg.gameObject.SetActive(false);
                            canClick = true;
                        });
                    });
                });
            });
        }

        IEnumerator GetPhoto()
        {
            yield return new WaitForEndOfFrame();

            textureRenderer = myCamera.targetTexture;

            var rect = new Rect(0, 0, Screen.width, Screen.height);

            textureCapture.ReadPixels(rect, 0, 0);
            textureCapture.Apply();

            spriteCapture = Sprite.Create(
                textureCapture, rect,
                new Vector2(0.5f, 0.5f), 1100);


            previewImg.sprite = spriteCapture;
            GameManager.instance.ScaleImage(previewImg, previewImg.rectTransform.sizeDelta.x, previewImg.rectTransform.sizeDelta.y);

            textureRenderer = null;
        }
        private void OnPostRender()
        {
            if (!isCapturing) return;

            //isCapturing = false;
            //var rect = new Rect(
            //    (int)0,
            //    (int)0,
            //    Screen.width,
            //    Screen.height);

            //textureCapture.ReadPixels(rect, 0, 0);
            //textureCapture.Apply();

            //var texture3 = new Rect(0, 0, Screen.width, Screen.height);

            //spriteCapture = Sprite.Create(
            //    textureCapture, texture3,
            //    new Vector2(0.5f, 0.5f), 1100);
            //previewImg.sprite = spriteCapture;
            //previewImg.SetNativeSize();
            //textureRenderer = null;
        }
        void RawCamera()
        {
            textureRenderer = myCamera.targetTexture;
            isCapturing = true;
        }

        protected override void GetDragItem(EventKey.OnDragBackItem item)
        {
            base.GetDragItem(item);

        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            StartCoroutine(GetPhoto());
            // RawCamera();
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == null) return;
            if (item.backitem.id == id) return;

            //     StartCoroutine(GetPhoto());
            // RawCamera();
        }
    }
}