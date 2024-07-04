using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _WolfooShoppingMall.SelfHouseDataSO;

namespace _WolfooShoppingMall
{
    public class PaintingFloor : PaintingObject
    {
        private BeachVillaDataSO.PaintingData[] data;

        private void Start()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(GetInitItem);
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnLoadDataCompleted>(GetInitItem);
        }
        private void GetInitItem(EventKey.OnLoadDataCompleted item)
        {
            data = DataSceneManager.Instance.BeachVillaData.FloorPaintingDatas;
        }

        internal void Drawing(Sprite sprite)
        {
            if (sprite == null) return;

            Setup(sprite);
        }

        internal void Drawing(PaintingColorName color)
        {
            foreach (var item in data)
            {
                if (item.colorName == color)
                {
                    Setup(item.color);
                }
            }
        }
    }
}
