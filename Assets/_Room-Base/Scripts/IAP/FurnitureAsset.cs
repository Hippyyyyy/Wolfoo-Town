using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "IAP Furniture Asset", menuName = "IAP Packages/IAP Furniture Asset", order = 1)]
    public class FurnitureAsset : IAPAssetPack
    {
        public Sprite[] furnitureSpts;
        public Sprite[] decorSpts;
        public Sprite[] floorSpts;
        public Sprite[] wallSpts;
    }
}
