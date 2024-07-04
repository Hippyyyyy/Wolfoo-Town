using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class TeaofCupWorld : BackItemWorld
    {
        [SerializeField] SpriteRenderer waterSprite;
        [SerializeField] int totalTimePouringWater;

        private int waterCount;
        private bool isCounting;

        public bool HasWater => waterSprite != null && waterSprite.gameObject.activeSelf;


        public override void Setup()
        {
            IsDragable = true;
            IsCarryItem = true;
            IsStandingOnTable = true;
            base.Setup();

            if (waterSprite != null)
            {
                waterSprite.gameObject.SetActive(false);
            }
        }
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            EventRoomBase.OnDragBackItem += GetDragBackItem;
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            EventRoomBase.OnDragBackItem -= GetDragBackItem;
        }

        private void GetDragBackItem(BackItemWorld obj)
        {
            var teapot = obj.GetComponent<TeaPotWorld>();
            if(teapot)
            {
                var distance = Vector2.Distance(transform.position, teapot.PouringPos);
                if(distance < 0.5f)
                {
                    OnPouringWater();
                }
                else
                {
                    waterCount = 0;
                }
            }
        }
        private void Counting()
        {
            if (isCounting) return;
            if (waterCount > totalTimePouringWater) return;

            waterCount++;
            if (waterCount == totalTimePouringWater)
            {
                waterSprite.gameObject.SetActive(true);
                isCounting = false;
            }
        }

        private void OnPouringWater()
        {
            if (HasWater) return;
            Counting();
        }
    }
}
