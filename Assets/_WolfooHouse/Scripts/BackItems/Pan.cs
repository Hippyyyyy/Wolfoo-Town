using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Pan : BackItem
    {
        protected override void InitItem()
        {
            canDrag = true;
            base.InitItem();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { pan = this, backitem = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { pan = this, backitem = this });
        }
    }
}
