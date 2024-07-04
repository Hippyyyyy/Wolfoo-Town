using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public abstract class ItemWorldBase : MonoBehaviour
    {
        public abstract void Setup();
        protected abstract void OnKill();
        protected abstract void RegisterEvent();
        protected abstract void RemoveEvent();

        void Start()
        {
            Setup();
        }
        void OnDestroy()
        {
            OnKill();
        }

    }
}
