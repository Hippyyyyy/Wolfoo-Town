using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class News : BackItem
    {
        [SerializeField] Sprite foldSprite;
        [SerializeField] Sprite straightSprite;

        [SerializeField] bool isFolding;
        protected override void InitItem()
        {

        }
        protected override void InitData()
        {
            base.InitData();

            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
        }

        void SetState()
        {
            if(isFolding)
            {
                image.sprite = foldSprite;
                image.SetNativeSize();
            } else
            {
                image.sprite = straightSprite;
                image.SetNativeSize();
            }
        }
        public void Fold()
        {
            SoundManager.instance.PlayOtherSfx(myClip);
            isFolding = true;
            SetState();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, news = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            isFolding = false;
            SetState();
        }
    }
}