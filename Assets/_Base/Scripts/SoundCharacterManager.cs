using _Base;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SoundCharacterManager : SoundBase<SoundCharacterManager>
    {
        [SerializeField] protected SfxData[] myMaleData;
        [SerializeField] protected SfxData[] myFemaleData;
        private RandomNoRepeat<SfxWolfooType> rdIdxNrp;

        public enum SfxWolfooType
        {
            Hello,
            Hooray,
            Hoow,
            Sad,
            Wow,
            Thankyou,
            Laugh,
            Goodbye,
            Great,
            Disagree,
            Perfect,
            Delicious,
            Cheering,
            Eating1,
            Eating2,
        }

        [System.Serializable]
        public struct SfxData
        {
            public AudioClip clip;
            public SfxWolfooType sfxType;
        }
        protected override void OnInit()
        {
            base.OnInit();
            var lst = new List<SfxWolfooType>() { SfxWolfooType.Hooray, SfxWolfooType.Wow, SfxWolfooType.Hoow, SfxWolfooType.Perfect, SfxWolfooType.Great };
            rdIdxNrp = new RandomNoRepeat<SfxWolfooType>(lst);
        }

        public void Play(AudioClip clip)
        {
            if (IsSoundMuted) return;
            if (clip == null || sfx == null) return;

            sfx.clip = clip;
            sfx.Play();
        }
        public void Play(SfxWolfooType sfxType, bool isMale = true)
        {
            if (IsSoundMuted) return;
            if (sfx == null) return;

            foreach (var item in (isMale ? myMaleData : myFemaleData))
            {
                if (item.sfxType == sfxType)
                {
                    sfx.clip = item.clip;
                    sfx.Play();
                    break;
                }
            }
        }
        public void PlayWolfooInteresting(bool isMale = true)
        {
            if (IsSoundMuted) return;
            if (sfx == null) return;

            var type = rdIdxNrp.Random();
            foreach (var item in (isMale ? myMaleData : myFemaleData))
            {
                if (item.sfxType == type)
                {
                    sfx.clip = item.clip;
                    sfx.Play();
                    break;
                }
            }
        }
    }
}