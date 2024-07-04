using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

namespace _WolfooShoppingMall
{
	[CreateAssetMenu(fileName = "CharacterFeatureAsset", menuName = "Scriptable Objects/" + "CharacterFeatureAsset")]
	public class CharacterFeatureAsset : ScriptableObject
    {
        [Serializable]
		public struct FeatureMetadata
		{
			public bool IsAvailableInCharacterCreator;

			public bool IsAvailableForBabies;

			public bool IsAvailableForAdults;

			public bool IsFree;

			public bool IsLockForPack;
			public IAPPackData iAPPack;

			public long UnlockedDateTimeTick;
		}

		public int ID;

		public Sprite Icon;

		public CharacterFeatureCategoryEnum Category;

		public Sprite Front;

		public Sprite Back;

		[ReadOnly]
		public bool NewlyCreated;

		public FeatureMetadata Metadata;

		public string GetSubcategoryString()
		{
			return null;
		}

		public int GetSubcategoryInt()
		{
			return 0;
		}
	}
}
