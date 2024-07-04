using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class CinemaCoupleChair : BackItem
    {
        [SerializeField] Sprite[] statusSprites;
        [SerializeField] Transform[] sitZone;
        [SerializeField] Transform[] cupZones;

        private List<bool> enableSitZones = new List<bool>();
        private int curIdx;
        private float distance;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
            for (int i = 0; i < sitZone.Length; i++)
            {
                enableSitZones.Add(false);
            }
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);

            if (item.backitem != null)
            {
                if (item.backitem.IsBeverage)
                {

                }

                if (item.character != null)
                {
                }
            }
        }


        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == null) return;

            if (item.character != null)
            {
                for (int i = 0; i < sitZone.Length; i++)
                {
                    if (!enableSitZones[i]) continue;
                    if (sitZone[i].childCount > 0) continue;

                    distance = Vector2.Distance(item.backitem.transform.position, sitZone[i].position);
                    if (distance < 2)
                    {
                        item.character.OnSitToChair(sitZone[i].position, sitZone[i], true);
                    }
                }
            }
            if (item.newCharacter != null)
            {
                for (int i = 0; i < sitZone.Length; i++)
                {
                    if (!enableSitZones[i]) continue;
                    if (sitZone[i].childCount > 0) continue;

                    distance = Vector2.Distance(item.backitem.transform.position, sitZone[i].position);
                    if (distance < 2)
                    {
                        item.newCharacter.OnSitToChair(sitZone[i].position, sitZone[i], true);
                    }
                }
            }

            if (item.backitem.IsBeverage)
            {
                for (int i = 0; i < cupZones.Length; i++)
                {
                    if (cupZones[i].childCount > 0) continue;

                    distance = Vector2.Distance(item.backitem.StandZone.position, cupZones[i].position);
                    if (distance > 1) continue;

                    // Cup use method to sit here
                    item.backitem.transform.SetParent(cupZones[i]);
                    item.backitem.JumpToEndLocalPos(Vector3.up * item.backitem.transform.position.y, null, Ease.OutBounce);
                    return;
                }
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            curIdx++;
            if (curIdx == statusSprites.Length) curIdx = 0;

            image.sprite = statusSprites[curIdx];
            switch (curIdx)
            {
                case 0:
                    enableSitZones[0] = false;
                    enableSitZones[1] = true;
                    break;
                case 1:
                    enableSitZones[0] = true;
                    enableSitZones[1] = true;
                    break;
                case 2:
                    enableSitZones[0] = false;
                    enableSitZones[1] = false;
                    break;
                case 3:
                    enableSitZones[0] = true;
                    enableSitZones[1] = false;
                    break;
            }
        }
    }
}