using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class GiftDecal : BackItem
    {
        [SerializeField] bool isFlower;

        Sprite packageSprite;
        int idItem;
        private BackItem curItem;
        private ParticleSystem smokeFx;
        private Vector3 downPos;
        private GiftDecalData.TiedFlowerData[] packageSprites;

        public bool IsPackaged { private set; get; }
        public bool IsFlower { get => isFlower; }

        private void Awake()
        {
            image = GetComponent<Image>();
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void InitItem()
        {
            isComparePos = true;
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            downPos = transform.localPosition;
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            if (transform.localPosition != downPos) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnClickBackItem { decal = this });

            if (!canClick) return;

            canClick = false;
            canDrag = false;

            smokeFx.Play();

            image.enabled = false;
            curItem.GetImage().enabled = true;
            //  curItem.transform.localScale = startItemScale;
        }
        public void AssignItem(int _id, Sprite sprite, Sprite packageSprite, ParticleSystem smokePb)
        {
            idItem = _id;
            image.sprite = sprite;
            image.SetNativeSize();

            this.packageSprite = packageSprite;

            smokeFx = Instantiate(smokePb, transform);
            smokeFx.transform.localPosition = Vector3.zero;
        }
        public void AssignItem(int _id, Sprite sprite, GiftDecalData.TiedFlowerData[] packageSprite, ParticleSystem smokePb)
        {
            idItem = _id;
            image.sprite = sprite;
            image.SetNativeSize();

            packageSprites = packageSprite;

            smokeFx = Instantiate(smokePb, transform);
            smokeFx.transform.localPosition = Vector3.zero;
        }
        public void OnPackaging(BackItem _curItem, Transform _endParent, int idx = -1)
        {
            if (curItem != null) return;

            IsPackaged = true;
            curItem = _curItem;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Correct);

            transform.SetParent(_endParent);
            transform.position = curItem.transform.position;

            smokeFx.Play();

            image.sprite = idx == -1 ? packageSprite : packageSprites[idItem].tiedFlowerSprites[idx];
            image.SetNativeSize();

            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;

            curItem.OnPacking(transform);

            delayTween = DOVirtual.DelayedCall(1, () =>
            {
                startScale = transform.localScale * 0.8f;
                startScale =  Vector3.one * 0.8f;
                canClick = true;
                isScaleDown = true;
                isComparePos = true;
                canDrag = true;
            });
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { giftDecal = this, backitem = this });
        }
    }
}