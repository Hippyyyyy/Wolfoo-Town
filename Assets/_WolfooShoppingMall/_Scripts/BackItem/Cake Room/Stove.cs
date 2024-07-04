using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Stove : BackItem
    {
        [SerializeField] Button doorBtn;
        [SerializeField] Image heatingImg;
        [SerializeField] Transform itemZone;
        [SerializeField] ParticleSystem smokeFx;

        private int status;
        private List<Cake> curCakes = new List<Cake>();
        private Tween tweenDelay;
        private Tweener fadeTween;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

            doorBtn.onClick.AddListener(OnClickDoor);
        }

        private void OnClickDoor()
        {
            if (!canClick) return;

            status = 1 - status;
            doorBtn.image.sprite = data.doorData.stoveDoorSprites[status];
            doorBtn.image.SetNativeSize();

            if (status == 1)
            {
                doorBtn.image.rectTransform.pivot = new Vector2(0.39f, 1f);
            }
            else
            {
                doorBtn.image.rectTransform.pivot = new Vector2(0.5f, 0f);
                OnBaking();
            }
        }

        private void OnBaking()
        {
            if (curCakes.Count > 0)
            {
                canClick = false;
                fadeTween = heatingImg.DOFade(1, 0.5f).SetLoops(-1, LoopType.Yoyo);
                foreach (var item in curCakes)
                {
                    item.OnBaking();
                }

                tweenDelay = DOVirtual.DelayedCall(2, () =>
                {
                    if (fadeTween != null) fadeTween?.Kill();
                    smokeFx.Play();

                    tweenDelay = DOVirtual.DelayedCall(1, () =>
                    {
                        canClick = true;
                        OnClickDoor();
                    });
                });
            }
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (status == 0) return;
            if (item.cake != null)
            {
                if (Vector2.Distance(item.cake.transform.position, itemZone.position) > 1)
                {
                    if (curCakes.Contains(item.cake)) curCakes.Remove(item.cake);
                }
                else
                {
                    CheckPriority(() =>
                    {
                        item.cake.JumpToStove(itemZone);
                        curCakes.Add(item.cake);
                    });
                }
            }
        }
    }
}