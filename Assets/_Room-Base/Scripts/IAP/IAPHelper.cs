using _Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class IAPHelper
    {
        public static IAPPackData[] ListIapID
        {
            get
            {
                var iapData = BaseDataManager.Instance.IAPDataManager;
                return new IAPPackData[]
                {
                    iapData.romanticBeach,
                    iapData.rainbowApartment,
                    iapData.familyHouse,
                };
            }
        }
    }
}
