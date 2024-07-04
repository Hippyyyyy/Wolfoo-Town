using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class BathTubWorld : BackItemWorld
    {
        [SerializeField] ParticleSystem[] waterFxs;
        [SerializeField] AudioClip waterAudio;

        private bool isPlaying;
        private ParticleSystemRenderer[] particleRenderers;
        private AudioSource waterAus;

        public override void Setup()
        {
            IsClick = true;
            base.Setup();
            particleRenderers = waterFxs[0].GetComponentsInChildren<ParticleSystemRenderer>();
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
                foreach (var waterFx in waterFxs)
                {
                    waterFx.Play();
                }
                waterAus.Play();
            }
            else
            {
                foreach (var waterFx in waterFxs)
                {
                    waterFx.Stop();
                }
                waterAus.Stop();
            }
        }
    }
}
