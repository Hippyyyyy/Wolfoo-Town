using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Picture : BackItem
    {
        [SerializeField] bool isAssgin = true;
        private Vector2 startSizeIMg;

        protected override void InitItem()
        {
            if (!isAssgin) return;

            isComparePos = true;
            isScaleDown = true;
            canDrag = true;
            scaleIndex = 0.6f;
        }
        protected override void Start()
        {
            base.Start();
        }
        public void AssignItem()
        {
            isComparePos = true;
            isScaleDown = true;
            canDrag = true;
            scaleIndex = 0.6f;
        }
        public void AssignItem(Sprite sprite)
        {
            startSizeIMg = image.rectTransform.sizeDelta;

            image.sprite = sprite;
            image.SetNativeSize();
            GameManager.instance.ScaleImage(image, startSizeIMg.x, startSizeIMg.y);
            gameObject.SetActive(true);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
        }
    }
}