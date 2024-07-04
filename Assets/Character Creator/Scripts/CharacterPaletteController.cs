using _WolfooShoppingMall;
using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPaletteController : MonoBehaviour
{
    [SerializeField] List<ExpandedSizeItem> expandedSizeItems;

    [SerializeField] List<CharacterItemCategory> characterItemCategories;

    [SerializeField] List<CharacterItemCategory> characterItemColorCategories;

    [SerializeField] List<CharacterItem> characterItems;

    CharacterItemCategory characterItemCategorySelect;

    [SerializeField] List<CharacterPaletteColorItem> characterPaletteColorItems;

    [SerializeField] ScrollRect scrollRect;

    [SerializeField] RectTransform content;

    [SerializeField] RectTransform contentScrollShow;

    [SerializeField] RectTransform CharacterPaletteItem;

    AgeSetting age;

    [SerializeField] Button btnRandom;

    [SerializeField] CharacterController SelectedCharacter;

    [SerializeField] int indexCharacter = 0;

    [SerializeField] CharacterPaletteItem characterItem;

    [SerializeField] CharacterPaletteColorItem characterColorItem;

    [SerializeField] GridLayoutGroup gridLayoutGroup;

    [SerializeField] SliderController sliderController;
    
    int selectedItem;

    int CONSTRAINTCOUNT_COLOR = 5;

    int CONSTRAINTCOUNT_FEATURES = 3;

    int CONSTRAINTCOUNT_BODYTYPE = 2;

    int CELLSIZE_COLOR = 170;

    int CELLSIZE_FEATURES = 300;

    int CELLSIZE_FEATURES_BODY_TYPE = 500;

    public static CharacterPaletteController Ins;

    private void Awake()
    {
        SetUpSlider();
        Ins = this;
    }


    void SetUpSlider()
    {
        layoutElement.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
        DOVirtual.DelayedCall(0.1f, () =>
        {
            layoutElement.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
        });
        btnRandom.transform.DOScale(0f, 0f);
        StartCoroutine(StartAnimating(layoutElement.preferredHeight, _expandedSize, () => { Debug.Log("slider" + _expandedSize); }));
        
    }
    private void OnEnable()
    {
        EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(GetWatchAds);
    }
    private void OnDisable()
    {
        EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(GetWatchAds);
    }

    private void Start()
    {
        InitItemScroll();
        InitCharacterItemCategory();
    }

    void InitItemScroll()
    {
        for (int i = 0; i < expandedSizeItems.Count; i++)
        {
            expandedSizeItems[i].Init();
        }
    }

    public CharacterController GetCharacter(CharacterController characterController)
    {
        Debug.Log("Testing GetCharacter : " + characterController);
        if (characterController == null) return SelectedCharacter;
        SelectedCharacter = characterController;
        return SelectedCharacter;
    }

    public void ClickItemToShow()
    {
        for (int i = 0; i < expandedSizeItems.Count; i++)
        {
            if (expandedSizeItems[i].IsClick)
            {
                expandedSizeItems[i].HideItem();
            }
        }
    }

    private void GetWatchAds(EventKey.OnWatchAds obj)
    {
        if (obj.instanceID == GetInstanceID())
        {
            var item = obj.characterItem;
            var colorItem = obj.characterPaletteColorItem;

            if(item != null)
            {
                PerformCharacterFeatureSelection(item);
                item.ActiveAds();
            }
            else if(colorItem != null)
            {
                PerformCharacterFeatureColorSelection(colorItem);
                colorItem.ActiveAds();
            }
        }
    }
    public void InitCharacterItemCategory()
    {
        for (int i = 0; i < characterItemCategories.Count; i++)
        {
            var item = characterItemCategories[i];
            item.Btn.onClick.AddListener(() =>
            {
                characterItemCategorySelect = item;
                ChangeRow(item.IsColorPicker, item.IsBodyType);
                SetUp(item, item.IsColorPicker);
                if (item.CharacterFeatureCategoryEnum == CharacterFeatureCategoryEnum.Skin)
                {
                    var age = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == SelectedCharacter.Id).Age;
                    ResetColor((int)age);
                }
            });
        }
    }
    public void InitCharacterItem()
    {
        foreach (var item in characterItems)
        {
            item.Btn.onClick.AddListener(() =>
            {
                if (item.HasIDData())
                {
                    PerformCharacterFeatureSelection(item);
                }
                else
                {
                    /// Check IAP Data
                    if (item.CharacterFeatureAsset.Metadata.IsLockForPack)
                    {
                        var isUnlocked = item.CharacterFeatureAsset.Metadata.iAPPack.IsUnlock;
                        if(!isUnlocked)
                        {
                            /// Open Panel IAP
                            Debug.Log("Open IAP Panel");
                        }
                        else
                        {
                            PerformCharacterFeatureSelection(item);
                        }
                    }
                    else
                    {
                        EventDispatcher.Instance.Dispatch(
                            new EventKey.InitAdsPanel()
                            {
                                spriteItem = item.ImgIcon.sprite,
                                instanceID = GetInstanceID(),
                                characterItem = item,
                                color = item.ImgIcon.color,
                                curPanel = item.CharacterFeatureCategoryEnum.ToString(),
                                nameObj = item.CharacterFeatureAsset.Icon.name,
                            });
                        GUIManager.instance.OpenPanel(PanelType.Ads);
                    }
                }
            });
        }
    }
    public void InitCharacterColorItem()
    {
        foreach (var item in characterPaletteColorItems)
        {
            item.Btn.onClick.AddListener(() =>
            {
                
                if (item.HasIDData())
                {
                    PerformCharacterFeatureColorSelection(item);
                }
                else
                {
                    Debug.Log("Color Item!");
                    EventDispatcher.Instance.Dispatch(
                        new EventKey.InitAdsPanel() { 
                            spriteItem = item.Img.sprite, 
                            instanceID = GetInstanceID(), 
                            characterPaletteColorItem = item,
                            color = item.Img.color,
                            curPanel = "ColorItem",
                            nameObj = item.GetColor().grayscale.ToString(),
                        });
                    GUIManager.instance.OpenPanel(PanelType.Ads);
                }
            });
        }
    }
    void PerformCharacterFeatureColorSelection(CharacterPaletteColorItem item)
    {
        item.Audiobtn.Play();
        item.ColorSwatch.IsFree = true;
        item.AddData();
        var itemCategory = characterItemColorCategories.Find(x => x.CharacterColor == item.CharacterColorCategory);
        item.Scale();
        sliderController.SetColor(item);
        sliderController.Init(() =>
        {
            SelectedCharacter.CharacterPartHelper.SetColor(item.CharacterColorCategory, item.Img.color);
            DataCharacterManager.Instance.LocalData.ListCharacters[SelectedCharacter.Id].SetColorByCategory(item.CharacterColorCategory, item.Img.color);
            itemCategory.Item.color = item.Img.color;
        });
        DataCharacterManager.Instance.LocalData.ListCharacters[SelectedCharacter.Id].SetColorByCategory(item.CharacterColorCategory, item.Img.color);
        DataCharacterManager.Instance.LocalData.ListCharacters[SelectedCharacter.Id].SetIdColorByCategory(item.CharacterColorCategory, item.Id);
        SelectedCharacter.CharacterPartHelper.SetColor(item.CharacterColorCategory, item.Img.color);
        itemCategory.Item.color = item.Img.color;
    }
    
    void PerformCharacterFeatureSelection(CharacterItem item)
    {
        Debug.Log(DataCharacterManager.Instance.LocalData);
        var data = DataCharacterManager.Instance.LocalData;
        Debug.Log(data.ListCharacters.Find(x => x.CharacterID == SelectedCharacter.Id));
        var character = data.ListCharacters.Find(x => x.CharacterID == SelectedCharacter.Id);
        //DataCharacterManager.Instance.LocalData.ActiveBool(item.CharacterFeatureCategoryEnum, item.CharacterFeatureAsset.ID);
        item.AddData();
        item.Audiobtn.Play();//
        item.Scale();
        selectedItem = item.Id;
        item.ChangeColor();
        ResetColor(selectedItem);
        if (item.CharacterFeatureCategoryEnum == CharacterFeatureCategoryEnum.Skin)
        {
            character.SetFeatureByCategory(item.CharacterFeatureCategoryEnum, item.CharacterFeatureAsset.ID);
            SelectedCharacter.CharacterPartHelper.SetFeatureItem(item.CharacterFeatureCategoryEnum, item.CharacterFeatureAsset);
        }
        else
        {
            character.SetFeatureByCategory(item.CharacterFeatureCategoryEnum, item.CharacterFeatureAsset.ID);
            SelectedCharacter.CharacterPartHelper.SetFeatureItem(item.CharacterFeatureCategoryEnum, item.CharacterFeatureAsset);
        }
    }

    public void SetUp(CharacterItemCategory itemCategory, bool IsItemColor)
    {
        foreach (var paletteColorItem in characterPaletteColorItems)
        {
            if (paletteColorItem)
            {
                paletteColorItem.gameObject.SetActive(false);
            }
        }

        foreach (var characterItem in characterItems)
        {
            if (characterItem)
            {
                characterItem.gameObject.SetActive(false);
            }

        }

        if (IsItemColor)
        {
            int count = itemCategory.CharacterFeatureCategory.characterFeatureFilter.filteredColorSwatch.Count;
            while (characterPaletteColorItems.Count < count)
            {
                characterPaletteColorItems.Add((CharacterPaletteColorItem)Instantiate(characterColorItem, contentScrollShow));
            }

            var featureFilter = itemCategory.CharacterFeatureCategory.characterFeatureFilter;

            foreach (var item in characterPaletteColorItems)
            {
                item.gameObject.SetActive(false);
            }

            var sp = featureFilter.filteredColorSwatch;

            for (int i = 0; i < count; i++)
            {
                characterPaletteColorItems[i].Id = i;
                characterPaletteColorItems[i].ColorSwatch = sp[i];
                characterPaletteColorItems[i].LerpColor();
                characterPaletteColorItems[i].CharacterColorCategory = sp[i].Category;
                characterPaletteColorItems[i].AddDataInList();
                characterPaletteColorItems[i].ActiveAds();
                characterPaletteColorItems[i].gameObject.SetActive(true);
            }
            InitCharacterColorItem();
            /*
                        var colorID = DataCharacterManager.Instance.LocalData.FindColorID(SelectedCharacter.Id, itemCategory.CharacterColor);
                        var color = itemCategory.CharacterFeatureCategory.characterFeatureFilter.filteredColorSwatch[colorID];
                        sliderController.SetColor(color, SelectedCharacter);*/
            layoutElement.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
            DOVirtual.DelayedCall(0.03f, () =>
            {
                layoutElement.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
            });
            StartCoroutine(StartAnimating(layoutElement.preferredHeight, _nonExpandedSize, () => { Debug.Log("itemColor" + _expandedSize); }));
        }
        else
        {
            int count = itemCategory.CharacterFeatureCategory.characterFeatureFilter.filteredFeatures.Count;
            while (characterItems.Count < count)
            {
                characterItems.Add((CharacterItem)Instantiate(characterItem, contentScrollShow));
            }
            
            var featureFilter = itemCategory.CharacterFeatureCategory.characterFeatureFilter;

            var sp = /*age == AgeSetting.Baby ? featureFilter.filteredBabyFeatures : */featureFilter.filteredFeatures;

            foreach (var item in characterItems)
            {
                item.gameObject.SetActive(false);
            }
            for (int i = 0; i < count; i++)
            {
                characterItems[i].Id = sp[i].ID;
                if (sp[i].Icon)
                {
                    characterItems[i].ImgIcon.DOFade(1f, 0f);
                    characterItems[i].ImgIcon.sprite = sp[i].Icon;
                }
                else
                {
                    characterItems[i].ImgIcon.DOFade(0f, 0f);
                }
                characterItems[i].CharacterFeatureCategoryEnum = sp[i].Category;
                characterItems[i].CharacterFeatureAsset = itemCategory.CharacterFeatureCategory.characterFeatureFilter.filteredFeatures[i];
                characterItems[i].AddDataInList();
                characterItems[i].ActiveAds();
                characterItems[i].SetDefaultColorEyebrows();
                characterItems[i].Skin2();
                characterItems[i].Img.preserveAspect = true;
                characterItems[i].gameObject.SetActive(true);

            }
            InitCharacterItem();
            layoutElement.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
            DOVirtual.DelayedCall(0.03f, () =>
            {
                layoutElement.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
            });
            StartCoroutine(StartAnimating(layoutElement.preferredHeight, _expandedSize, ()=> { Debug.Log("item" + _expandedSize); }));
            
            var cate = itemCategory.CharacterFeatureCategoryEnum;
            var data = DataCharacterManager.Instance.LocalData;
            DOVirtual.DelayedCall(0.01f, () => {
                if (data.ListCharacters.Find(x => x.CharacterID == SelectedCharacter.Id) != null)
                {
                    var id = data.ListCharacters.Find(x => x.CharacterID == SelectedCharacter.Id).GetFeatureByCategory(cate);
                    ResetColor(id);
                }
                
            });
        }
    }
    //
    public void ResetColor(int id)
    {
        foreach (var item in characterItems)
        {
            if (item.Id != id)
            {
                item.ResetColor();
            }
            else
            {
                item.ChangeColor();
            }
        }
    }

    public void ChangeRow(bool IsColorItem, bool IsBodyTypeItem)
    {
        int cellSize;
        int constraintCount;
        if (IsColorItem)
        {
            cellSize = CELLSIZE_COLOR;
            constraintCount = CONSTRAINTCOUNT_COLOR;
        }
        else if (IsBodyTypeItem)
        {
            cellSize = CELLSIZE_FEATURES_BODY_TYPE;
            constraintCount = CONSTRAINTCOUNT_BODYTYPE;
        }
        else
        {
            cellSize = CELLSIZE_FEATURES;
            constraintCount = CONSTRAINTCOUNT_FEATURES;
        }

        Vector2 cellSizeVector = new Vector2(cellSize, cellSize);

        gridLayoutGroup.cellSize = cellSizeVector;
        gridLayoutGroup.constraintCount = constraintCount;

    }

    float _expandedSize = 957f;
    [SerializeField] LayoutElement layoutElement;
    float _nonExpandedSize = 736f;

    IEnumerator StartAnimating(float from, float to, System.Action onDone)
    {
        float startTime = Time.unscaledTime;
        float elapsed;
        float t01;
        do
        {
            yield return null;

            elapsed = Time.unscaledTime - startTime;
            t01 = Mathf.Clamp01(elapsed / 0.1f);
            t01 = Mathf.Sqrt(t01);

            layoutElement.preferredHeight = Mathf.Lerp(from, to, t01);

        }
        while (t01 < 1f);

        if (onDone != null)
            onDone();
    }

    public void Show(float duration = 1f)
    {
        transform.GetComponent<RectTransform>().DOAnchorPosY(100f, duration);
        CharacterPaletteItem.DOAnchorPosX(-562f, duration);
        ChangeRow(false, true);
        SetUp(characterItemCategories[0], false);
        characterItemCategorySelect = characterItemCategories[0];
        //StartCoroutine(StartAnimating(layoutElement.preferredHeight, 957f, null));
        DOVirtual.DelayedCall(0.1f, ()=> {
            var age = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == SelectedCharacter.Id).Age;
            ResetColor((int)age);
        }); 
        
        //
    }
    public void Hide(float duration = 1f)
    {
        transform.GetComponent<RectTransform>().DOAnchorPosY(-109f, duration);
        CharacterPaletteItem.DOAnchorPosX(564f, duration);
    }

    public void InitButtonRandomItemCharacter()
    {
        btnRandom.transform.DOScale(1f, 0.4f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            btnRandom.onClick.AddListener(() => { SetRandomItem(SelectedCharacter); ChangeImgForItemCategoryColor(); });
        });
    }
    public void RemoveButtonRandomItemCharacter()
    {
        btnRandom.onClick.RemoveListener(() => { SetRandomItem(SelectedCharacter); ChangeImgForItemCategoryColor(); });
        btnRandom.transform.DOScale(0f, 0.2f).SetEase(Ease.InBack);
    }
    public void SetRandomItem(CharacterController character)
    {
        var age = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == character.Id).Age;
        var characterChoose = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == character.Id);
        var library = CharacterFeatureLibraryManager.Instance.CharacterFeatureLibrary;
        var featureEyes = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Eyes);
        var featureEyebrows = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Eyebrows);
        var featureClothes = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Clothes);
        var featureMouth = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Mouth);
        var featureNose = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Nose);
        var featureHair = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Hair);
        var featureHat = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Hat);
        var featureMask = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Mask);
        var featureAccess = library.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.AccessoryHand);
        var colorEyes = library.RandomFilterColor(CharacterColorCategory.Eyes);
        var colorEyebrows = library.RandomFilterColor(CharacterColorCategory.Eyebrows);
        var colorHair = library.RandomFilterColor(CharacterColorCategory.Hair);
        
        var dataset = new CharacterFeatureLibrary.Data
                (age, characterChoose.SkinColor, characterChoose.HairColor, characterChoose.EyebrowsColor, characterChoose.EyesColor,
                featureEyes, featureEyebrows, featureMouth, featureNose, featureHair, featureClothes, featureHat, featureMask, featureAccess);
        character.CharacterPartHelper.Assign(dataset);

        character.CharacterPartHelper.SetEyes(featureEyes);
        character.CharacterPartHelper.SetEyebrows(featureEyebrows);
        character.CharacterPartHelper.SetClothes((CharacterFeatureClothesAsset)featureClothes);
        character.CharacterPartHelper.SetMouth(featureMouth);
        character.CharacterPartHelper.SetNose(featureNose);
        character.CharacterPartHelper.SetHat(featureHat);
        character.CharacterPartHelper.SetMask(featureMask);
        character.CharacterPartHelper.SetAccessoryInHand(featureAccess);
        character.CharacterPartHelper.SetHair((CharacterFeatureHairAsset)featureHair);

        character.CharacterPartHelper.SetColor(colorEyes.Category, colorEyes.Color2);
        character.CharacterPartHelper.SetColor(colorEyebrows.Category, colorEyebrows.Color2);
        character.CharacterPartHelper.SetColor(colorHair.Category, colorHair.Color2);

        characterChoose.EyesID = featureEyes.ID;
        characterChoose.EyebrowsID = featureEyebrows.ID;
        characterChoose.ClothingID = featureClothes.ID;
        characterChoose.MouthID = featureMouth.ID;
        characterChoose.NoseID = featureNose.ID;
        characterChoose.HairID = featureHair.ID;
        characterChoose.HatID = featureHat.ID;
        characterChoose.MaskID = featureMask.ID;
        characterChoose.AccessoryInHandID = featureAccess.ID;

        characterChoose.EyesColor = colorEyes.Color2;
        characterChoose.EyebrowsColor = colorEyebrows.Color2;
        characterChoose.HairColor = colorHair.Color2;

        if (characterItemCategorySelect != null)
        {
            if (!characterItemCategorySelect.IsColorPicker)
            {
                var cate = characterItemCategorySelect.CharacterFeatureCategory.FeatureCategories[0];
                var data = DataCharacterManager.Instance.LocalData;
                var id = data.ListCharacters.Find(x => x.CharacterID == character.Id).GetFeatureByCategory(cate);
                var item = characterItems.Find(x => x.Id == id);
                ResetColor(id);
            }//
        }
        
    }

    public List<CharacterItemCategory> GetAllItemCategoryColor()
    {
        return characterItemCategories.FindAll(x => x.IsColorPicker == true);
    }

    public void ChangeImgForItemCategoryColor()
    {
        List<CharacterItemCategory> list = new List<CharacterItemCategory>();
        list = GetAllItemCategoryColor();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].ChangeItemColor(SelectedCharacter);
        }
    }
}
