using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
	[CreateAssetMenu]
    public class CharacterFeatureCategory : ScriptableObject
    {
		public List<CharacterFeatureCategoryEnum> FeatureCategories;

		public CharacterHairCategoryEnum HairCategory;

		public List<CharacterFeatureAsset> featureFilter;

		public CharacterColorCategory ColorCategory;

		public CharacterFeatureFilter characterFeatureFilter;

	}
}
