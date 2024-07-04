using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class PopcornMachine : BackItem
    {
        [SerializeField] PopcornMachineAnimation machineAnimation;
        [SerializeField] Transform popcornBoxZone;
        [SerializeField] Transform popcornZone;
        [SerializeField] Popcorn popcornPb;

        private float distance;
        private bool isHasPopcorn;

        private List<Popcorn> curPopcorns = new List<Popcorn>();
        private PopcornBox curPopcornBox;
        private Tween delayTweenItem;

        protected override void Start()
        {
            base.Start();
            machineAnimation.PlayIdle();

            if (popcornBoxZone.childCount > 0)
            {
                curPopcornBox = popcornBoxZone.GetComponentInChildren<PopcornBox>();
                curPopcornBox.transform.SetParent(popcornBoxZone);
                curPopcornBox.JumpToEndLocalPos(Vector3.zero, () =>
                {
                    AssignClick();
                });
            }
        }
        private void OnDestroy()
        {
            if (delayTweenItem != null) delayTweenItem?.Kill();
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem obj)
        {
            base.GetEndDragItem(obj);
            if (obj.popcornBox != null)
            {
                distance = Vector2.Distance(popcornBoxZone.position, obj.popcornBox.StandZone.position);
                if (distance < 2)
                {
                    curPopcornBox = obj.popcornBox;
                    obj.popcornBox.transform.SetParent(popcornBoxZone);
                    obj.popcornBox.JumpToEndLocalPos(Vector3.zero, () =>
                    {
                        AssignClick();
                    });

                    transform.parent.SetAsLastSibling();
                }
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            if (isHasPopcorn)
            {
                curPopcornBox.CheckHasPopcorn();
                if (curPopcornBox == null)
                {
                    canClick = true;
                    return;
                }
                if (curPopcornBox.IsHasPopcorn)
                {
                    curPopcornBox.OnPunchScale();
                    canClick = true;
                    return;
                }

                isHasPopcorn = false;
                machineAnimation.PlayIdle();

                //for (int i = 0; i < curPopcorns.Count; i++)
                //{
                //    int idx = i;
                //    curPopcorns[i].gameObject.SetActive(true);
                //    curPopcorns[idx].JumpToEndPos(
                //        curPopcornBox.ChildPopcornZone[idx].position,
                //        () =>
                //        {
                //            curPopcorns[idx].AssignDrag();
                //        },
                //        Ease.OutBounce,
                //        false);
                //}

                curPopcornBox.GetPopcorn(curPopcorns);

                delayTweenItem = DOVirtual.DelayedCall(1, () =>
                {
                    canClick = true;
                });
            }
            else
            {
                isHasPopcorn = true;
                curPopcorns.Clear();
                machineAnimation.PlayExcute();
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Blender);

                delayTweenItem = DOVirtual.DelayedCall(machineAnimation.GetTimeAnimation(PopcornMachineAnimation.AnimState.Excute), () =>
                {
                    machineAnimation.PlayIdle2();
                    for (int i = 0; i < popcornZone.childCount; i++)
                    {
                        var popcorn = Instantiate(popcornPb, popcornZone.GetChild(i));
                        popcorn.transform.localPosition = Vector3.zero;
                        popcorn.gameObject.SetActive(false);

                        curPopcorns.Add(popcorn);
                    }
                    delayTweenItem = DOVirtual.DelayedCall(0.5f, () =>
                    {
                        canClick = true;
                    });
                });
            }
        }
        protected override void InitItem()
        {
            canClick = true;
        }
    }
}