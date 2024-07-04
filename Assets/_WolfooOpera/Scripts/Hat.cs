using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Hat : BackItem
    {
        [SerializeField] bool canHanging;
        protected Vector3 wearingLocalPos;

        public bool IsNormalHat { get; protected set; }

        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            isScaleDown = true;
            IsNormalHat = true;
        }

        public void Setup(Sprite sprite, Vector3 wearingPos)
        {
            if (image == null) image = GetComponent<Image>();
            image.sprite = sprite;
            image.SetNativeSize();

            wearingLocalPos = wearingPos;

            if(canHanging)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            startScale = Vector3.one;
        }
        public void Hanging(Transform _endParent)
        {
            IsAssigned = true;
            KillDragging();

            transform.SetParent(_endParent);
            transform.localPosition = Vector3.zero;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        public void Swear(Transform _parent, bool isFlip, bool isWolfoo=true)
        {
            isScaleDown = false;
            IsAssigned = true;
            KillDragging();
            KillScalling();
            transform.SetParent(_parent);
            transform.localPosition = wearingLocalPos;
            transform.localRotation = Quaternion.Euler(isFlip ? Vector3.up * 180 : Vector3.zero);
            if (isWolfoo)
                transform.localScale = Vector3.one * 1.3f;
            else 
                transform.localScale = Vector3.one * 3.3f;
            isScaleDown = true;
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;
            if(canHanging)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { backitem = this, hat = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, hat = this });
        }
    }
}