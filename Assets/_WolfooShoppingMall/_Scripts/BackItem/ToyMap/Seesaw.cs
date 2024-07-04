using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Seesaw : BackItem
    {
        [SerializeField] SeesawAnimation seesawAnimation;
        [SerializeField] SeesawAnimation seesawMaskAnimation;
        [SerializeField] Transform sitZone;

        private float distance;
        private List<BackItem> curCharacters = new List<BackItem>();
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
            seesawMaskAnimation.PlayExcute();

            if (tweenDelay != null) tweenDelay?.Kill();
            tweenDelay = DOVirtual.DelayedCall(seesawAnimation.GetTimeAnimation(SeesawAnimation.AnimState.Excute), () =>
            {
                seesawAnimation.PlayIdle();
                seesawMaskAnimation.PlayIdle();
            });
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.character != null)
            {
                if (curCharacters.Contains(item.character))
                {
                    curCharacters.Remove(item.character);

                    if (curCharacters.Count == 0)
                    {
                        seesawAnimation.PlayIdle();
                        seesawMaskAnimation.PlayIdle();

                        seesawAnimation.SkeletonAnim.raycastTarget = true;
                        seesawMaskAnimation.SkeletonAnim.raycastTarget = true;
                    }
                }
            }
            if (item.newCharacter != null)
            {
                if (curCharacters.Contains(item.newCharacter))
                {
                    curCharacters.Remove(item.newCharacter);

                    if (curCharacters.Count == 0)
                    {
                        seesawAnimation.PlayIdle();
                        seesawMaskAnimation.PlayIdle();

                        seesawAnimation.SkeletonAnim.raycastTarget = true;
                        seesawMaskAnimation.SkeletonAnim.raycastTarget = true;
                    }
                }
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (item.character != null)
            {
                distance = Vector2.Distance(item.character.transform.position, sitZone.position);
                if (distance < 2)
                {
                    curCharacters.Add(item.character);

                    item.character.OnSitToSeesaw(sitZone);

                    seesawAnimation.PlayExcute();
                    seesawMaskAnimation.PlayExcute();

                    seesawAnimation.SkeletonAnim.raycastTarget = false;
                    seesawMaskAnimation.SkeletonAnim.raycastTarget = false;
                }
            }
            if (item.newCharacter != null)
            {
                distance = Vector2.Distance(item.newCharacter.transform.position, sitZone.position);
                if (distance < 2)
                {
                    curCharacters.Add(item.newCharacter);

                    item.newCharacter.OnSitToSeesaw(sitZone);

                    seesawAnimation.PlayExcute();
                    seesawMaskAnimation.PlayExcute();

                    seesawAnimation.SkeletonAnim.raycastTarget = false;
                    seesawMaskAnimation.SkeletonAnim.raycastTarget = false;
                }
            }
        }
    }
}