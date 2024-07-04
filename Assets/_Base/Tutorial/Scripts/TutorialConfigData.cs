using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "Tutorial Data", menuName = "Scriptale Objects/" + "Tutorial Data")]
    public class TutorialConfigData : ScriptableObject
    {
        public TutorialWithBG tutorialStep;

        public TutorialStep GetData<T>() where T: TutorialStep
        {
            if(tutorialStep.GetType() == typeof(T))
            {
                return tutorialStep;
            }

            return null;
        }
    }
}
