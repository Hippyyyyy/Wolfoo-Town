using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _WolfooShoppingMall.CampingParkDataSO;

namespace _WolfooShoppingMall
{
    public class BeverageWorld : BackItemWorld
    {
        [SerializeField] protected SpriteRenderer water;
        [SerializeField] bool hasWater;
        
        public bool HasWater { get => hasWater; private set => hasWater = value; }

        private Tweener _tweenWater;

        public override void Setup()
        {
            IsDragable = true;
            IsFood = true;
            IsCarryItem = true;
            base.Setup();
        }
        protected override void OnKill()
        {
            base.OnKill();
            if (_tweenWater != null) _tweenWater?.Kill();
        }

        public void Release(Vector2 endPos, System.Action OnCompleted)
        {
            if (!HasWater) return;
            HasWater = false;
            transform.position = endPos;
            StopMovingToGround();

            _tweenWater?.Kill();
            _tweenWater = water.transform.DOScale(Vector2.zero, 0.25f).OnComplete(() =>
            {
                OnCompleted?.Invoke();
                PlayMovingToGround(true);
            });
        }
    }
}
