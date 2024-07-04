using DG.Tweening;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CharacterItem : CharacterPaletteItem
    {
        [SerializeField] CharacterFeatureCategoryEnum characterFeatureCategoryEnum;

        [SerializeField] Button btn;

        [SerializeField] RectTransform rect;

        [SerializeField] int id;

        [SerializeField] Image img;

        [SerializeField] Image imgChild;

        [SerializeField] Image imgFront;

        [SerializeField] Image imgIcon;

        [SerializeField] Sprite defaultItem;

        [SerializeField] Sprite noneImg;

        [SerializeField] Sprite noneImgDf;

        [SerializeField] CharacterFeatureAsset characterFeatureAsset;

        [SerializeField] Color defaultColor;

        [SerializeField] Color changeColor;

        [SerializeField] AudioSource audiobtn;

        [SerializeField] Transform adsImg;

        [SerializeField] Transform iapBtn;

        public CharacterFeatureCategoryEnum CharacterFeatureCategoryEnum { get => characterFeatureCategoryEnum; set => characterFeatureCategoryEnum = value; }
        public Button Btn { get => btn; set => btn = value; }
        public RectTransform Rect { get => rect; set => rect = value; }
        public int Id { get => id; set => id = value; }
        public Image Img { get => img; set => img = value; }
        public Sprite DefaultItem { get => defaultItem; set => defaultItem = value; }
        public Image ImgIcon { get => imgIcon; set => imgIcon = value; }
        public CharacterFeatureAsset CharacterFeatureAsset { get => characterFeatureAsset; set => characterFeatureAsset = value; }
        public AudioSource Audiobtn { get => audiobtn; set => audiobtn = value; }
        public Image ImgChild { get => imgChild; set => imgChild = value; }

        public override CharacterFeatureSet GetFeatureSet()
        {
            CharacterFeatureSet set = new CharacterFeatureSet();
            return set;
        }

        private void Awake()
        {
            
        }

        private void Start()
        {

        }
        
        private void OnEnable()
        {
            //
            if (characterFeatureAsset != null)
            {
                if (characterFeatureAsset.Icon != null)
                {
                    imgChild.sprite = noneImgDf;
                }
                else
                {
                    imgChild.sprite = noneImg;
                }
            }

            if (CharacterFeatureCategoryEnum == CharacterFeatureCategoryEnum.Skin)
            {
                imgFront.transform.DOScale(1.3f, 0f);
            }
            else
            {
                imgFront.transform.DOScale(1f, 0f);
            }
        }

        public override bool IsSelected()
        {
            return true;
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

        public void ChangeColor()
        {
            imgChild.color = changeColor;
        }
        public void ResetColor()
        {
            imgChild.color = defaultColor;
        }

        public void ActiveAds()
        {
            var data = DataCharacterManager.Instance.LocalData.HasCateItem(CharacterFeatureCategoryEnum);
            var item = data.HasFeature(id);
            
            // Follow IAP Button active
            if(characterFeatureAsset.Metadata.IsLockForPack)
            {
                if(characterFeatureAsset.Metadata.iAPPack.IsUnlock)
                {
                    iapBtn.gameObject.SetActive(false);
                }
                else
                {
                    iapBtn.gameObject.SetActive(true);
                }
                adsImg.gameObject.SetActive(false);
                return;
            }


            // Follow Ads Button active
            if (item || AdsManager.Instance.IsRemovedAds)
            {
                adsImg.gameObject.SetActive(false);
            }
            else
            {
                adsImg.gameObject.SetActive(true);
            }
            iapBtn.gameObject.SetActive(false);
        }
        public void SetDefaultColorEyebrows()
        {
            if (CharacterFeatureCategoryEnum == CharacterFeatureCategoryEnum.Eyebrows)
            {
                ImgIcon.color = Color.black;
            }
            else
            {
                ImgIcon.color = Color.white;
            }
        }

        public void FadeIcon()
        {
            if (characterFeatureAsset.Icon)
            {
                ImgIcon.DOFade(1f, 0f);
                ImgIcon.sprite = characterFeatureAsset.Icon;
            }
            else
            {
                ImgIcon.DOFade(0f, 0f);
            }
        }

        public void AddData()
        {
            var cate = DataCharacterManager.Instance.LocalData.HasCateItem(CharacterFeatureCategoryEnum);
            cate.AddFeature(id);
            adsImg.gameObject.SetActive(false);
        }
        public void AddDataInList()
        {
            var cate = DataCharacterManager.Instance.LocalData.HasCateItem(CharacterFeatureCategoryEnum);
            if (characterFeatureAsset.Metadata.IsFree)
            {
                if (!characterFeatureAsset.Metadata.IsLockForPack)
                {
                    if (!characterFeatureAsset.Metadata.iAPPack)
                    {
                        cate.AddFeature(id);
                    }
                }
                else
                {
                    //    cate.AddFeature(id);
                }
            }
        }

        public bool HasIDData()
        {
            var cate = DataCharacterManager.Instance.LocalData.HasCateItem(CharacterFeatureCategoryEnum);
            var itemId = cate.HasFeature(id);

            return itemId;
        }

        public void Skin2()
        {
            if (id == 2 && characterFeatureCategoryEnum == CharacterFeatureCategoryEnum.Skin)
            {
                imgIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 400);
            }
            else
            {
                imgIcon.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (Img == null)
            {
                Img = transform.GetComponent<Image>();
            }
            if (Btn == null)
            {
                Btn = transform.GetComponent<Button>();
            }
        }

#endif
    }
}
