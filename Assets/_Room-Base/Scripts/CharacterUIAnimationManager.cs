using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterUIAnimationManager : MonoBehaviour
    {
        [SerializeField] Animator animator;
        [SerializeField] private float curSpeed;
        [SerializeField] private float maxSpeed;
        [SerializeField] private float minSpeed;
        [SerializeField] private float accelPercentage;

        private Vector3 startPos;
        private bool isLeft;
        private bool isRight;
        private bool leftDragging;
        private bool rightDragging;
        private bool isDragging;
        private Vector3 lastPos;

        public bool Enable
        {
            set
            {
                animator.enabled = value;
            }
        }

        public void PlayEating()
        {
            animator.Play("CharEating", 0, 0);
        }
        public void PlayIdle()
        {
            animator.Play("Idle", 0, 0);
        }
        public void PlayDrag()
        {
            // Detect change Direction
            if (leftDragging != isLeft && isDragging)
            {
                leftDragging = isLeft;
            }
            else if (rightDragging != isRight && isDragging)
            {
                rightDragging = isRight;
                startPos = transform.position;
            }

            // Detect Movement
            if (Vector2.Distance(transform.position, startPos) <= 0.5f)
            {
                isDragging = false;
            }
            else
            {
                isDragging = true;
            }
            isLeft = transform.position.x - lastPos.x < 0;
            isRight = transform.position.x - lastPos.x > 0;
            lastPos = transform.position;

            // Detect on Dragging
            if (isDragging)
            {
                if (!isLeft)
                {
                    if (curSpeed < maxSpeed)//Stop once we reach max speed.
                        curSpeed += maxSpeed * accelPercentage * Time.deltaTime;
                    else
                        curSpeed = maxSpeed;
                }
                else
                {
                    if (curSpeed > minSpeed)
                        curSpeed += minSpeed * accelPercentage * Time.deltaTime;
                    else
                        curSpeed = minSpeed;
                }
            }
            else
            {
                // Detect on EndDrag
                if (curSpeed != 0)
                    curSpeed += (0 - curSpeed) * accelPercentage * Time.deltaTime;
            }

            SetFloatAnim(curSpeed);
        }
        public void StopDrag()
        {
            PlayIdle();
        }
        public void InitDrag()
        {
            startPos = transform.position;
            leftDragging = isLeft;
            rightDragging = isRight;
            animator.Play("DragAnim");
            SetFloatAnim((maxSpeed + minSpeed) / 2);
        }
        private void SetFloatAnim(float magnitude)
        {
            animator.SetFloat("Drag", magnitude);
        }
    }
}
