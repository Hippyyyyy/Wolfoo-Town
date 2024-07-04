using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [Serializable]
    public class CharacterFeatureFilter 
    {
		public List<CharacterFeatureAsset> filteredFeatures;

		public List<CharacterFeatureAsset> filteredBabyFeatures;

		public List<CharacterFeatureCategoryEnum> categories;

		public List<CharacterColorSwatch> filteredColorSwatch;

		bool wasCharacterCreatorUnlocked;

    }
}
