using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CaptureCharacter : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Image image;
        private int curIdx;
        private Sprite[] curSprites;
        private int nextIdx;
        private Tweener moveTween;
        private Tweener rotateTWeen;
        private Vector3 startPos;
        private Quaternion startRotate;
        private bool canClick = true;
        private Tween delayTween;

        private void Start()
        {
            image.enabled = false;
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragItem>(GetEndDragItem);
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragItem>(GetEndDragItem);
        }
        private void GetEndDragItem(EventKey.OnEndDragItem obj)
        {
            if (obj.captureItem != null)
            {
                //if(Vector2.Distance(transform.position, obj.captureItem.transform.position) < 1)
                //{
                //    OnSpawn();
                //}
            }
        }

        public void InitItem()
        {
            startPos = transform.position;
            startRotate = transform.rotation;
        }

        public void AssignItem(int idx, Sprite[] sprite)
        {
            nextIdx = curIdx == idx ? nextIdx : 0;

            curIdx = idx;
            curSprites = sprite;
        }
        public void OnSpawn()
        {
            image.enabled = true;
            image.sprite = curSprites[nextIdx];
            image.SetNativeSize();

            nextIdx++;
            if (nextIdx == curSprites.Length) nextIdx = 0;

            if (moveTween != null) moveTween?.Kill();
            transform.position = startPos;
            moveTween = transform.DOPunchPosition(Vector3.up * 100, 0.6f, 1, 0.5f).SetEase(Ease.OutBounce);

            if (rotateTWeen != null) rotateTWeen?.Kill();
            transform.rotation = startRotate;
            rotateTWeen = transform.DORotate(Vector3.forward * 10, 0.2f).OnComplete(() =>
            {
                rotateTWeen = transform.DORotate(Vector3.forward * -10, 0.2f).OnComplete(() =>
                {
                    rotateTWeen = transform.DORotate(startRotate.eulerAngles, 0.2f).OnComplete(() =>
                    {
                    }).SetEase(Ease.OutBounce);
                });
            });
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (!canClick) return;

            delayTween = DOVirtual.DelayedCall(0.2f, () =>
            {
                canClick = true;
            });
            OnSpawn();
        }
    }
}