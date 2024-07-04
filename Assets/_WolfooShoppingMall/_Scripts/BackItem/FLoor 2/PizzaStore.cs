using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class PizzaStore : BackItem
    {
        [SerializeField] PizzaTopping pizzaToppingPb;
        [SerializeField] Pizza pizzaPb;
        [SerializeField] Transform[] spawnToppingZones;
        [SerializeField] Transform pizzaZone;
        [SerializeField] Transform boxZone;
        [SerializeField] Transform grillZone;

        private int curToppingIdx;
        private Vector3 startScaleBox;
        private Tweener scaleTween;

        protected override void InitItem()
        {
        }
        protected override void Start()
        {
            base.Start();

            startScaleBox = boxZone.transform.localScale;

        }
        protected override void InitData()
        {
            base.InitData();
            foreach (var zone in spawnToppingZones)
            {
                var item = Instantiate(pizzaToppingPb, zone);
                item.AssignItem(data.PizzaData.toppingSprites[curToppingIdx]);
                item.OnGeneration();
                curToppingIdx++;
                if (curToppingIdx >= data.PizzaData.toppingSprites.Length) curToppingIdx = 0;
            }
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.pizzaTopping != null)
            {
                if (delayTween != null) delayTween?.Kill();
                delayTween = DOVirtual.DelayedCall(0.2f, () =>
                {
                    foreach (var zone in spawnToppingZones)
                    {
                        if (zone.childCount > 0) continue;

                        var pizza = Instantiate(pizzaToppingPb, zone);
                        pizza.AssignItem(data.PizzaData.toppingSprites[curToppingIdx]);
                        pizza.OnGeneration();
                        curToppingIdx++;
                        if (curToppingIdx >= data.PizzaData.toppingSprites.Length) curToppingIdx = 0;
                    }
                });
            }

            if (item.pizza != null)
            {
                if (delayTween != null) delayTween?.Kill();
                delayTween = DOVirtual.DelayedCall(0.2f, () =>
                {
                    if (pizzaZone.childCount == 0)
                    {
                        var pizza = Instantiate(pizzaPb, pizzaZone);
                        pizza.OnGeneration();
                    }
                    if (boxZone.childCount == 0)
                    {
                        scaleTween = boxZone.DOScale(startScaleBox, 0.25f).SetEase(Ease.OutBack);
                    }
                });
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.pizzaTopping != null)
            {
            }

            if (item.pizza != null)
            {
                if (Vector2.Distance(item.pizza.transform.position, boxZone.position) <= 1f)
                {
                    item.pizza.OnPack(boxZone.position);

                    if (scaleTween != null) scaleTween?.Kill();
                    boxZone.transform.localScale = Vector3.zero;
                }

                if (Vector2.Distance(item.pizza.transform.position, grillZone.position) <= 1)
                {
                    item.pizza.OnGrilled(grillZone.position, data.PizzaData.pizzaSprites[0]);
                }

            }
        }
    }
}