using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ClickCharacterAnim : MonoBehaviour
    {
        [SerializeField] bool isAutoPlay;
        [SerializeField] bool isRandom;
        [SerializeField] AnimType myType = AnimType.Dance2;
        [SerializeField] float strength = 0.1f;
        [SerializeField] float time = 0.25f;
        [SerializeField] Animator animator;
        [SerializeField] string animName;
        [SerializeField] bool registerUIEvent;
        private Tweener _tweenAnim;
        private Tween _tweenAutoPlay;
        private Vector3 startScale;
        private EventTrigger trigger;

        public enum AnimType
        {
            Dance1 = 1,
            Dance2 = 2,
            Anim = 3,
        }
        private void Start()
        {
            startScale = transform.localScale;
            if(isAutoPlay)
            {
                PlayAuto();
            }
            if(registerUIEvent)
            {
                trigger = gameObject.AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerDown;
                entry.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
                trigger.triggers.Add(entry);
            }
        }

        private void OnDestroy()
        {
            if (_tweenAutoPlay != null) _tweenAutoPlay?.Kill();
            if (registerUIEvent)
            {
                trigger.triggers.Clear();
            }
        }

        private void OnPointerDownDelegate(PointerEventData data)
        {
            OnClick();
        }

        private void OnMouseDown()
        {
            OnClick();
        }

        private void OnClick()
        {
            if (isRandom)
            {
                myType = GetRandomEnumValue();
            }
            PlayAnim();
        }

        private void PlayAuto()
        {
            if (_tweenAutoPlay != null) _tweenAutoPlay?.Kill();
            _tweenAutoPlay = DOVirtual.DelayedCall(2, () =>
            {
                PlayAnim();
                _tweenAutoPlay = DOVirtual.DelayedCall(time, () =>
                {
                    PlayAuto();
                });
            });
        }
        AnimType GetRandomEnumValue()
        {
            return AnimType.GetValues(typeof(AnimType))
                .OfType<AnimType>()
                .OrderBy(e => Guid.NewGuid())
                .FirstOrDefault();           
        }
        private void PlayAnim()
        {
            switch (myType)
            {
                case AnimType.Dance1:
                    PlayDance1();
                    break;
                case AnimType.Dance2:
                    PlayDance2();
                    break;
                case AnimType.Anim:
                    animator.enabled = true;
                    animator.Play(animName, 0, 0);
                    break;
            }
        }

        private void PlayDance2()
        {
            if (_tweenAnim != null) _tweenAnim?.Kill();
            transform.localScale = startScale;
            _tweenAnim = transform.DOPunchScale(new Vector3(1 * strength, -1 * strength, -1) , time).OnComplete(() =>
            {
                if (isAutoPlay) PlayAuto();
            });
        }

        private void PlayDance1()
        {
            if (_tweenAnim != null) _tweenAnim?.Kill();
            transform.localScale = startScale;
            _tweenAnim = transform.DOPunchScale(new Vector3(1 * strength, 1 * strength, -1) , time).OnComplete(() =>
            {
                if (isAutoPlay) PlayAuto();
            });
        }
    }
}
