using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Bed : BackItem
    {
        [SerializeField] Animator animator_;
        [SerializeField] Transform sleepZone;
        [SerializeField] string triggerStr;
        private BackItem curCharacter;

        protected override void InitItem()
        {
            canClick = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            animator_.SetTrigger(triggerStr);
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.character == null) return;
            if (curCharacter == null) return;
            if(curCharacter == item.character)
            {
                item.character.transform.rotation = Quaternion.Euler(Vector3.zero);
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == null) return;

            if (item.character != null)
            {
                if (Vector2.Distance(item.character.transform.position, sleepZone.position) < 2 && item.character.transform.position.y > sleepZone.transform.position.y)
                {
                    if (curCharacter != null) return;

                    curCharacter = item.character;
                    item.character.OnGoToBed(sleepZone.position, sleepZone);
                    item.character.transform.localRotation = Quaternion.Euler(0,0,0);
                }
                else
                {
                    if(item.character == curCharacter)
                    {
                        curCharacter = null;
                    }
                }
            }
            else if (item.newCharacter)
            {
                if (Vector2.Distance(item.newCharacter.transform.position, sleepZone.position) < 2)
                {
                    if (curCharacter != null) return;

                    curCharacter = item.newCharacter;
                    item.newCharacter.OnGoToBed(sleepZone.position, sleepZone);
                    item.newCharacter.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    if (item.newCharacter == curCharacter)
                    {
                        curCharacter = null;
                    }
                }
            }
        }
    }
}