using _Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SoundTown<T> : SoundBase<T> where T : Component
    {
        [SerializeField] protected AudioSource music;
        [SerializeField] protected SfxData[] soundData;

        [System.Serializable]
        public struct SfxData
        {
            public AudioClip clip;
            public SFXType sfxType;
        }

        public enum SFXType
        {
            Printing,
            MagicLighting,
            ApolloHit,
            MerryGoRound,
            Garbage,
            CarImpact,
            SlideDoor,
            SprayPerfume,
            Piano,
            CurtainSlide,
            Capture,
            Select,
            Scratch,
            Suck,
            Spawn,
            Guitar1,
            Guitar2,
            Drum,
            Drum2,
        }
        protected override void OnInit()
        {
            base.OnInit();

            startMusicVolume = music.volume;
            if (BaseDataManager.Instance.IsMuteMusic) { MuteSound(SoundType.Music); }
            else { EnableSound(SoundType.Music); }
        }

        protected override void MuteSound(SoundType type)
        {
            base.MuteSound(type);

            if (type != SoundType.Music) return;
            if (music == null) return;
            //   music.volume = 0;
            switch (type)
            {
                case SoundType.Music:
                    music.volume = 0;
                    break;
            }
        }
        protected override void EnableSound(SoundType type)
        {
            base.EnableSound(type);

            if (type != SoundType.Music) return;
            if (music == null) return;
            //  music.volume = startVolumeMusic;
            switch (type)
            {
                case SoundType.Music:
                    music.volume = startMusicVolume;
                    music.Play();
                    break;
            }
        }

        public void PlayOtherSfx(AudioClip clip)
        {
            if (IsSoundMuted) return;
            if (clip == null || sfx == null) return;

            sfx.clip = clip;
            sfx.Play();
        }
        public void PlayOtherSfx(SFXType sfxType)
        {
            if (IsSoundMuted) return;
            if (sfx == null) return;

            foreach (var item in soundData)
            {
                if (item.sfxType == sfxType)
                {
                    sfx.clip = item.clip;
                    sfx.Play();
                    break;
                }
            }
        }
    }
}