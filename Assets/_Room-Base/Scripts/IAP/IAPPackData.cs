using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "IAP Pack", menuName = "IAP Packages/IAP Data", order = 1)]
    public class IAPPackData : ScriptableObject
    {
        public enum Categories
        {
            House,
            Furniture,
            Clothes,
            Summarize
        }

        public string storeID;
        public Categories Category;
        public string Name;
        public int Price;
        public bool IsUnlock;
    }
}
