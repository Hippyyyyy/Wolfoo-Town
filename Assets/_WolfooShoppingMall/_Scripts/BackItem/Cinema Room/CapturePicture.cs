using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class CapturePicture : BackItem
    {
        private Tweener rotateTween;

        protected override void Start()
        {
            base.Start();
        }
        protected override void InitItem()
        {
            isScaleDown = true;
            isStandTable = true;
            IsCarry = true;

            scaleIndex = 1;
            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;
        }

        public void AssginItem(Sprite sprite)
        {
            image.sprite = sprite;
        }
        public void OnCaptured(Vector3 _endPos, Action OnComplete)
        {
            MoveToEndLocalPos(_endPos);
            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.forward * 180, 0.25f).OnComplete(() =>
            {
                rotateTween = transform.DORotate(Vector3.forward * 360, 0.15f).OnComplete(() =>
                {
                    rotateTween = transform.DORotate(Vector3.one * 30, 0.1f).OnComplete(() =>
                    {

                        AssignDrag();
                        OnComplete?.Invoke();
                    });
                });
            });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!canDrag) return;

            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.zero, 0.5f);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);

            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.one * 30, 0.5f);
        }
        public override void MoveToEndLocalPos(Vector3 _endPos, Action OnComplete = null, Ease jumpEase = Ease.Flash, bool isOffset = false, float time = 0.5F)
        {
            base.MoveToEndLocalPos(_endPos, OnComplete, jumpEase, isOffset, time);

            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.zero, 0.5f);
        }
    }
}