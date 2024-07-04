using _WolfooShoppingMall.Minigame.DrawingPicture;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class DrawingPictureHolder : BackItemWorld
    {
        [SerializeField] CampingParkDrawingPicture pictureModePb;
        [SerializeField] PictureWorld picturePb;
        [SerializeField] SpriteRenderer borderSprite;
        private Tweener _fadeTween;

        public override void Setup()
        {
            IsClick = true;
            IsDragable = true;
            base.Setup();

            PlayBorderAnim();
        }

        private void PlayBorderAnim()
        {
            _fadeTween = borderSprite.DOFade(0, 1).SetLoops(-1, LoopType.Yoyo);
        }

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            this.OnChangeSorting += OnSortingLayer;
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            this.OnChangeSorting -= OnSortingLayer;
        }
        protected override void OnKill()
        {
            base.OnKill();
            if (_fadeTween != null) _fadeTween?.Kill();
        }

        private void OnSortingLayer(int obj)
        {
            borderSprite.sortingOrder = obj - 1;
        }

        protected override void OnClick()
        {
            base.OnClick();
            OnCreateMinigame();
        }

        private void OnCreateMinigame()
        {
            var minigame = Instantiate(pictureModePb);
            minigame.OnCapturing = OnCompleteMinigame;
        }

        private void OnCompleteMinigame(Sprite sprite)
        {
            var picture = Instantiate(picturePb, transform);
            picture.Setup();
            picture.Assign(sprite);
        }
    }
}
