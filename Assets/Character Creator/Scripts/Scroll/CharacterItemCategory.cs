using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CharacterItemCategory : MonoBehaviour
    {
        [SerializeField] CharacterFeatureCategoryEnum characterFeatureCategoryEnum;

        [SerializeField] CharacterColorCategory characterColor;

        [SerializeField] AgeSetting ageSetting;

        [SerializeField] CharacterFeatureCategory characterFeatureCategory;

        [SerializeField] Button btn;

        [SerializeField] RectTransform rect;

        [SerializeField] int id;

        [SerializeField] Image img;

        [SerializeField] bool isColorPicker;

        [SerializeField] bool isBodyType;

        [SerializeField] Color DefaultColor;

        [SerializeField] Color SelectedColor;

        [SerializeField] Image item;
        public CharacterFeatureCategory CharacterFeatureCategory { get => characterFeatureCategory; set => characterFeatureCategory = value; }
        public Button Btn { get => btn; set => btn = value; }
        public RectTransform Rect { get => rect; set => rect = value; }
        public CharacterColorCategory CharacterColor { get => characterColor; set => characterColor = value; }
        public bool IsColorPicker { get => isColorPicker; set => isColorPicker = value; }
        public bool IsBodyType { get => isBodyType; set => isBodyType = value; }
        public Image Img { get => img; set => img = value; }
        public Image Item { get => item; set => item = value; }
        public CharacterFeatureCategoryEnum CharacterFeatureCategoryEnum { get => characterFeatureCategoryEnum; set => characterFeatureCategoryEnum = value; }

        private void Start()
        {
            if (CharacterFeatureCategoryEnum == CharacterFeatureCategoryEnum.Skin && !IsColorPicker)
            {
                IsBodyType = true;
            }
            else
            {
                IsBodyType = false;
            }
        }
        
        public void ChangeItemColor(CharacterController characterController)
        {
            //var data = DataCharacterManager.Instance.LocalData.ListCharacters[characterController.Id];
            var characterChoose = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == characterController.Id);
            var color = characterChoose.GetColorByCategory(characterColor);
            Item.color = (Color)color;
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Img == null) Img = transform.GetComponent<Image>();
            if (Btn == null) Btn = transform.GetComponent<Button>();
            if (Rect == null) Rect = transform.GetComponent<RectTransform>();
        }
#endif
    }

}
