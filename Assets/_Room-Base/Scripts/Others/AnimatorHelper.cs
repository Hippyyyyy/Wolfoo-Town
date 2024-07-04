using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class AnimatorHelper : MonoBehaviour
    {
        public System.Action OnPlayComplete;
        public System.Action OnCloseComplete;

        public void OnPlayingComplete()
        {
            OnPlayComplete?.Invoke();
        }
        public void OnClosingComplete()
        {
            OnCloseComplete?.Invoke();
        }
    }
}
