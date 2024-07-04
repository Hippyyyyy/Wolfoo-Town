using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class JumpingToy : BackItem
    {
        [SerializeField] Transform sitZone;
        [SerializeField] JumpingToyAnimation jumpingToyAnimation;
        private BackItem curItem;

        protected override void InitItem()
        {

        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.character != null)
            {
                if (curItem == item.character)
                {
                    curItem = null;
                }
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.character != null)
            {
                //   if (isSitted) return;
                if (Vector2.Distance(item.character.transform.position, sitZone.position) < 2)
                {
                    curItem = item.character;
                    item.character.Jumping(
                        new Vector3(Random.Range(sitZone.GetChild(0).position.x, sitZone.GetChild(1).position.x), sitZone.transform.position.y, 0),
                        sitZone);
                    jumpingToyAnimation.PlayExcute();
                }
            }
            if (item.newCharacter != null)
            {
                //   if (isSitted) return;
                if (Vector2.Distance(item.newCharacter.transform.position, sitZone.position) < 2)
                {
                    curItem = item.newCharacter;
                    item.newCharacter.Jumping(
                        new Vector3(Random.Range(sitZone.GetChild(0).position.x, sitZone.GetChild(1).position.x), sitZone.transform.position.y, 0),
                        sitZone);
                    jumpingToyAnimation.PlayExcute();
                }
            }
        }
    }
}