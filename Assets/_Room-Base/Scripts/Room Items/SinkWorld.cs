using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SinkWorld : BackItemWorld
    {
        [SerializeField] ParticleSystem waterFx;
        [SerializeField] AudioClip waterAudio;
        private bool isPlaying;
        private AudioSource waterAus;

        public override void Setup()
        {
            IsClick = true;
            base.Setup();
            waterAus = SoundBeachVillaManager.Instance.CreateNewAus(new SoundBase<SoundBeachVillaManager>.Item(Id, "Water Bath Tub + " + Id, waterAudio, true));
        }

        protected override void OnClick()
        {
            base.OnClick();
            isPlaying = !isPlaying;

            SetStateWater();
        }

        private void SetStateWater()
        {
            if (isPlaying)
            {
                waterFx.Play();
                waterAus.Play();
            }
            else
            {
                waterFx.Stop();
                waterAus.Stop();
            }
        }
    }
}
