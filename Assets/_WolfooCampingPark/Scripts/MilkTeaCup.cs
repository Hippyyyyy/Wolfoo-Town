using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _WolfooShoppingMall.CampingParkDataSO;

namespace _WolfooShoppingMall
{
    public class MilkTeaCup : BeverageWorld
    {
        public void Assign(MilkTeaData data)
        {
            water.sprite = data.waterSprite;
        }
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            MilkTeaMachine.OnVerified += OnItemVerified;
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            MilkTeaMachine.OnVerified -= OnItemVerified;
        }
        private void OnItemVerified(BackItemWorld obj, MilkTeaMachine teaMachine)
        {
            if (obj.Id == Id)
            {
                PlayWithBox(true);
                transform.SetParent(teaMachine.CupHolder);
            }
        }
        protected override void OnBeginDrag()
        {
            PlayWithBox(false);
            base.OnBeginDrag();
        }
    }
}
