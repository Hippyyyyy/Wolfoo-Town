using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class CoupleSeesaw : BackItem
    {
        [SerializeField] SeesawAnimation seesawAnimation;
        [SerializeField] Transform[] sitZones;

        private float distance;
        private BackItem curCharacter;
        private Tween tweenDelay;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            seesawAnimation.PlayExcute();

            if (tweenDelay != null) tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(seesawAnimation.GetTimeAnimation(SeesawAnimation.AnimState.Excute), () =>
            {
                seesawAnimation.PlayIdle();
            });
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (item.character != null)
            {
                canClick = false;
                var count = 0;
                foreach (var sitZone in sitZones)
                {
                    distance = Vector2.Distance(sitZone.position, item.character.transform.position);
                    if (distance < 2 && sitZone.childCount == 0)
                    {
                        curCharacter = item.character;
                        item.character.OnSitToSeesaw(sitZone, count == 0 ? Direction.Right : Direction.Left);

                        seesawAnimation.PlayExcute();
                    }
                    count++;
                }

                foreach (var sitzone in sitZones)
                {
                    if (sitzone.childCount > 0) return;
                }

                seesawAnimation.PlayIdle();
                canClick = true;
            }
            if (item.newCharacter != null)
            {
                canClick = false;
                var count = 0;
                foreach (var sitZone in sitZones)
                {
                    if (sitZone.childCount > 0) return;

                    distance = Vector2.Distance(sitZone.position, item.newCharacter.transform.position);
                    if (distance < 2)
                    {
                        curCharacter = item.newCharacter;
                        item.newCharacter.OnSitToSeesaw(sitZone, count == 0 ? Direction.Right : Direction.Left);

                        seesawAnimation.PlayExcute();
                    }
                    count++;
                }

                foreach (var sitzone in sitZones)
                {
                    if (sitzone.childCount > 0) return;
                }

                seesawAnimation.PlayIdle();
                canClick = true;
            }
        }
    }
}