using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class WaterBottle : BackItem
    {
        [SerializeField] bool canPouring;
        [SerializeField] ParticleSystem waterFx;
        [SerializeField] float moveSpeed;
        [SerializeField] float rotationModifier;
        [SerializeField] float speed;

        private bool isPouring;
        private Transform _endTarget;
        private float distance;
        private float remainingDistance;
        private Tween _tweenDelay;

        private Action OnPouring;
        private Vector3 beginDragPos;

        protected override void InitItem()
        {
            IsBeverage = !canPouring;
            canDrag = true;
            isComparePos = true;
            isStandTable = true;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            isPouring = false;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, waterBottle = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;

            beginDragPos = transform.position;
            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { backitem = this });
        }

        private void Update()
        {
            if (!canPouring) return;
            if (isPouring && remainingDistance > 0)
            {
                transform.position = Vector3.Lerp(transform.position, _endTarget.position + new Vector3(1, 2.5f), 1 - (remainingDistance / distance));
                remainingDistance -= moveSpeed * Time.deltaTime;

                Vector3 vectorToTarget = transform.position - _endTarget.position - Vector3.right;
                float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg - rotationModifier;
                Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
                transform.rotation = Quaternion.Slerp(transform.rotation, q, Time.deltaTime * speed);
            }

            if(isPouring && remainingDistance <= 1f)
            {
                isPouring = false;
                OnMoveCompleted();
            }
        }

        private void OnPoured()
        {
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            MoveToGround(true);
        }
        private void OnMoveCompleted()
        {
            waterFx.Play();
            OnPouring?.Invoke();
            _tweenDelay?.Kill();
            _tweenDelay = DOVirtual.DelayedCall(waterFx.main.duration + 0.5f, () =>
            {
                waterFx.Stop();
                OnPoured();
                OnPouring = null;
            });
        }

        public void PourWater(Transform endTarget, System.Action OnPouring)
        {
            if (!canPouring) return;

            this.OnPouring = OnPouring;

            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            _endTarget = endTarget;
            distance = Vector2.Distance(transform.position, _endTarget.position + new Vector3(1, 2.5f));
            remainingDistance = distance;
            isPouring = true;
        }
    }

}