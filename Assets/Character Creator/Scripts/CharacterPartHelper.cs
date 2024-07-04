using _WolfooShoppingMall;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPartHelper : MonoBehaviour
{
    [SerializeField] bool isCreator;
    [SerializeField] Transform Blush;

    [SerializeField] Transform Eyebrows;

    [SerializeField] Transform Eyes;

    [SerializeField] Transform Freckles;

    [SerializeField] Transform Mouth;

    [SerializeField] Transform Nose;

    [SerializeField] Transform Wrinkles;

    [SerializeField] Transform Scar;

    [SerializeField] Transform LegLeft;

    [SerializeField] Transform LegRight;

    [SerializeField] Transform ArmLeft;

    [SerializeField] Transform ArmRight;

    [SerializeField] Transform Head;

    [SerializeField] Transform HeadMain;

    [SerializeField] Transform HairFront;

    [SerializeField] Transform HairBack;

    [SerializeField] Transform Torso;

    [SerializeField] Transform TorsoBack;

    [SerializeField] Transform ScaleTrans;

    [SerializeField] Transform EarsRight;

    [SerializeField] Transform EarsLeft;

    [SerializeField] Transform PantLeft;

    [SerializeField] Transform PantRight;

    [SerializeField] Transform SleeveLeft;

    [SerializeField] Transform SleeveRight;

    [SerializeField] Transform ShoesLeft;

    [SerializeField] Transform ShoesRight;

    [SerializeField] Transform ShirtFront;

    [SerializeField] Transform ShirtBack;

    [SerializeField] Transform Mask;

    [SerializeField] Transform Hat;

    [SerializeField] Transform AccessoryHand;


    private void Start()
    {

    }

    public void Assign(CharacterFeatureLibrary.Data data)
    {
        ChangeAge(data.assetAge);
        SetEyebrows(data.assetEyebrows);
        SetEyes(data.assetEyes);
        SetMouth(data.assetMouth);
        SetNose(data.assetNose);
        SetHair((CharacterFeatureHairAsset)data.assetHair);
        SetClothes((CharacterFeatureClothesAsset)data.assetClothes);
        if (data.assetHat) SetHat(data.assetHat);
        if (data.assetHat) SetMask(data.assetMask);
        SetColorEyebrows(data.watchEyebrows);
        SetColorEyes(data.watchEyes);
        SetColorSkin(data.watchSkin);
        SetColorHair(data.watchHair);
    }

    public void SetBlush(CharacterFeatureAsset featureAsset)
    {

    }

    public void SetEyebrows(CharacterFeatureAsset featureAsset)
    {
        ChangeSprite(Eyebrows, featureAsset.Front);
    }

    public void SetEyes(CharacterFeatureAsset featureAsset)
    {
        ChangeSprite(Eyes, featureAsset.Front);
    }
    public void SetMouth(CharacterFeatureAsset featureAsset)
    {
        ChangeSprite(Mouth, featureAsset.Front);
    }

    public void SetNose(CharacterFeatureAsset featureAsset)
    {
        ChangeSprite(Nose, featureAsset.Front);
    }

    public void SetHair(CharacterFeatureHairAsset featureAsset)
    {
        Vector2 customPivotFront = new Vector2(
               featureAsset.Front.pivot.x / featureAsset.Front.rect.width,
               featureAsset.Front.pivot.y / featureAsset.Front.rect.height
           );
        HairBack.gameObject.SetActive(false);
        ChangeSprite(HairFront, featureAsset.Front);
        if (featureAsset.Back)
        {
            Vector2 customPivotBack = new Vector2(
                featureAsset.Back.pivot.x / featureAsset.Back.rect.width,
                featureAsset.Back.pivot.y / featureAsset.Back.rect.height
              );
            ChangeSprite(HairBack, featureAsset.Back);
            if (HairBack.GetComponent<RectTransform>())
            {
                HairBack.GetComponent<RectTransform>().pivot = customPivotBack;
            }
        }
        
        if (HairFront.GetComponent<RectTransform>())
        {
            HairFront.GetComponent<RectTransform>().pivot = customPivotFront;
        }

        
    }
    public void SetBeard(CharacterFeatureHairAsset featureAsset)
    {

    }
    public void SetClothes(CharacterFeatureClothesAsset featureAsset)
    {
        ShirtBack.gameObject.SetActive(false);
        SleeveLeft.gameObject.SetActive(false);
        SleeveRight.gameObject.SetActive(false);
        PantLeft.gameObject.SetActive(false);
        PantRight.gameObject.SetActive(false);

        if (featureAsset.Front) ChangeSprite(ShirtFront, featureAsset.Front);
        if (featureAsset.Back) ChangeSprite(ShirtBack, featureAsset.Back);

        if (featureAsset.LeftSleeve) ChangeSprite(SleeveLeft, featureAsset.LeftSleeve); 
        if (featureAsset.RightSleeve) ChangeSprite(SleeveRight, featureAsset.RightSleeve);

        if (featureAsset.LeftPant) ChangeSprite(PantLeft, featureAsset.LeftPant);
        if (featureAsset.RightPant) ChangeSprite(PantRight, featureAsset.RightPant);

        if (featureAsset.LeftShoes) ChangeSprite(ShoesLeft, featureAsset.LeftShoes);
        if (featureAsset.RightShoes) ChangeSprite(ShoesRight, featureAsset.RightShoes);

        if (featureAsset.LeftPant == null || featureAsset.RightPant == null)
        {
            PantLeft.gameObject.SetActive(false);
            PantRight.gameObject.SetActive(false);
        }
        else
        {
            PantLeft.gameObject.SetActive(true);
            PantRight.gameObject.SetActive(true);
        }
    }//
    public void SetMask(CharacterFeatureAsset featureAsset)
    {
        if (featureAsset.Front)
        {
            ChangeSprite(Mask, featureAsset.Front);
        }
        else
        {
            Mask.gameObject.SetActive(false);
        }
    }
    public void SetHat(CharacterFeatureAsset featureAsset)
    {
        if (featureAsset.Front)
        {
            ChangeSprite(Hat, featureAsset.Front);
        }
        else
        {
            Hat.gameObject.SetActive(false);
        }
    }
    public void SetAccessoryInHand(CharacterFeatureAsset featureAsset)
    {
        if (featureAsset.Front)
        {
            ChangeSprite(AccessoryHand, featureAsset.Front);
            Vector2 customPivot = new Vector2(
            featureAsset.Front.pivot.x / featureAsset.Front.rect.width,
            featureAsset.Front.pivot.y / featureAsset.Front.rect.height
            );
            if (AccessoryHand.GetComponent<RectTransform>())
            {
                AccessoryHand.GetComponent<RectTransform>().pivot = customPivot;
            }
        }
        else
        {
            AccessoryHand.gameObject.SetActive(false);
        }
    }


    public void SetFeatureItem(CharacterFeatureCategoryEnum characterFeature, CharacterFeatureAsset characterFeatureAsset = null)
    {
        switch (characterFeature)
        {
            case CharacterFeatureCategoryEnum.Hair:
                SetHair((CharacterFeatureHairAsset)characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Mouth:
                SetMouth(characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Nose:
                SetNose(characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Eyes:
                SetEyes(characterFeatureAsset);
                Debug.Log(characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Mask:
                SetMask(characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Eyebrows:
                SetEyebrows(characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Clothes:
                SetClothes((CharacterFeatureClothesAsset)characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Hat:
                SetHat(characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.AccessoryHand:
                SetAccessoryInHand(characterFeatureAsset);
                break;
            case CharacterFeatureCategoryEnum.Skin:
                ChangeAge((AgeSetting)characterFeatureAsset.ID);
                break;
        }
    }
    public void SetAllParts(CharacterFeatureAsset[] featureAssets)
    {
        if (featureAssets.Length != Enum.GetValues(typeof(CharacterFeatureCategoryEnum)).Length)
        {
            return;
        }

        for (int i = 0; i < featureAssets.Length; i++)
        {
            SetFeatureItem((CharacterFeatureCategoryEnum)i, featureAssets[i]);
        }
    }
    #region Color
    public void SetColorEyes(Color color)
    {
        ChangeColor(Eyes, color);
    }
    public void SetColorEyebrows(Color color)
    {
        ChangeColor(Eyebrows, color);
    }
    public void SetColorHair(Color color)
    {
        ChangeColor(HairFront, color);
        ChangeColor(HairBack, color);
    }

    public void SetColorSkin(Color color)
    {
        ChangeColor(ArmLeft, color);
        ChangeColor(ArmRight, color);
        ChangeColor(LegLeft, color);
        ChangeColor(LegRight, color);
        ChangeColor(Torso, color);
        ChangeColor(Head, color);
        ChangeColor(EarsLeft, color);
        ChangeColor(EarsRight, color);
    }

    public void SetColor(CharacterColorCategory characterColor, Color color)
    {
        switch (characterColor)
        {
            case CharacterColorCategory.Eyebrows:
                SetColorEyebrows(color);
                break;
            case CharacterColorCategory.Eyes:
                SetColorEyes(color);
                break;
            case CharacterColorCategory.Hair:
                SetColorHair(color);
                break;
            case CharacterColorCategory.Skin:
                SetColorSkin(color);
                break;
        }
    }
    #endregion

    #region Age

    public void ChangeAge(AgeSetting age)
    {
        var isUI = transform.GetComponent<Image>() != null;
        if(isCreator)
        {
            if (age == AgeSetting.Baby)
            {
                ChangeScaleCharacter(transform, Vector3.one * 40);
            }
            else if (age == AgeSetting.Kid)
            {
                ChangeScaleCharacter(transform, Vector3.one * 65);
            }
        }
        else
        {
            switch (age)
            {
                case AgeSetting.Baby:
                    if (isUI) transform.localScale -= Vector3.one * 0.1f;
                    else transform.localScale -= Vector3.one * 10;
                    break;
                case AgeSetting.Kid:
                    break;
            }
        }
    }
    void ChangeScaleCharacter(Transform trans, Vector3 scale)
    {
        if (trans.GetComponent<Image>())
        {
            trans.DOScale(scale / 100, 0f);
        }
        else if (trans.GetComponent<SpriteRenderer>())
        {
            trans.DOScale(scale, 0f);
        }
    }

    #endregion


    #region Change
    void ChangeSprite(Transform trans, Sprite spr)
    {
        trans.gameObject.SetActive(true);
        if (spr)
        {
            if (trans.GetComponent<Image>())
            {
                trans.GetComponent<Image>().sprite = spr;
                trans.GetComponent<Image>().SetNativeSize();
            }
            else if (trans.GetComponent<SpriteRenderer>())
            {
                trans.GetComponent<SpriteRenderer>().sprite = spr;
            }
        }
    }
    void ChangeColor(Transform trans, Color colorChange)
    {
        if (trans.GetComponent<Image>() && trans.GetComponent<Image>().sprite)
        {
            trans.GetComponent<Image>().color = colorChange;
        }
        else if (trans.GetComponent<SpriteRenderer>() && trans.GetComponent<SpriteRenderer>().sprite)
        {
            trans.GetComponent<SpriteRenderer>().color = colorChange;
        }
    }

    void Fade(Image img, float value, float time)
    {
        img.DOFade(value, time);
    }

    void ChangeScale(Transform trans, Vector3 scale)
    {
        trans.localScale = scale;
    }

    void ChangePosition(Transform trans, float pos)
    {
        if (trans.GetComponent<Image>())
        {
            trans.GetComponent<RectTransform>().DOAnchorPosY(pos * 100, 0f);
            trans.GetComponent<RectTransform>().DOAnchorPosX(0f, 0f);
        }
        else
        {
            trans.localPosition = new Vector3(0, pos, 0);
        }
    }
    //

    #endregion



}
