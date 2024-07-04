using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
	[CreateAssetMenu(fileName = "CharacterFeatureClothesAsset", menuName = "Scriptable Objects/" + "CharacterFeatureClothesAsset")]
	public class CharacterFeatureClothesAsset : CharacterFeatureAsset
    {
		//public TocaPrefabReference PrefabReference;

		public CharacterClothesCategoryEnum ClothesCategory;

		public Sprite LeftSleeve;

		public Sprite LeftPant;

		public Sprite RightSleeve;

		public Sprite RightPant;

		public Sprite RightShoes;

		public Sprite LeftShoes;

		public Sprite Hanging;
	}
}
