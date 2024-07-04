using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Peanut : BackItem
    {
        private int idItem;
        private Sprite flowerSprite;
        private bool isGrowth;
        private Tweener scaleTween;

        public int IdItem { get => idItem; }
        public bool IsGrowth { get => isGrowth; }

        protected override void InitItem()
        {
            canDrag = true;
            isComparePos = true;
            IsCarry = true;
            isScaleDown = true;
        }
        protected override void Start()
        {
            base.Start();

            if (GameManager.instance.curGround != null)
            {
                Ground = GameManager.instance.curGround.gameObject;
                Content = GameManager.instance.curFloorScroll.content.gameObject;
            }
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { peanut = this, backitem = this });
        }


        public void AssignItem(int id, Sprite peanutSprite, Sprite flowerSprite)
        {
            idItem = id;
            this.flowerSprite = flowerSprite;
            image.sprite = peanutSprite;
            image.SetNativeSize();
        }

        public void OnJUmpToPot(Vector3 _endPos, Transform _endParent, System.Action OnComplete = null)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();
            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(_endPos, 200, 1, 0.5f)
            .OnComplete(() =>
            {
                if (isGrowth) IsAssigned = false;
                OnComplete?.Invoke();
                //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
            });
        }

        public void OnGrowth(System.Action OnComplete = null)
        {
            isGrowth = true;

            image.sprite = flowerSprite;
            image.SetNativeSize();

            SoundManager.instance.PlayOtherSfx(SfxOtherType.RingLighting);

            KillScalling();
            transform.localScale = Vector3.zero;
            scaleTween = transform.DOScale(Vector3.one, 1).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canDrag = true;
                OnComplete?.Invoke();
            });
        }
    }
}