using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "CharacterFeatureLibrary", menuName = RESOURCE_PATH)]
    public class CharacterFeatureLibrary : ScriptableObject
    {
        public List<CharacterFeatureCategory> CharacterFeatureCategories;

        public Data data;
        [System.Serializable]
        public class Data
        {
            public Color watchSkin;
            public Color watchHair;
            public Color watchEyebrows;
            public Color watchEyes;
            
            public AgeSetting assetAge;
            public CharacterFeatureAsset assetEyes;
            public CharacterFeatureAsset assetEyebrows;
            public CharacterFeatureAsset assetMouth;
            public CharacterFeatureAsset assetNose;
            public CharacterFeatureAsset assetHair;
            public CharacterFeatureAsset assetClothes;
            public CharacterFeatureAsset assetHat;
            public CharacterFeatureAsset assetMask;
            public CharacterFeatureAsset assetAccess;

            public Data(AgeSetting age, Color watchSkin, Color watchHair, Color watchEyebrows, Color watchEyes, CharacterFeatureAsset assetEyes, CharacterFeatureAsset assetEyebrows, CharacterFeatureAsset assetMouth, CharacterFeatureAsset assetNose, CharacterFeatureAsset assetHair, CharacterFeatureAsset assetClothes, CharacterFeatureAsset assetHat, CharacterFeatureAsset assetMask, CharacterFeatureAsset assetAccess)
            {
                this.assetAge = age;
                this.watchSkin = watchSkin;
                this.watchHair = watchHair;
                this.watchEyebrows = watchEyebrows;
                this.watchEyes = watchEyes;
                this.assetEyes = assetEyes;
                this.assetEyebrows = assetEyebrows;
                this.assetMouth = assetMouth;
                this.assetNose = assetNose;
                this.assetHair = assetHair;
                this.assetClothes = assetClothes;
                this.assetHat = assetHat;
                this.assetMask = assetMask;
                this.assetAccess = assetAccess;
            }
        }

        const string RESOURCE_PATH = "Data/Character/CharacterFeatureLibrary";

        public CharacterColorSwatch GetColorSwatch(CharacterColorCategory characterColor, int id)
        {
            var category = CharacterFeatureCategories.Find(x => x.ColorCategory == characterColor);
            var colorSwatch = category.characterFeatureFilter.filteredColorSwatch.Find(x => x.ID == id);
            return colorSwatch;
        }
        public CharacterFeatureAsset GetCharacterFeature(CharacterFeatureCategoryEnum characterFeature, int id)
        {
            var category = CharacterFeatureCategories.Find(x => x.FeatureCategories[0] == characterFeature);
            var characterFeatureIndex = category.characterFeatureFilter.filteredFeatures.Find(x => x.ID == id);
            return characterFeatureIndex;
        }
        public CharacterFeatureAsset RandomFilterCharacterFeatures(CharacterFeatureCategoryEnum characterCategory)
        {
            var category = CharacterFeatureCategories.Find(x => x.FeatureCategories[0] == characterCategory);
            var characterFeatures = category.characterFeatureFilter.filteredFeatures[Random.Range(0, 3)];
            return characterFeatures;
        }
        //
        public CharacterColorSwatch RandomFilterColor(CharacterColorCategory characterColorCategory)
        {
            var category = CharacterFeatureCategories.Find(x => x.ColorCategory == characterColorCategory);
            var characterColor = category.characterFeatureFilter.filteredColorSwatch[Random.Range(0, 5)];
            return characterColor;
        }
    }
}
