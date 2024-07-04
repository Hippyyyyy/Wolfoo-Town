using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Base
{
    public class SoundBaseManager : MonoBehaviour
    {
        [SerializeField] AudioSource sfx;
        [SerializeField] AudioSource music;
        [SerializeField] AudioClip homeMusic;
        [SerializeField] List<AudioClip> sfxOthers;

        public static SoundBaseManager instance;
        private float startVolumeMusic;

        public bool IsSoundMuted { get; private set; }
        public bool IsMusicMuted { get; private set; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }
        private void Start()
        {
            if (music != null)
            {
                music.clip = homeMusic;
                music.Play();
                startVolumeMusic = music.volume;
            }

            if (BaseDataManager.Instance.IsMuteSound)
            {
                MuteSound(SoundType.SFx);
            }

            if (BaseDataManager.Instance.IsMuteMusic)
            {
                MuteSound(SoundType.Music);
            }
        }
        public void PlayOtherSfx(SfxOtherType type)
        {
            sfx.clip = sfxOthers[(int)type];
            sfx.Play();
        }
        public void MuteSound(SoundType type, bool isMain = false)
        {
            if (music == null) return;

            if (type == SoundType.SFx)
            {
                sfx.volume = 0;
                IsSoundMuted = true;
            }
            else if (type == SoundType.Music)
            {
                music.volume = 0;
                IsSoundMuted = true;
            }
            BaseDataManager.Instance.SetMute(type);
        }
        public void EnableSound(SoundType type, bool isMain = false)
        {
            if (music == null) return;

            if (type == SoundType.SFx)
            {
                //    if (!isMain && IsSoundMuted) return;
                sfx.volume = 1;
                IsSoundMuted = false;
            }
            else if (type == SoundType.Music)
            {
                //    if (!isMain && IsMusicMuted) return;
                music.volume = startVolumeMusic;
                IsSoundMuted = false;
            }
            BaseDataManager.Instance.SetUnMute(type);
        }
    }
    public enum SoundType
    {
        Music,
        SFx,
    }
    public enum SfxOtherType
    {
        Click,
    }
}