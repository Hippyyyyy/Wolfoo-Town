using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public abstract class CustomFx : MonoBehaviour
    {
        [SerializeField] protected bool isLoop;
        [SerializeField] protected bool isAutoPlay;
        [SerializeField] protected ParticleSystem myFx;

        public abstract void Play();
        public abstract void Stop();
        public abstract void Pause();
        public abstract void OnPlay();
        public abstract void OnStop();
    }
}
