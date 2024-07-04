using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class MoneyPolime : BackItem
    {
        private Tweener rotateTween;

        protected override void InitItem()
        {
            IsCarry = true;
            isScaleDown = true;
            scaleIndex = 1f;
        }
        protected override void Start()
        {
            base.Start();

            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;
        }
        private void OnDestroy()
        {
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, money = this });
        }

        public void OnPrinted(Sprite spriteItem, Vector3 _endPos, Vector3 _endPos2)
        {
            image.sprite = spriteItem;
            image.SetNativeSize();

            MoveToEndLocalPos(_endPos, () =>
            {
                if (rotateTween != null) rotateTween?.Kill();
                rotateTween = transform.DORotate(Vector3.zero, 0.5f);

                MoveToEndLocalPos(_endPos2, () =>
                {
                    AssignDrag();
                    transform.SetParent(Content.transform);
                }, Ease.OutBounce, false, 1);
            },
            Ease.Flash, false, 1);
        }
    }
}