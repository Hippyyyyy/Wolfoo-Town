using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Tutorial : ITutorial
    {
        List<TutorialStep> mySteps = new List<TutorialStep>();
        private int totalStep { get => mySteps.Count; }
        private int countStep;

        public System.Action OnCompleteAllStep;

        public void PlayNextStep()
        {
            if (countStep >= totalStep)
            {
                OnCompleteAllStep?.Invoke();
                return;
            }
            countStep++;
        }

        public void Register(TutorialStep step)
        {
            if (!mySteps.Contains(step))
            {
                mySteps.Add(step);
            }
        }

        public void Remove(TutorialStep step)
        {
            if (mySteps.Contains(step))
            {
                mySteps.Remove(step);
            }
        }
    }
}
