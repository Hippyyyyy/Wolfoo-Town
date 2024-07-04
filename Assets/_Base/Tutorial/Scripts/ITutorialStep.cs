using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public interface ITutorialStep
    {
        public void Play();
        public void Stop();
        public void Setup(Transform highlightTarget);
    }
}
