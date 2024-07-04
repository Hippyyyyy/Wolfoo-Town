using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SoundMachine : BackItemWorld
    {
        [SerializeField] ParticleSystem soundFx;
        [SerializeField] AudioClip soundClip;
        private bool isPlaying;
        private AudioSource mySoundAus;

        public override void Setup()
        {
            IsClick = true;
            IsDragable = true;
            IsStandingOnTable = true;
            base.Setup();
        }
        protected override void OnClick()
        {
            base.OnClick();
            isPlaying = !isPlaying;
            PlayCurrentState();
            mySoundAus = SoundCampingParkManager.Instance.CreateNewAus(new SoundBase<SoundCampingParkManager>.Item(Id, "Dai catsset " + Id, soundClip, true));
        }
        public void PlayCurrentState()
        {
            if (soundFx != null)
            {
                if (isPlaying)
                {
                    soundFx.Play();
                    if(mySoundAus != null) mySoundAus.Play();
                }
                else
                {
                    soundFx.Stop();
                    if (mySoundAus != null) mySoundAus.Stop();
                }
            }
        }
    }
}
