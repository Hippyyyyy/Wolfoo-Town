using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Door : BackItem
    {
        [SerializeField] Animator animator;
        [SerializeField] DoorAnimation doorAnimation;
        [SerializeField] ElevatorAnimation elevatorAnimation;
        [Range(0, 1)]
        [SerializeField] int state;
        [SerializeField] Button button;
        [SerializeField] PanelType mapPanelType;
        [SerializeField] Sprite[] sprites;
        [SerializeField] bool isMoveDoor;
        [SerializeField] Vector3[] moveLocalPos;

        private GameObject curMap;
        private Tweener scaleTween;
        private Tweener moveTween;

        public Action OnTouching;
        public Action<Door> OnTouched;
        private int myIdx;

        public bool IsOpen
        {
            get { return state == 1; }
            private set { IsOpen = value; }
        }

        public int Idx { get => myIdx; }

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

            if (doorAnimation != null)
            {
                if (state == 1) doorAnimation.PlayExcute();
                else doorAnimation.PlayIdle();
            }

            if (elevatorAnimation != null)
            {
                if (state == 0)
                    elevatorAnimation.PlayCloseAnim();
                else
                    elevatorAnimation.PlayOpenAnim();
            }

            if (button != null)
            {
                button.onClick.AddListener(OnSpawnMap);
                if (scaleTween != null) scaleTween?.Kill();
                button.interactable = false;
                scaleTween = button.transform.DOScale(Vector3.one * state, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
                {
                    button.interactable = true;
                    moveTween = button.transform.DOPunchPosition(Vector3.up * 50, 1f, 3).SetLoops(-1, LoopType.Yoyo);
                });

            }
        }

        public void AssignIndex(int idx)
        {
            myIdx = idx;
        }

        public void OnAnimatorCompleted()
        {
            Debug.Log("Animator Completed");
            state = 1- state;
            OnTouched?.Invoke(this);
        }

        private void OnSpawnMap()
        {
            if (scaleTween != null) scaleTween?.Kill();
            button.interactable = false;
            scaleTween = button.transform.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.5f, 1).OnComplete(() =>
            {
                button.interactable = true;
            });

            GUIManager.instance.OpenPanel(mapPanelType);
        }

        public void ChangeStateSprite(bool isClose)
        {
            canClick = false;

            state = isClose ? 0 : 1;
            image.sprite = sprites[state];
            image.SetNativeSize();

            if (isMoveDoor)
            {
                image.transform.localPosition = moveLocalPos[state];
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnTouching?.Invoke();

            if (!canClick) return;

            if (myClip != null) SoundManager.instance.PlayOtherSfx(myClip);
            else if(SoundBaseRoomManager.Instance != null) SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.ClickDoor);

            if(animator != null)
            {
                if (IsOpen) animator.SetTrigger("Close");
                else animator.SetTrigger("Open");
                return;
            }

            state = 1 - state;

            if (elevatorAnimation != null)
            {
                if (state == 0)
                {
                    elevatorAnimation.PlayCloseAnim();
                }
                else
                {
                    elevatorAnimation.PlayOpenAnim();
                }
                return;
            }

            if (doorAnimation != null)
            {
                if (state == 1)
                {
                    SoundManager.instance.PlayOtherSfx(SfxOtherType.SlideDoor);
                    doorAnimation.PlayExcute();
                }
                else
                {
                    SoundManager.instance.PlayOtherSfx(SfxOtherType.SlideDoor);
                    doorAnimation.PlayIdle();
                }
            }

            if (button != null)
            {
                if (scaleTween != null) scaleTween?.Kill();
                button.interactable = false;
                if (state == 0)
                {
                    scaleTween = button.transform.DOScale(Vector3.one * 0, 0.25f)
                        .OnComplete(() =>
                        {
                            button.interactable = true;
                        });
                }
                else
                {
                    scaleTween = button.transform.DOScale(Vector3.one * 1, 0.5f)
                        .SetEase(Ease.OutBack)
                        .OnComplete(() =>
                        {
                            button.interactable = true;
                        });
                }
            }

            if (sprites.Length > 0)
            {
                image.sprite = sprites[state];
                image.SetNativeSize();

                if(isMoveDoor)
                {
                    image.transform.localPosition = moveLocalPos[state];
                }

                OnTouched?.Invoke(this);
            } 
        }
    }

    public enum MapType
    {
        Toy,
        Cinema,
        Bakery,
        SuperMarket
    }
}