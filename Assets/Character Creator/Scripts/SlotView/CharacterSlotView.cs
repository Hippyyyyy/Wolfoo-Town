using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SCN.Common;
using UnityEngine.EventSystems;
using System;
using SCN;

namespace _WolfooShoppingMall
{
    public class CharacterSlotView : MonoBehaviour
    {
        [SerializeField] int id;

        [SerializeField] CharacterController characterPrefab;

        CharacterController currentCharacter;

        [SerializeField] Transform characterPosition;

        [SerializeField] CharacterFeatureLibrary characterFeatureLibrary;

        [SerializeField] Button btn;

        [SerializeField] RectTransform floop;

        [SerializeField] Transform parAddx10;

        [SerializeField] RectTransform emptyCharacter;

        [SerializeField] ButtonResetCharacter btnReset;

        [SerializeField] ButtonAddCharacter btnAdd;

        [SerializeField] List<RectTransform> floops;

        bool isSelected;

        bool isCreated;

        bool isDragging;

        bool isAds;

        Vector3 defaultPos;

        bool isFirstClick = true;

        public int Id { get => id; set => id = value; }
        public Button Btn { get => btn; set => btn = value; }
        public bool IsSelected { get => isSelected; set => isSelected = value; }
        public Vector3 DefaultPos { get => defaultPos; set => defaultPos = value; }
        public bool IsFirstClick { get => isFirstClick; set => isFirstClick = value; }
        public CharacterController CurrentCharacter { get => currentCharacter; set => currentCharacter = value; }
        public ButtonAddCharacter BtnAdd { get => btnAdd; set => btnAdd = value; }
        public bool IsAds { get => isAds; set => isAds = value; }
        public Transform ParAddx10 { get => parAddx10; set => parAddx10 = value; }
        public bool IsDragging { get => isDragging; set => isDragging = value; }

        private void Awake()
        {

        }

        private void Start()
        {
            DataCharacterManager.Instance.LocalData.CheckListCharacter();
            DataCharacterManager.Instance.LocalData.CheckListCharacterAds();
            Init();
            //SetUp();
            InitBtnReset();
            CheckButtonAds();
            CheckCreatedCharacter();
        }
        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(GetWatchAds);
        }
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(GetWatchAds);
        }
        private void GetWatchAds(EventKey.OnWatchAds obj)
        {
            if (obj.instanceID == GetInstanceID())
            {
                Debug.Log("Ads Watched: " + obj.idxItem);
                var id = obj.idxItem;
                AddCharacter();
                IsAds = false;
                DataCharacterManager.Instance.LocalData.AddCharacterAds(id);
            }
        }
        //
        public void Init()
        {
            btn.onClick.AddListener(() =>
            {
                
                CheckCharacterLock();
            });
        }

        void CheckButtonAds()
        {
            var characterId = DataCharacterManager.Instance.LocalData.IsCharacterIdExist(id);
            var idUnlock = DataCharacterManager.Instance.LocalData.UnlockCharacter;//
            if (!characterId)
            {
                if (id > idUnlock && !AdsManager.Instance.IsRemovedAds)
                {
                    BtnAdd.ChangeSprite();
                    IsAds = true;
                }
                else
                {
                    BtnAdd.ChangeCurrentSprite();
                    IsAds = false;
                }
                if (id > 4)
                {
                    BtnAdd.ChangeCurrentSprite();
                    IsAds = false;
                }
            }
        }

        public void RemoveCharacter()
        {
            btnReset.ScaleIn(() =>
            {
                DataCharacterManager.Instance.LocalData.RemoveCharacter(id);
                Destroy(CurrentCharacter.gameObject);
                CurrentCharacter.gameObject.SetActive(false);
                isCreated = false;
                emptyCharacter.gameObject.SetActive(true);
                BtnAdd.gameObject.SetActive(true);
                BtnAdd.ChangeCurrentSprite();
                btnReset.gameObject.SetActive(false);
                btnReset.transform.DOScale(0.5f, 0f);
                btnReset.ResetFillAmount();
            });

        }

        public void EnableButton(bool isButton)
        {
            btn.enabled = isButton;
            btnReset.enabled = isButton;
        }

        public void SetUp()
        {
            CheckShow(IsSelected);
        }


        public void InitBtnReset()
        {
            btnReset.CustomButton.OnPointerDown.AddListener(BtnResetRemove);
            btnReset.CustomButton.OnPointerUp.AddListener(btnReset.OnPointerUp);
        }

        public void BtnResetRemove()
        {
            btnReset.OnPointerDown(() => RemoveCharacter());
        }

        void CheckShow(bool select)
        {
            if (select)
            {
                ShowFloop();
            }
            else
            {
                HideFloop();
            }
        }

        public void CheckCharacterLock()
        {
            if (IsAds)
            {
                Debug.Log("Ads Init: " + id);
                EventDispatcher.Instance.Dispatch(
                    new EventKey.InitAdsPanel()
                    {
                        instanceID = GetInstanceID(),
                        spriteItem = emptyCharacter.GetComponent<Image>().sprite,
                        idxItem = id,
                        curPanel = "PlayerScroll",
                        nameObj = "UnlockNewCharacterHolder"
                    });
                GUIManager.instance.OpenPanel(PanelType.Ads);
            }
            else
            {
                AddCharacter();
            }
        }

        public void AddCharacter()
        {
            if (IsFirstClick)
            {
                CharacterSlotsView.OnSelected?.Invoke();
                IsSelected = true;
                IsFirstClick = false;
                CheckShow(IsSelected);
            }
            else
            {
                if (!isAds)
                {
                    SecondClickAction();
                }
            }
            BtnAdd.ChangeCurrentSprite();
            if (!isCreated && IsDragging && !isAds)
            {
                emptyCharacter.gameObject.SetActive(false);
                btnAdd.gameObject.SetActive(false);
                btnReset.gameObject.SetActive(false);
                var item = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == id);
                if (item == null)
                {
                    var character = Instantiate(characterPrefab, characterPosition);
                    character.Id = Id;
                    CurrentCharacter = character;
                    CharacterPaletteController.Ins.GetCharacter(currentCharacter);
                    DataCharacterManager.Instance.LocalData.AddCharacter(new CharacterFeatureSet(), id);
                    isCreated = true;
                    SetRandomItem(character);
                    RectTransform rectTransform = character.GetComponent<RectTransform>();
                    if (rectTransform != null)
                    {
                        rectTransform.DOAnchorPos(Vector2.zero, 0f);
                    }
                }
                if (DataCharacterManager.Instance.LocalData != null)
                {
                    DataCharacterManager.Instance.LocalData.SortListCharacters();
                }
            }
            IsDragging = true;
        }
        public void FirstClick()
        {
            CharacterSlotsView.OnSelected?.Invoke();
            IsSelected = true;
            IsFirstClick = false;

            CheckShow(IsSelected);
            CharacterPaletteController.Ins.ChangeImgForItemCategoryColor();
            if (!isCreated && IsDragging)
            {
                
                emptyCharacter.gameObject.SetActive(false);
                btnAdd.gameObject.SetActive(false);
                btnReset.gameObject.SetActive(false);
                foreach (var floop in floops)
                {
                    floop.gameObject.SetActive(false);
                }//
                var item = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == id);
                if (item == null)
                {
                    var character = Instantiate(characterPrefab, characterPosition);
                    CurrentCharacter = character;
                    CharacterPaletteController.Ins.GetCharacter(currentCharacter);
                    DataCharacterManager.Instance.LocalData.AddCharacter(new CharacterFeatureSet(), id);
                    DataCharacterManager.Instance.LocalData.SortListCharacters();
                    character.Id = Id;
                    isCreated = true;
                    SetRandomItem(character);
                    character.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 0f);
                    DataCharacterManager.Instance.LocalData.ListCharacters[character.Id].IsCreated = true;
                }
            }
        }

        public void SetRandomItem(CharacterController character)
        {
            var age = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == id).Age;
            var characterChoose = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == id);
            var featureEyes = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Eyes);
            var featureEyebrows = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Eyebrows);
            var featureClothes = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Clothes);
            var featureMouth = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Mouth);
            var featureNose = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Nose);
            var featureHair = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Hair);
            var featureHat = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Hat);
            var featureMask = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Mask);
            var featureAccess = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.AccessoryHand);
            var colorEyes = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Eyes);
            var colorEyebrows = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Eyebrows);
            var colorHair = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Hair);
            var colorSkin = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Skin);
            var dataset = new CharacterFeatureLibrary.Data
                (age, characterChoose.SkinColor, characterChoose.HairColor, characterChoose.EyebrowsColor, characterChoose.EyesColor,
                featureEyes, featureEyebrows, featureMouth, featureNose, featureHair, featureClothes, featureHat, featureMask, featureAccess);
            character.CharacterPartHelper.Assign(dataset);
            character.CharacterPartHelper.ChangeAge(age);
            character.CharacterPartHelper.SetEyes(featureEyes);
            character.CharacterPartHelper.SetEyebrows(featureEyebrows);
            character.CharacterPartHelper.SetClothes((CharacterFeatureClothesAsset)featureClothes);
            character.CharacterPartHelper.SetMouth(featureMouth);
            character.CharacterPartHelper.SetHat(featureHat);
            character.CharacterPartHelper.SetMask(featureMask);
            character.CharacterPartHelper.SetNose(featureNose);
            character.CharacterPartHelper.SetAccessoryInHand(featureAccess);
            character.CharacterPartHelper.SetHair((CharacterFeatureHairAsset)featureHair);

            character.CharacterPartHelper.SetColor(colorEyes.Category, colorEyes.Color2);
            character.CharacterPartHelper.SetColor(colorEyebrows.Category, colorEyebrows.Color2);
            character.CharacterPartHelper.SetColor(colorHair.Category, colorHair.Color2);
            character.CharacterPartHelper.SetColor(colorSkin.Category, colorSkin.Color2);

            characterChoose.EyesID = featureEyes.ID;
            characterChoose.EyebrowsID = featureEyebrows.ID;
            characterChoose.ClothingID = featureClothes.ID;
            characterChoose.MouthID = featureMouth.ID;
            characterChoose.NoseID = featureNose.ID;
            characterChoose.HairID = featureHair.ID;
            characterChoose.HatID = featureHat.ID;
            characterChoose.MaskID = featureMask.ID;

            characterChoose.EyesColor = colorEyes.Color2;
            characterChoose.EyebrowsColor = colorEyebrows.Color2;
            characterChoose.HairColor = colorHair.Color2;
            characterChoose.SkinColor = colorSkin.Color2;
        }

        public void HideFloop()
        {
            floop.DOAnchorPosY(-69f, 0.3f).OnComplete(() =>
            {
                if (!isSelected)
                {
                    foreach (var item in floops)
                    {
                        item.gameObject.SetActive(false);
                    }
                    if (!CurrentCharacter)
                    {
                        BtnAdd.gameObject.SetActive(true);
                    }
                    else
                    {
                        BtnAdd.gameObject.SetActive(false);
                    }
                    btnReset.gameObject.SetActive(false);
                }
            });
        }
        public void ShowFloop()
        {
            if (isSelected)
            {
                foreach (var item in floops)
                {
                    item.gameObject.SetActive(true);
                }
                floop.DOAnchorPosY(65f, 0.3f).OnComplete(() =>
                {
                    if (!CurrentCharacter)
                    {
                        BtnAdd.gameObject.SetActive(true);
                        btnReset.gameObject.SetActive(false);
                    }
                    else
                    {
                        BtnAdd.gameObject.SetActive(false);
                        btnReset.gameObject.SetActive(true);
                    }
                });
            }
        }
        public void SetItem(CharacterController character)
        {
            var item = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == id);
            var age = DataCharacterManager.Instance.LocalData.ListCharacters.Find(x => x.CharacterID == id).Age;
            var featureEyes = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Eyes, item.EyesID);
            var featureEyebrows = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Eyebrows, item.EyebrowsID);
            var featureClothes = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Clothes, item.ClothingID);
            var featureMouth = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Mouth, item.MouthID);
            var featureNose = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Nose, item.NoseID);
            var featureHair = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Hair, item.HairID);
            var featureAccessory = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.AccessoryHand, item.AccessoryInHandID);
            var featureHat = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Hat, item.HatID);
            var featureMask = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Mask, item.MaskID);
            var colorEyes = item.EyesColor;
            var colorEyebrows = item.EyebrowsColor;
            var colorHair = item.HairColor;
            var colorSkin = item.SkinColor;

            character.CharacterPartHelper.ChangeAge(age);
            character.CharacterPartHelper.SetEyes(featureEyes);
            character.CharacterPartHelper.SetEyebrows(featureEyebrows);
            character.CharacterPartHelper.SetClothes((CharacterFeatureClothesAsset)featureClothes);
            character.CharacterPartHelper.SetMouth(featureMouth);
            character.CharacterPartHelper.SetNose(featureNose);
            character.CharacterPartHelper.SetAccessoryInHand(featureAccessory);
            character.CharacterPartHelper.SetHat(featureHat);
            character.CharacterPartHelper.SetMask(featureMask);
            character.CharacterPartHelper.SetHair((CharacterFeatureHairAsset)featureHair);

            character.CharacterPartHelper.SetColor(CharacterColorCategory.Eyes, colorEyes);
            character.CharacterPartHelper.SetColor(CharacterColorCategory.Eyebrows, colorEyebrows);
            character.CharacterPartHelper.SetColor(CharacterColorCategory.Hair, colorHair);
            character.CharacterPartHelper.SetColor(CharacterColorCategory.Skin, colorSkin);
        }

        public void CheckCreatedCharacter()
        {
            var findCharacter = DataCharacterManager.Instance.LocalData.HasExistCharacter(id);

            if (findCharacter)
            {
                var character = Instantiate(characterPrefab, characterPosition);
                CurrentCharacter = character;
                character.Id = Id;
                isCreated = true;
                SetItem(character);
                emptyCharacter.gameObject.SetActive(false);
                BtnAdd.gameObject.SetActive(false);
            }
            else
            {
                
            }
        }
        private void SecondClickAction()
        {
            //IsFirstClick = true;//
            CustomSnap.Ins.HidePanel(id);
            floop.DOAnchorPosY(-69f, 0.3f).OnComplete(() =>
            {
                CharacterPaletteController.Ins.ChangeImgForItemCategoryColor();
                foreach (var item in floops)
                {
                    item.gameObject.SetActive(false);
                }
                btnReset.gameObject.SetActive(false);
                btnAdd.gameObject.SetActive(false);
            });
        }

        public void DeActiveFloop()
        {
            btnAdd.gameObject.SetActive(false);
            btnReset.gameObject.SetActive(false);
            foreach (var floop in floops)
            {
                floop.gameObject.SetActive(false);
            }
        }

        public void DeActiveFirstClick()
        {
            if (!isAds)
            {
                isFirstClick = false;
            }
        }
    }
}
