using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ChangeFeatureCharacter : MonoBehaviour
    {
        [SerializeField] CharacterFeatureLibrary characterFeatureLibrary;
        [SerializeField] CharacterController characterController;

        private void Start()
        {
            SpawnCharacter();
        }

        void SpawnCharacter()
        {
            var count = DataCharacterManager.Instance.LocalData.ListCharacters.Count;
            for (int i = 0; i < count; i++)
            {
                var character = Instantiate(characterController, transform);
                character.Id = i;
                SetItem(character, DataCharacterManager.Instance.LocalData.ListCharacters);
            }
            var count2 = DataCharacterManager.Instance.LocalData.ListCharacterCreated.Count;
            for (int i = 0; i < count2; i++)
            {
                var character = Instantiate(characterController, transform);
                character.Id = i;
                SetItem(character, DataCharacterManager.Instance.LocalData.ListCharacterCreated);
            }
        }
        public void SetItem(CharacterController character, List<CharacterFeatureSet> listChar)
        {
            var item = listChar[character.Id];
            var age = listChar[character.Id].Age;
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
    }
}
