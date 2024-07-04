using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "IAP Manger", menuName = "IAP Packages/IAP Manager", order = 1)]
    public class IAPDataManager : ScriptableObject
    {
        public IAPPackData romanticBeach;
        public IAPPackData rainbowApartment;
        public IAPPackData familyHouse;
    }
}
