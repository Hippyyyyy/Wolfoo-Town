using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class FruitFertilizer : Fertilizer
    {
        protected override void InitData()
        {
            canDrag = true;
            base.InitData();
            myData = DataSceneManager.Instance.HouseData.FruitSprites;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, fertilizer = this });
        }
    }
}
