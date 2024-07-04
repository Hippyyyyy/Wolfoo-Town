using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public interface ITutorial
    {
        public void Register(TutorialStep step);
        public void Remove(TutorialStep step);
        public void PlayNextStep();
    }
}
