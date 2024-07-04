using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class HospitalAnimAction: MonoBehaviour
    {
        public Action OnCompleted;
        public void OnActionCompleted()
        {
            OnCompleted?.Invoke();
        }
    }
}