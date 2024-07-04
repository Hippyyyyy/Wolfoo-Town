using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static _WolfooShoppingMall.SelfHouseDataSO;

namespace _WolfooShoppingMall
{
    public class WallWorld : PaintingObject
    {
        [SerializeField] SeperatorData[] seperators;
        private PaintingData[] data;

        [System.Serializable]
        public struct SeperatorData
        {
            public SpriteRenderer spriteRenderer;
            public PaintingColorName color;
            public Sprite sprite;
        }

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
            data = DataSceneManager.Instance.BeachVillaData.WallPaintingDatas;
        }

        internal void Drawing(Sprite sprite)
        {
            if (sprite == null) return;

            Setup(sprite);
            foreach (var seperator in seperators)
            {
                seperator.spriteRenderer.sprite = sprite;
            }
        }

        internal void Drawing(PaintingColorName color)
        {
            foreach (var item in data)
            {
                if (item.colorName == color)
                {
                    Setup(item.color);
                    foreach (var seperator in seperators)
                    {
                        if (seperator.color == color)
                            seperator.spriteRenderer.sprite = seperator.sprite;
                    }
                }
            }
        }
    }
}
