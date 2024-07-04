using _Base;
using DG.Tweening;
using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource sfx;
        [SerializeField] AudioSource speaker;
        [SerializeField] AudioSource music;
        [SerializeField] AudioSource loopingAus;
        [SerializeField] AudioClip homeMusic;
        [SerializeField] List<AudioClip> ingameMusics;
        [SerializeField] List<AudioClip> sfxOthers;

        /// <summary>
        /// S? index trong Các list trên T??ng ?ng v?i index Sound Type
        /// </summary>
        public static SoundManager instance;

        List<int> exceptionIdx = new List<int>();
        private int rd;
        private Tween delayTwen;
        private int curIdx;
        private Tween delayTwen2;
        private RandomNoRepeat<int> rdIdxNrp;
        private IngameType curIngameType;
        private float startVolumeMusic;

        public AudioSource Sfx { get => sfx; }
        public bool IsMuted { get; private set; }
        public AudioSource Music { get => music; }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        }

        private void Start()
        {
            if(music != null)
            {
                music.clip = homeMusic;
                music.Play();

                startVolumeMusic = music.volume;
            }

            var lst = new List<int>() { 1, 2, 4, 8, 10 };
            rdIdxNrp = new RandomNoRepeat<int>(lst);

            if (BaseDataManager.Instance.IsMuteSound)
            {
                MuteSound(SoundType.SFx, true);
            }
            else
            {
                EnableSound(SoundType.SFx, true);
            }

            if (BaseDataManager.Instance.IsMuteMusic)
            {
                MuteSound(SoundType.Music, true);
            }
            else
            {
                EnableSound(SoundType.Music, true);

            }
        }
        private void OnDestroy()
        {
        }

        #region BASE

        public void PlayIngame(IngameType ingameType)
        {
            if (music == null) return;
            if (curIngameType == ingameType) return;
            curIngameType = ingameType;

            if (music.isPlaying) music.Stop();

            music.clip = ingameMusics[(int)ingameType];
            music.Play();
        }
        public void PlayOtherSfx(SfxOtherType type)
        {
            if (sfx == null) return;
            int idx = (int)type;
            for (int i = 0; i < sfxOthers.Count; i++)
            {
                if (i == curIdx && sfx.isPlaying) return;
            }

            curIdx = idx;
            sfx.clip = sfxOthers[idx];
            sfx.Play();

            if (delayTwen2 != null) delayTwen2?.Kill();
            delayTwen2 = DOVirtual.DelayedCall(sfx.time, () =>
            {
                curIdx = -1;
            });
        }
        public void PlayIngame(AudioClip clip)
        {
            if (music == null) return;

            if (music.isPlaying) music.Stop();

            music.clip = clip;
            music.Play();
        }

        #endregion

        public void PlayOtherSfx(AudioClip clip)
        {
            if(clip == null || sfx == null) return;
            if (delayTwen != null) delayTwen?.Kill();
            sfx.clip = clip;
            sfx.Play();
        }
        public void PlayOtherSfx(SfxOtherType type, bool isForcePlay)
        {
            if (sfx == null) return;
            int idx = (int)type;

            curIdx = idx;
            sfx.clip = sfxOthers[idx];
            sfx.Play();

            if (delayTwen2 != null) delayTwen2?.Kill();
            delayTwen2 = DOVirtual.DelayedCall(sfx.time, () =>
            {
                curIdx = -1;
            });
        }
        public void PlayOtherSfx(SfxOtherType type, float delayTime, float stopTime = -1)
        {
            if (sfx == null) return;
            if (delayTwen != null) delayTwen?.Kill();
            delayTwen = DOVirtual.DelayedCall(delayTime, () =>
            {
                int idx = (int)type;
                for (int i = 0; i < sfxOthers.Count; i++)
                {
                    if (i == curIdx && sfx.isPlaying) return;
                }

                curIdx = idx;
                sfx.clip = sfxOthers[idx];
                sfx.Play();

                if (delayTwen2 != null) delayTwen2?.Kill();
                delayTwen2 = DOVirtual.DelayedCall(sfx.time, () =>
                {
                    curIdx = -1;
                });

                if (stopTime > 0)
                {
                    delayTwen = DOVirtual.DelayedCall(stopTime, () =>
                    {
                        sfx.Stop();
                    });
                }
            });
        }
        public void PlayLoopingSfx(SfxOtherType type)
        {
            if (loopingAus == null) return;
            if (loopingAus.isPlaying) loopingAus.Stop();

            loopingAus.clip = sfxOthers[(int)type];
            loopingAus.Play();
        }
        public void PlayLoopingSfx(AudioClip clip)
        {
            if (loopingAus == null) return;

            loopingAus.clip = clip;
            loopingAus.Play();
        }
        public void TurnOffLoop()
        {
            if (loopingAus == null) return;
            loopingAus.Stop();
        }
        public void MuteSound(SoundType type, bool isMain = false)
        {
            if (music == null) return;

            if (type == SoundType.SFx)
            {
                speaker.volume = 0;
                sfx.volume = 0;
                loopingAus.volume = 0;
            }
            else if (type == SoundType.Music)
            {
                IsMuted = true;
                music.volume = 0;
            }
        }
        public void EnableSound(SoundType type, bool isMain = false)
        {
            if (!isMain && IsMuted) return;
            if (music == null) return;

            if (type == SoundType.SFx)
            {
                sfx.volume = 1;
                speaker.volume = 1;
                loopingAus.volume = 1;
            }
            else if (type == SoundType.Music)
            {
                music.volume = startVolumeMusic;
                IsMuted = false;
            }
        }
    }
    public enum SoundType
    {
        Music,
        SFx,
    }
    public enum OtherBackSoundType
    {
        Piano
    }
    public enum IngameType
    {
        Home,
        Capture,
        Toy,
        ClawMachine,
        Floor2,
        SuperMarket,
        Cinema,
        Cake,
        House,
        Hospital,
        Opera,
        Playground
    }

    public enum SfxOtherType
    {
        Click,
        Correct,
        Incorrect,
        Lighting,
        ControlMoving,
        RingLighting,
        PourCream,
        Watering,
        Congratulation,
        BreadMachine,
        RainbowScratch,
        FallingDown,
        Shaking,
        Scratch,
        Printing,
        BasketBall,
        Swinging,
        SlideWhistle,
        WindChime,
        Capture,
        Blender,
        Elevator,
        SlideDoor,
        Dicing,
    }
}
