using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ColorCakeMachine : BackItem
    {
        [SerializeField] ColorCakeMachineAnimation machineAnimation;
        [SerializeField] Transform itemZone;
        [SerializeField] ParticleSystem rainbowFx;
        [SerializeField] HandmadeCakeMode handmadeCakeModePb;

        private Tween delayItemTween;
        private HandMadeCake curItem;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
        }

        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.handmadeCake != null)
            {
                canClick = false;
                machineAnimation.PlayIdle();

                SoundManager.instance.PlayOtherSfx(SfxOtherType.RainbowScratch);

                curItem = obj.handmadeCake.MainCake;
                curItem.transform.SetParent(itemZone);
                curItem.transform.localPosition = Vector3.zero;
                curItem.transform.localScale = Vector3.zero * 0.1f;
                curItem.GetComponent<RectTransform>().pivot = Vector2.right * 0.5f;

                delayItemTween = DOVirtual.DelayedCall(1, () =>
                {
                    // Sound Rainbow Scratch Here

                    machineAnimation.PlayExcute();
                    curItem.transform.DOScale(
                        Vector3.one * 0.2f,
                        machineAnimation.GetTimeAnimation(ColorCakeMachineAnimation.AnimState.Excute))
                    .OnComplete(() =>
                    {
                        rainbowFx.Play();
                        machineAnimation.PlayIdle();

                        curItem.transform.DOPunchPosition(Vector3.forward * 40, 0.3f, 4);
                        curItem.transform.DOScale(0.4f, 0.3f).SetEase(Ease.OutBack).OnComplete(() =>
                        {
                            curItem.AssignCake();
                        });

                        delayItemTween = DOVirtual.DelayedCall(1, () =>
                        {
                            canClick = true;
                        });
                    });
                });
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            // Sound Click Here
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            Instantiate(handmadeCakeModePb, GUIManager.instance.canvasSpawnMode.transform);
        }
    }
}