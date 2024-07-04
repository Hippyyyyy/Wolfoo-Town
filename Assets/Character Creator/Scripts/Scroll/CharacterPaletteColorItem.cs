using DG.Tweening;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CharacterPaletteColorItem : CharacterPaletteItem
    {
        [SerializeField] int id;

        [SerializeField] CharacterColorSwatch colorSwatch;

        [SerializeField] CharacterColorCategory characterColorCategory;

        [SerializeField] float LerpAmount = 0.5f;

        [SerializeField] Image img;

        [SerializeField] Button btn;

        [SerializeField] AudioSource audiobtn;

        [SerializeField] Transform adsImg;
        public CharacterColorSwatch ColorSwatch { get => colorSwatch; set => colorSwatch = value; }
        public float LerpAmount1 { get => LerpAmount; set => LerpAmount = value; }
        public Image Img { get => img; set => img = value; }
        public Button Btn { get => btn; set => btn = value; }
        public int Id { get => id; set => id = value; }
        public CharacterColorCategory CharacterColorCategory { get => characterColorCategory; set => characterColorCategory = value; }
        public AudioSource Audiobtn { get => audiobtn; set => audiobtn = value; }
        public Transform AdsImg { get => adsImg; set => adsImg = value; }

        private void Start()
        {
            if (img)
            {
                var colorChange = Color.Lerp(colorSwatch.Color1, colorSwatch.Color2, LerpAmount);
                img.color = colorChange;
                img.DOFade(1f, 0f);
            }

        }

        private void OnEnable()
        {

        }
        Tween currentTweener;
        public void Scale()
        {
            DOTweenManager.Instance.KillTween(currentTweener);
            currentTweener = DOTweenManager.Instance.TweenScaleTime(
                transform, new Vector3(1.1f, 1.1f, 1.1f), 0.25f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    DOTweenManager.Instance.KillTween(currentTweener);
                    currentTweener = DOTweenManager.Instance.TweenScaleTime(transform, Vector3.one, 0.1f);
                });
        }
        public override CharacterFeatureSet GetFeatureSet()
        {
            CharacterFeatureSet featureSet = new CharacterFeatureSet();

            featureSet.HairColor = (ColorSwatch.Category == CharacterColorCategory.Hair) ? ColorSwatch.Color1 : featureSet.HairColor;
            featureSet.EyebrowsColor = (ColorSwatch.Category == CharacterColorCategory.Eyebrows) ? ColorSwatch.Color1 : featureSet.EyebrowsColor;

            return featureSet;
        }

        public void LerpColor()
        {
            if (img)
            {
                var colorChange = Color.Lerp(colorSwatch.Color1, colorSwatch.Color2, LerpAmount);
                img.color = colorChange;
                img.DOFade(1f, 0f);
            }
        }
        public void ActiveAds()
        {
            var data = DataCharacterManager.Instance.LocalData.HasCateColorItem(characterColorCategory);
            var item = data.HasFeature(id);

            if (item || AdsManager.Instance.IsRemovedAds)
            {
                AdsImg.gameObject.SetActive(false);
            }
            else
            {
                AdsImg.gameObject.SetActive(true);
            }
        }
        public void AddData()
        {
            var cate = DataCharacterManager.Instance.LocalData.HasCateColorItem(characterColorCategory);
            cate.AddFeature(id);
            adsImg.gameObject.SetActive(false);
        }
        public void AddDataInList()
        {
            var cate = DataCharacterManager.Instance.LocalData.HasCateColorItem(characterColorCategory);
            if (ColorSwatch.IsFree)
            {
                cate.AddFeature(id);
            }
        }
        public bool HasIDData()
        {
            var cate = DataCharacterManager.Instance.LocalData.HasCateColorItem(CharacterColorCategory);
            var itemId = cate.HasFeature(id);
            return itemId;
        }
        public override bool IsSelected()
        {
            return true;
        }
        public Color GetColor()
        {
            return default(Color);
        }


    }
}
