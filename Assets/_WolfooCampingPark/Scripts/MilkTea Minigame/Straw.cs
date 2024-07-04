using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _WolfooShoppingMall.Minigame.MilkTea.MilkTeaGameManager;

namespace _WolfooShoppingMall.Minigame.MilkTea
{
    public class Straw : MonoBehaviour
    {
        [System.Serializable]
        public struct Limit
        {
            public float left;
            public float up;
            public float right;
            public float down;
        }

        [SerializeField] RoomFloorConfig floorConfig;
        [SerializeField] Limit myLimit;
        [SerializeField] Animator animator;
        [SerializeField] string suckName;
        [SerializeField] SpriteRenderer fakeBall;

        private bool isSucking;
        public Action<MilkTeaPearl> OnCollisionWithBall;
        private Action OnCompleted;
        private bool isDragging;
        private Tweener _tween;

        private void Start()
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, -5);
            PlayTutorial();
        }

        private void PlayTutorial()
        {
            _tween = transform.DOMove(transform.position - Vector3.one, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }
        private void StopTutorial()
        {
            _tween?.Kill();
        }

        public void Assign(Sprite ballSprite)
        {
            fakeBall.sprite = ballSprite;
        }

        public void SuckBall(Action OnCompleted)
        {
            isSucking = true;
            this.OnCompleted = OnCompleted;
            animator.Play(suckName, 0, 0);
        }
        public void OnAnimCompleted()
        {
            isSucking = false;
            OnCompleted?.Invoke();
        }

        private void OnTriggerEnterWith(GameObject obj)
        {
            if (obj.layer.Equals(floorConfig.OTHER_LAYER))
            {
                if (!isSucking && isDragging)
                {
                    var ball = obj.GetComponent<MilkTeaPearl>();
                    if (ball != null)
                    {
                        OnCollisionWithBall?.Invoke(ball);
                    }
                }
            }

            if (obj.layer.Equals(floorConfig.OBSTACLE_LAYER))
            {
            }
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            OnTriggerEnterWith(collision.gameObject);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            OnTriggerExitWith(collision.gameObject);
        }

        private void OnTriggerExitWith(GameObject obj)
        {
            if (obj.layer.Equals(floorConfig.OBSTACLE_LAYER))
            {
            }
        }

        private void OnMouseDown()
        {
            isDragging = true;
            StopTutorial();
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }
        private void OnMouseDrag()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePos.y >= myLimit.up) mousePos = new Vector3(mousePos.x, myLimit.up, 0);
            if (mousePos.y <= myLimit.down) mousePos = new Vector3(mousePos.x, myLimit.down, 0);
            if (mousePos.x >= myLimit.right) mousePos = new Vector3(myLimit.right, mousePos.y, 0);
            if (mousePos.x <= myLimit.left) mousePos = new Vector3(myLimit.left, mousePos.y, 0);

            mousePos.z = -5;
            transform.position = mousePos;
        }
        private void OnMouseUp()
        {
            isDragging = false;
        }
    }
}
