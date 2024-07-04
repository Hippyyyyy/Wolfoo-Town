using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using _WolfooShoppingMall;
using System;

[AddComponentMenu("UI/Effects/Gradient")]
public class SliderController : BaseMeshEffect
{
    public Color m_color1 = Color.white;
    public Color m_color2 = Color.white;
    [Range(-180f, 180f)]
    public float m_angle = 0f;
    public bool m_ignoreRatio = true;

    public Slider colorSlider; // Thêm Slider vào Inspector và kết nối nó với đây

    [SerializeField] CharacterPaletteColorItem colorItem;
    Vector3 defaultPos;

    protected override void Start()
    {
        base.Start();
        defaultPos = colorSlider.transform.localPosition;
        // Đăng ký sự kiện để lắng nghe sự thay đổi từ Slider

        // Gọi hàm để cập nhật màu theo giá trị hiện tại của Slider
        if (colorItem)
        {
            UpdateGradientColor(colorSlider.handleRect.GetComponent<Image>(), colorItem.ColorSwatch.Color1, colorItem.ColorSwatch.Color2, colorSlider.value);
        }
    }

    public void Init(Action callback)
    {
        colorSlider.onValueChanged.AddListener((float value) => OnColorSliderChanged(value, callback));
    }
    public void Remove(Action callback)
    {
        colorSlider.onValueChanged.RemoveListener((float value) => OnColorSliderChanged(value, callback));
    }

    public void OnColorSliderChanged(float sliderValue, Action callback)
    {
        // Gọi hàm để cập nhật màu theo giá trị của Slider
        UpdateGradientColor(colorSlider.handleRect.GetComponent<Image>(), colorItem.ColorSwatch.Color1, colorItem.ColorSwatch.Color2, colorSlider.value);
        SetColorItem(colorItem);
        colorItem.LerpAmount1 = colorSlider.value;
        callback();
    }

    private void UpdateGradientColor(Image img, Color color1, Color color2, float sliderValue)
    {
        // Lấy giá trị màu tương ứng từ việc lerp giữa m_color1 và m_color2
        Color lerpedColor = Color.Lerp(color1, color2, sliderValue);
        colorSlider.handleRect.GetComponent<Image>().color = lerpedColor;
        // Cập nhật m_color1 để áp dụng màu mới
        img.color = lerpedColor;
    }
    public void SetColorItem(CharacterPaletteColorItem colorItem)
    {
        UpdateGradientColor(colorItem.Img, colorItem.ColorSwatch.Color1, colorItem.ColorSwatch.Color2, colorSlider.value);

    }

    /*public void SliceMoved()
    {
        colorSlider.transform.DOMove(Vector2.one * 104f, 0.4f);
    }*/

    public override void ModifyMesh(VertexHelper vh)
    {
        if (enabled)
        {
            Rect rect = graphic.rectTransform.rect;
            Vector2 dir = UIGradientUtils.RotationDir(m_angle);

            if (!m_ignoreRatio)
                dir = UIGradientUtils.CompensateAspectRatio(rect, dir);

            UIGradientUtils.Matrix2x3 localPositionMatrix = UIGradientUtils.LocalPositionMatrix(rect, dir);

            UIVertex vertex = default(UIVertex);
            for (int i = 0; i < vh.currentVertCount; i++)
            {
                vh.PopulateUIVertex(ref vertex, i);
                Vector2 localPosition = localPositionMatrix * vertex.position;
                vertex.color *= Color.Lerp(m_color2, m_color1, localPosition.y);
                vh.SetUIVertex(vertex, i);
            }
        }
    }

    public void SetColor(CharacterPaletteColorItem item)
    {
        colorItem = item;
        m_color1 = colorItem.ColorSwatch.Color1;
        m_color2 = colorItem.ColorSwatch.Color2;
        colorSlider.value = colorItem.LerpAmount1;
        colorSlider.handleRect.GetComponent<Image>().color = Color.Lerp(m_color2, m_color1, colorSlider.value);
        transform.GetComponent<Image>().DOFade(0.5f, 0f).OnComplete(()=> {
            transform.GetComponent<Image>().DOFade(1f, 0f);
        });
    }
    public void SetColor(CharacterColorSwatch colorSwatch, CharacterController character)
    {
        m_color1 = colorSwatch.Color1;
        m_color2 = colorSwatch.Color2;
        var color = DataCharacterManager.Instance.LocalData.ListCharacters[character.Id].GetColorByCategory(colorSwatch.Category);
        float lerp = CalculateLerpValue(m_color1, m_color2, (Color)color);
        colorSlider.handleRect.GetComponent<Image>().color = Color.Lerp(m_color1, m_color2, lerp);
        transform.GetComponent<Image>().DOFade(0.5f, 0f).OnComplete(() => {
            transform.GetComponent<Image>().DOFade(1f, 0f);
        });
    }
    public float CalculateLerpValue(Color color1, Color color2, Color original)
    {
        // Tính toán giá trị lerp 
        float lerpValue = Mathf.InverseLerp(color1.a, color2.a, original.a);
        return lerpValue;
    }
}

