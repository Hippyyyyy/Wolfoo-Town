using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "CharacterFeatureHairAsset", menuName = "Scriptable Objects/" + "CharacterFeatureHairAsset")]
    public class CharacterFeatureHairAsset : CharacterFeatureAsset
    {
        public CharacterHairCategoryEnum HairCategory;

        public bool CanDye;

        public bool HidesEars;

    }
}
