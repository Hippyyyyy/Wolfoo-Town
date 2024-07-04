using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class HotDogCar : BackItem
    {
        [SerializeField] Transform boxArea;
        [SerializeField] Transform grillArea;
        [SerializeField] Food grillFoodPb;
        [SerializeField] Food cakePb;

        protected override void InitData()
        {
            base.InitData();
            //for (int i = 0; i < boxArea.childCount; i++)
            //{
            //    var cake = Instantiate(cakePb, boxArea.GetChild(i));
            //    cake.Setup(i);
            //}
            //for (int i = 0; i < grillArea.childCount; i++)
            //{
            //    var food = Instantiate(grillFoodPb, grillArea.GetChild(i));
            //    food.Setup(i);
            //}
        }

        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.food != null)
            {

            }
        }
    }
}