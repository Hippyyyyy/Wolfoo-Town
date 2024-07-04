using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class IAPAssetPack : ScriptableObject
    {
        [NaughtyAttributes.ReadOnly]
        public string ID;

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (ID == "")
            {
                ID = System.Guid.NewGuid().ToString();
            }
        }
#endif
    }
}
