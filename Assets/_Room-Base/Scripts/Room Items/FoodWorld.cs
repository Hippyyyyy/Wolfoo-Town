using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class FoodWorld : BackItemWorld, IFood
    {
        private Tweener _tweenScale;

        public override void Setup()
        {
            IsCarryItem = true;
            IsDragable = true;
            IsFood = true;
            IsCarryItem = true;
            IsStandingOnTable = true;
            base.Setup();
            name = "Food - " + Id;
        }
        protected override void OnKill()
        {
            base.OnKill();
            if (_tweenScale != null) _tweenScale?.Kill();
        }
        public void Feed()
        {
            _tweenScale = transform.DOScale(0.5f, 0.2f).OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }
    }
}
