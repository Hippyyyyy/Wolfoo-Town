using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
namespace _WolfooShoppingMall
{
    public class WoodHorse : BackItem
    {
        [SerializeField] WoodHorseAnimation anim;
        [SerializeField] Transform sitZone;
        private float distance;
        private BackItem curItem;

        protected override void InitItem()
        {
            canClick = true;
        }

        protected override void Start()
        {
            base.Start();
            anim.PlayIdle();
        }

        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.backitem == this) return;
            if (item.character != null && curItem != null && item.character == curItem)
            {
                curItem = null;
                anim.PlayIdle();
            }
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == this) return;
            if (item.character != null)
            {
                distance = Vector2.Distance(item.character.transform.position, transform.position);
                if (distance < 2)
                {
                    curItem = item.character;
                    item.character.OnSitDownHorse(sitZone);
                    anim.PlayAnim(true);
                }
            }
            if (item.newCharacter != null)
            {
                distance = Vector2.Distance(item.newCharacter.transform.position, transform.position);
                if (distance < 2)
                {
                    curItem = item.newCharacter;
                    item.newCharacter.OnSitDownHorse(sitZone);
                    anim.PlayAnim(true);
                }
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            anim.PlayAnim(false);
        }
    }

}