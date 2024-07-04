using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Droptown : BackItem
    {
        [SerializeField] Animator _animator;
        [SerializeField] int maxLighting;
        [SerializeField] Transform lightingArea1;
        [SerializeField] Transform lightingArea2;
        [SerializeField] Led ledPb;
        [SerializeField] ControllerMachine controller;
        private List<Led> leds1;
        private List<Led> leds2;
        private bool isTurnOn;
        private Tween _tween;
        private AudioSource myAus;
        private Tween _tweenSound;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;

            ledPb.gameObject.SetActive(false);
            leds1 = new List<Led>();
            leds2 = new List<Led>();
            for (int i = 0; i < maxLighting; i++)
            {
                var light = Instantiate(ledPb, lightingArea1);
                var light2 = Instantiate(ledPb, lightingArea2);
                light.gameObject.SetActive(true);
                light2.gameObject.SetActive(true);
                light.Init();
                light2.Init();
                leds1.Add(light);
                leds2.Add(light2);
            }

            myAus = SoundPlaygroundManager.Instance.CreateNewAus(new SoundBase<SoundPlaygroundManager>.Item(GetInstanceID(), "AUS - Droptown", myClip, true));
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            controller.OnLevelChanged += GetLevelChange;
            controller.OnStateChanged += GetStateChange;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            controller.OnLevelChanged -= GetLevelChange;
            controller.OnStateChanged -= GetStateChange;
            if (_tween != null) _tween?.Kill();
            if (_tweenSound != null) _tweenSound?.Kill();
        }
        private void GetStateChange(bool isPlaying)
        {
            isTurnOn = isPlaying;
            if (isPlaying)
            {
                OnPlay();
            }
            else
            {
                OnStop();
            }

        }
       private void OnPlay()
        {
            PlayAnim();
            myAus.Play();
            _tweenSound?.Kill();
            _tweenSound = DOVirtual.DelayedCall(2, () =>
            {
                SoundCharacterManager.Instance.PlayWolfooInteresting();
            }).SetLoops(-1);
        }
        private void OnStop()
        {
            StopAnim();
            myAus.Stop();
            _tweenSound?.Kill();
        }
        private void GetLevelChange()
        {
            switch (controller.CurLevel)
            {
                case 1:
                    _animator.speed = 1;
                    break;
                case 2:
                    _animator.speed = 1.5f;
                    break;
                case 3:
                    _animator.speed = 2f;
                    break;
                case 4:
                    _animator.speed = 2.5f;
                    break;
            }
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            isTurnOn = !isTurnOn;
            if (isTurnOn)
            {
                OnPlay();
            }
            else
            {
                OnStop();
            }
        }

        void PlayAnimLighting()
        {
            var speed = 0.1f;
            var delayTime = (speed + ledPb.Speed) * leds2.Count;
            for (int i = 0; i < leds1.Count; i++)
            {
                leds1[i].Shine(speed * i);
                leds2[i].Shine(speed * i);
            }

            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(delayTime, () =>
            {
                for (int i = leds2.Count - 1; i >= 0; i--)
                {
                    leds1[i].Shine(speed * i);
                    leds2[i].Shine(speed * i);
                }
                _tween = DOVirtual.DelayedCall(delayTime, () =>
                {
                    PlaySubLighting();
                });
            });
        }
        void PlaySubLighting()
        {
            for (int i = 0; i < leds1.Count; i++)
            {
                leds1[i].Shine(0);
                leds2[i].Shine(0);
            }
            _tween = DOVirtual.DelayedCall(ledPb.Speed + 0.1f, () =>
            {
                PlaySubLighting();
            });
        }
        void StopAnimLighting()
        {
            _tween?.Kill();
        }

        void PlayAnim()
        {
            _animator.Play("Playing", 0, 0);
            PlayAnimLighting();
        }
        void StopAnim()
        {
            _animator.Play("Idle", 0, 0);
            StopAnimLighting();
        }
    }
}