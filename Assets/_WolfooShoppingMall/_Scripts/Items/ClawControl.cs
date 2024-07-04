using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ClawControl : MonoBehaviour
    {
        [SerializeField] Transform touchTrans;

        float xMoved;
        float yMoved;
        bool swipedLeft;
        Vector3 initTouch;
        private bool isLeft;
        private bool isDraged;
        private bool canDrag;

        private void OnMouseDown()
        {
            GameManager.instance.GetCurrentPosition(touchTrans);
            initTouch = touchTrans.position;
        }
        private void OnMouseUp()
        {
            isDraged = false;
            transform.rotation = Quaternion.Euler(Vector3.zero);
        }

        private void OnMouseDrag()
        {
            GameManager.instance.GetCurrentPosition(touchTrans);

            if (isDraged || canDrag)
                EventDispatcher.Instance.Dispatch(new EventKey.OnDragItem
                {
                    clawControl = this,
                    direction = isLeft ? Direction.Left : Direction.Right
                });
            OnSwipe();
        }
        public void DisableDrag()
        {
            canDrag = false;
        }
        public void EnableDrag()
        {
            canDrag = true;
        }


        void OnSwipe()
        {
            // Sound Drag Here
            xMoved = initTouch.x - touchTrans.position.x;
            yMoved = initTouch.y - touchTrans.position.y;
            swipedLeft = Mathf.Abs(xMoved) > Mathf.Abs(yMoved);

            initTouch = touchTrans.position;

            // Swipe Left
            if (swipedLeft && xMoved > 0)
            {
                isDraged = true;
                //    EventDispatcher.Instance.Dispatch(new EventKey.OnDragItem { clawControl = this, direction = Direction.Left });
                if (transform.rotation.eulerAngles.z < 180 && transform.rotation.eulerAngles.z >= 20) return;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + Vector3.forward);
                isLeft = true;
            }
            // Swipe Right
            else if (swipedLeft && xMoved < 0)
            {
                isDraged = true;
                //    EventDispatcher.Instance.Dispatch(new EventKey.OnDragItem { clawControl = this, direction = Direction.Right });
                if (transform.rotation.eulerAngles.z > 180 && transform.rotation.eulerAngles.z <= 340 && !isLeft) return;
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles - Vector3.forward);
                isLeft = false;
            }
        }
    }
}