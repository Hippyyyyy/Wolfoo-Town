using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class HorseFerrisWheel : BackItem
    {
        [SerializeField] Animator _animator;
        [SerializeField] ControllerMachine controller;
        private bool isPlaying;
        private bool _isPlaying;
        private AudioSource myAus;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;

            myAus = SoundPlaygroundManager.Instance.CreateNewAus(new SoundBase<SoundPlaygroundManager>.Item(GetInstanceID(), "AUS - Horse Ferris Wheel", myClip, true));
            controller.TurnOn();
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
        }
        private void Update()
        {
            Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
            if (viewPos.x >= 0 && viewPos.x <= 1 && viewPos.y >= 0 && viewPos.y <= 1 && viewPos.z > 0)
            {
                // Your object is in the range of the camera, you can apply your behaviour
                if (isPlaying)
                {
                    if (myAus.isPlaying) return;
                    myAus.Play();
                }
            }
            else
            {
                if (isPlaying) myAus.Stop();
            }
        }

        private void GetStateChange(bool isPlaying)
        {
            this.isPlaying = isPlaying;
            _animator.Play(isPlaying ? "Playing" : "Idle", 0, 0);

            _isPlaying = isPlaying;

            if (isPlaying) myAus.Play();
            else myAus.Stop();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;


            isPlaying = !isPlaying;
            _animator.Play(isPlaying ? "Playing" : "Idle", 0, 0);
            if (isPlaying) controller.TurnOn();
            else controller.TurnOff();
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
    }
}