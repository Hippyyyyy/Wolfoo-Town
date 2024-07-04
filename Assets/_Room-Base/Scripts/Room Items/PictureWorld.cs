using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class PictureWorld : BackItemWorld
    {
        private Sequence _tweenHanging;
        private CustomRoomItem customItem;
        private int triggerCount;

        public bool IsTriggerEnterWithWall { get => (triggerCount > 0); }

        public override void KillDragging()
        {
            base.KillDragging();
            if (_tweenHanging != null) _tweenHanging?.Kill();
        }
        public override void Setup()
        {
            IsDragable = true;
            IsCarryItem = true;
            base.Setup();
            customItem = GetComponent<CustomRoomItem>();

            if (IsDragable) transform.position = new Vector3(transform.position.x, transform.position.y, -0.01f);
        }
        public void ResetCollider()
        {
            var boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.offset = Vector2.zero;
        }

        public void Assign(Sprite sprite)
        {
            MySprite.sprite = sprite;

            Vector2 S = MySprite.sprite.bounds.size;
            GetComponent<BoxCollider2D>().size = S;
            GetComponent<BoxCollider2D>().offset = new Vector2(0, 0);
            transform.SetParent(_myMap.ItemContent);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($" PictureWorld {name} is Trigger Enter with: {collision.gameObject.name}");

            var wall = collision.GetComponent<WallWorld>();
            if (wall != null)
            {
                triggerCount++;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log($" PictureWorld {name} is Trigger Exit with: {collision.gameObject.name}");

            var wall = collision.GetComponent<WallWorld>();
            if (wall != null)
            {
                triggerCount--;
            }
        }

        public void HangOn(Vector3 _endPos, System.Action OnCompleted)
        {
            KillDragging();
            _tweenHanging = transform.DOJump(_endPos, 0.5f, 1, 0.25f).SetEase(Ease.Flash).OnComplete(() =>
            {
                OnCompleted?.Invoke();
            });
        }
        protected override void OnEndDrag()
        {
            base.OnEndDrag();
            if (customItem != null)
            {
                if (IsTriggerEnterWithWall)
                {
                    if (customItem.IsInDrangerZone) return;
                    StopMovingToGround();
                    OrderLayerIndex();
                    HangOn(transform.position, null);
                }
            }
            else
            {
                if (IsTriggerEnterWithWall)
                {
                    StopMovingToGround();
                    OrderLayerIndex();
                    HangOn(transform.position, null);
                }
            }
        }
    }
}
