using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class FilmTicket : BackItem
    {
        [SerializeField] int assignIdx;
        private Tweener rotateTween;

        public int AssignIdx { get => assignIdx; }

        protected override void InitItem()
        {
            IsCarry = true;
            isScaleDown = true;
            scaleIndex = 1f;
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

            if (GameManager.instance.curGround == null) return;
            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;
        }
        public void Init()
        {
            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;
        }
        private void OnDestroy()
        {
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnClickBackItem { ticket = this });
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
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
        public void OnPrinted()
        {
            AssignDrag();
            transform.SetParent(Content.transform);
        }
    }
}