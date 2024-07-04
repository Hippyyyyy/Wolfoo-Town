using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ChairItem : BackItem
    {
        private float distance_;
        [SerializeField] Transform sitTrans;
        [SerializeField] bool isRightSide;
        private BackItem myItem;

        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if(item.character != null)
            {
                if (item.character == myItem) myItem = null;
            }
            if(item.newCharacter != null)
            {
                if (item.newCharacter == myItem) myItem = null;
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (myItem != null) return;
            if (item.character != null)
            {
                distance_ = Vector2.Distance(item.character.transform.position, sitTrans.position);
                if (distance_ <= 1)
                {
                    myItem = item.character;
                    item.character.OnSitToChair(sitTrans.position, sitTrans, isRightSide);
                }
            }
            if (item.newCharacter != null)
            {
                distance_ = Vector2.Distance(item.newCharacter.transform.position, sitTrans.position);
                if (distance_ <= 1)
                {
                    myItem = item.newCharacter;
                    item.newCharacter.OnSitToChair(sitTrans.position, sitTrans, isRightSide);
                }
            }
        }
    }

}