using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class StickerBackItem : BackItem
    {
        [SerializeField] Sprite _candySprite;
        private int spawnIdx;
        private Tween _tween;

        public int SpawnIdx { get => spawnIdx; }

        protected override void InitData()
        {
            base.InitData();
            isScaleDown = true;
            isComparePos = true;
        }
        public void Setup(int spawnIdx,Sprite candySprite)
        {
            _candySprite = candySprite;
            this.spawnIdx = spawnIdx;
        }
        public void Spawn()
        {
            transform.localPosition = Vector3.zero;
            canDrag = false;
            tweenScale?.onKill();
            tweenScale = transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                canDrag = true;
            });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            if (!gameObject.activeSelf) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, sticker = this });
        }
        public void OnMakingCandy(Transform _endParent)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(_endParent);
            transform.localPosition = Vector3.zero;
            image.sprite = _candySprite;
            image.SetNativeSize();

            IsFood = true;
            AssignDrag();
        }
    }
}