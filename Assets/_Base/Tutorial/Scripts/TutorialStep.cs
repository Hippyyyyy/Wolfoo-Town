using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public abstract class TutorialStep : MonoBehaviour
    {
        public bool IsPlaying { get; protected set; }
        public System.Action OnTutorialComplete;
        public System.Action OnClickTutorial;
        public abstract void Play();
        public abstract void Stop();
    }
}
