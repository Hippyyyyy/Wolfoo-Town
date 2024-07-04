using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [RequireComponent(typeof(ParticleSystem))]
    public class FxHolder : MonoBehaviour
    {
        public ParticleSystem Particle;

        public Action OnTrigger;

        private void Start()
        {
            if (Particle == null)
                Particle = GetComponent<ParticleSystem>();
        }
    }
}
