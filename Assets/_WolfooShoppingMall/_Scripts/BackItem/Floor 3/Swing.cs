using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Swing : BackItem
    {
        [SerializeField] SwingAnimation swingAnimation;
        [SerializeField] Transform[] sitZones;
        private float distance;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
            swingAnimation.PlayIdle();
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.character != null)
            {
                if (sitZones[0].childCount == 0)
                {
                    if (sitZones[1].childCount == 0)
                    {
                        swingAnimation.PlayIdle();
                        return;
                    }
                    swingAnimation.PlayExcuteRight();
                    return;
                }

                if (sitZones[1].childCount == 0)
                {
                    swingAnimation.PlayExcuteLeft();
                    return;
                }

                swingAnimation.PlayExcuteBoth();
            }
            if (item.newCharacter != null)
            {
                if (sitZones[0].childCount == 0)
                {
                    if (sitZones[1].childCount == 0)
                    {
                        swingAnimation.PlayIdle();
                        return;
                    }
                    swingAnimation.PlayExcuteRight();
                    return;
                }

                if (sitZones[1].childCount == 0)
                {
                    swingAnimation.PlayExcuteLeft();
                    return;
                }

                swingAnimation.PlayExcuteBoth();
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (item.character != null)
            {
                for (int i = 0; i < sitZones.Length; i++)
                {
                    if (sitZones[i].childCount == 1) continue;

                    distance = Vector2.Distance(item.character.transform.position, sitZones[i].position);
                    if (distance < 2)
                    {
                        item.character.OnSitToChair(sitZones[i].position, sitZones[i]);
                        if (i == 0)
                        {
                            swingAnimation.PlayExcuteLeft();
                            continue;
                        }
                        else
                        {
                            swingAnimation.PlayExcuteRight();
                            continue;
                        }
                    }
                }

                foreach (var zone in sitZones)
                {
                    if (zone.childCount == 0) return;
                }
                swingAnimation.PlayExcuteBoth();
            }
            if (item.newCharacter != null)
            {
                for (int i = 0; i < sitZones.Length; i++)
                {
                    if (sitZones[i].childCount == 1) continue;

                    distance = Vector2.Distance(item.newCharacter.transform.position, sitZones[i].position);
                    if (distance < 2)
                    {
                        item.newCharacter.OnSitToChair(sitZones[i].position, sitZones[i]);
                        if (i == 0)
                        {
                            swingAnimation.PlayExcuteLeft();
                            continue;
                        }
                        else
                        {
                            swingAnimation.PlayExcuteRight();
                            continue;
                        }
                    }
                }

                foreach (var zone in sitZones)
                {
                    if (zone.childCount == 0) return;
                }
                swingAnimation.PlayExcuteBoth();
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (canClick) return;

            swingAnimation.PlayExcuteBoth();
        }

    }
}