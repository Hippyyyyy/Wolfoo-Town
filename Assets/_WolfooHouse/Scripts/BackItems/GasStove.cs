using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class GasStove : BackItem
    {
        [SerializeField] Transform leftSeat;
        [SerializeField] Transform rightSeat;
        [SerializeField] ParticleSystem leftFx;
        [SerializeField] ParticleSystem rightFx;

        private Transform leftItem;
        private Transform rightItem;

        protected override void Start()
        {
            base.Start();

        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if(item.pan != null)
            {
                TurnOffGrillWith(item.pan);
            }
            if(item.food != null && item.food.IsGrill)
            {
                TurnOffGrillWith(item.food);
            }
        }
        private void TurnOffGrillWith(BackItem backitem)
        {
            if (leftItem == backitem.transform)
            {
                leftItem = null;
                leftFx.Stop();
            }
            if (rightItem == backitem.transform)
            {
                rightItem = null;
                rightFx.Stop();
            }
        }
        private void Grill(BackItem backitem)
        {
            if (leftItem == null)
            {
                var distance = Vector2.Distance(backitem.transform.position, leftSeat.position);
                if (distance < 1)
                {
                    backitem.transform.SetParent(transform);
                    backitem.JumpToEndLocalPos(leftSeat.localPosition);
                    leftItem = backitem.transform;
                    leftFx.Play();
                }
            }

            if (rightItem == null)
            {
                var distance = Vector2.Distance(backitem.transform.position, rightSeat.position);
                if (distance < 1)
                {
                    backitem.transform.SetParent(transform);
                    backitem.JumpToEndLocalPos(rightSeat.localPosition);
                    rightItem = backitem.transform;
                    rightFx.Play();
                }
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if(item.pan != null)
            {
                Grill(item.pan);
            }
            if(item.food != null && item.food.IsGrill)
            {
                Grill(item.food);
            }
        }
    }
}
