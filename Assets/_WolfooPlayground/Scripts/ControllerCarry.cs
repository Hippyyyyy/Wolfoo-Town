using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ControllerCarry : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [SerializeField] float limitRotateValue = 20;
        [SerializeField] float verifiedDistanceValue;
        [SerializeField] float velocity;
        private Vector3 beginTouchPosition;
        private Vector3 curPostion;

        public System.Action OnPlayRight;
        public System.Action OnPlayleft;
        private bool isPlayed;

        public void OnDrag(PointerEventData eventData)
        {
            curPostion = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(curPostion, transform.position) > 1.5f) return;

            // Move Right
            OnMoveRight();
            // Move Left
            OnMoveLeft();
        }
        void OnMoveRight()
        {
            if (curPostion.x - beginTouchPosition.x >= verifiedDistanceValue)
            {
                beginTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (transform.localRotation.z < -limitRotateValue)
                {
                    transform.localRotation = Quaternion.Euler(Vector3.forward * -limitRotateValue);
                    return;
                }
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles - Vector3.forward * velocity);

                if (isPlayed) return;
                isPlayed = true;
                OnPlayRight?.Invoke();
            }
        }
        void OnMoveLeft()
        {
            if (curPostion.x - beginTouchPosition.x <= -verifiedDistanceValue)
            {
                beginTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (transform.localRotation.z > limitRotateValue)
                {
                    transform.localRotation = Quaternion.Euler(Vector3.forward * limitRotateValue);
                    return;
                }
                transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles + Vector3.forward * velocity);

                if (isPlayed) return;
                isPlayed = true;
                OnPlayleft?.Invoke();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            beginTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPlayed = false;
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}