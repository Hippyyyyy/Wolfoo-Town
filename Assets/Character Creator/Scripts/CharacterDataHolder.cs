using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterDataHolder : SingletonBind<CharacterDataHolder>
    {
        int count;
        private void Start()
        {
            DontDestroyOnLoad(this);
        }
        public CharacterFeatureLibrary AssetLibary;


        public int AssetLength { get { return DataCharacterManager.Instance.LocalData.ListCharacters.Count; } }
        public CharacterController WorldCharacter;
        public CharacterController UICharacter;
        public CharacterController UIInRoomCharacter;

        public CharacterFeatureLibrary.Data GetNextDataSet()
        {
            var id = count++;
            var characterFeatureLibrary = AssetLibary;
            DataCharacterManager.Instance.LocalData.ListCharacters[id].Age = (AgeSetting)UnityEngine.Random.Range(1, 2);
            var age = DataCharacterManager.Instance.LocalData.ListCharacters[id].Age;
            var characterColor = DataCharacterManager.Instance.LocalData.ListCharacters[id];

            var featureEyes = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Eyes);
            var featureEyebrows = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Eyebrows);
            var featureClothes = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Clothes);
            var featureMouth = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Mouth);
            var featureNose = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Nose);
            var featureHair = characterFeatureLibrary.RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum.Hair);
            var colorEyes = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Eyes);
            var colorEyebrows = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Eyebrows);
            var colorHair = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Hair);
            var colorSkin = characterFeatureLibrary.RandomFilterColor(CharacterColorCategory.Skin);

            var dataset = new CharacterFeatureLibrary.Data
                (age, characterColor.SkinColor, characterColor.HairColor, characterColor.EyebrowsColor, characterColor.EyesColor,
                featureEyes, featureEyebrows, featureMouth, featureNose, featureHair, featureClothes, null, null, null);

            return dataset;
            //  character.CharacterPartHelper.Assign(dataset);
        }
        public CharacterFeatureLibrary.Data GetDataSet(int id)
        {
            var characterFeatureLibrary = AssetLibary;
            //DataCharacterManager.Instance.LocalData.ListCharacters[id].Age = (AgeSetting)UnityEngine.Random.Range(1, 2);
            var age = DataCharacterManager.Instance.LocalData.ListCharacters[id].Age;
            var characterColor = DataCharacterManager.Instance.LocalData.ListCharacters[id];

            var featureEyes = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Eyes, characterColor.EyesID);
            var featureEyebrows = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Eyebrows, characterColor.EyebrowsID);
            var featureClothes = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Clothes, characterColor.ClothingID);
            var featureMouth = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Mouth, characterColor.MouthID);
            var featureNose = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Nose, characterColor.NoseID);
            var featureHair = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Hair, characterColor.HairID);
            var featureHat = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Hat, characterColor.HatID);
            var featureMask = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Mask, characterColor.MaskID);
            var featureAccess = characterFeatureLibrary.GetCharacterFeature(CharacterFeatureCategoryEnum.Mask, characterColor.AccessoryInHandID);
            //
            var dataset = new CharacterFeatureLibrary.Data
                (age, characterColor.SkinColor, characterColor.HairColor, characterColor.EyebrowsColor, characterColor.EyesColor,
                featureEyes, featureEyebrows, featureMouth, featureNose, featureHair, featureClothes, featureHat, featureMask, featureAccess);

            return dataset;
            //  character.CharacterPartHelper.Assign(dataset);
        }
        public void Release()
        {
            count = 0;
        }
    }
}
