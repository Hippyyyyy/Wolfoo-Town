using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class SprinkleItem : ItemMove
    {
        [SerializeField] Vector2 velocityRange;
        private Tweener rotateTWeen;

        public bool IsAttch { get; private set; }
        public static Action<SprinkleItem> OnReleaseCompleted;

        private void Start()
        {
        }
        private void OnDestroy()
        {
            if (rotateTWeen != null) rotateTWeen?.Kill();
            if (moveTween != null) moveTween?.Kill();
        }
        public void AssignItem(bool isAttach, Sprite sprite, Vector3 _endPos)
        {
            IsAttch = isAttach;
            itemImg.sprite = sprite;
            endPos = _endPos;
        }

        public void RemoveRemaining()
        {
            var isDestroy = false;
            if (rotateTWeen != null && rotateTWeen.IsActive())
            {
                rotateTWeen?.Kill();
                isDestroy = true;
            }
            if (moveTween != null && moveTween.IsActive())
            {
                moveTween?.Kill();
                isDestroy = true;
            }
            if (isDestroy) Destroy(this.gameObject);
        }

        public void OnRelease(float delayTime, Transform _endParent)
        {
            float rdMove = UnityEngine.Random.Range(velocityRange.x, velocityRange.y);
            rotateTWeen = transform.DORotate(Vector3.forward * 180, 1 * 10)
                .SetEase(Ease.Linear)
                .SetSpeedBased(true)
                .SetLoops(-1, LoopType.Yoyo);

            if (moveTween != null) moveTween?.Kill();
            moveTween = transform.DOMove(endPos, rdMove)
                .SetSpeedBased(true)
                .SetDelay(delayTime)
                .SetEase(Ease.Linear)
                .OnStart(() =>
                {
                    transform.SetParent(_endParent);
                })
                .OnComplete(() =>
                {
                    if (rotateTWeen != null) rotateTWeen?.Kill();
                    if (!IsAttch) Destroy(gameObject);
                    else transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);

                    OnReleaseCompleted?.Invoke(this);
                });
        }
    }
}