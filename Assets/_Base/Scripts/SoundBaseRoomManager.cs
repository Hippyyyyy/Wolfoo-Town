using _Base;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SoundBaseRoomManager : SoundBase<SoundBaseRoomManager>
    {
        [SerializeField] private SfxData[] myData;
        public enum SfxType
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
            Lamp,
            ClickDoor,
            Garbage,
        }

        [System.Serializable]
        public struct SfxData
        {
            public AudioClip clip;
            public SfxType sfxType;
        }

        public void Play(AudioClip clip)
        {
            if (IsSoundMuted) return;
            if (clip == null || sfx == null) return;

            sfx.clip = clip;
            sfx.Play();
        }
        public void Play(SfxType sfxType)
        {
            if (IsSoundMuted) return;
            if (sfx == null) return;

            foreach (var item in (myData))
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