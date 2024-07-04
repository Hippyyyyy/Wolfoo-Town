using DG.Tweening;
using SCN;
using SCN.Common;
using SCN.UIExtend;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class OperaClothesScrollItem : ScrollItemBase
    {
        [SerializeField] Image itemImg;
        [SerializeField] Clothing clothing;

        private Vector3 startScale;
        private Transform _packageArea;
        private Tweener scaleTween;
        private bool isInsidePackeEdge;
        private bool canDrag;
        private Edge[] packageEdges;
        private Tween _tween;

        public static Action<Transform> OnSetClothingToPackage;
        private OperaClothesPulledPackage _myPackage;

        public int TopicIdx { get; private set; }
        public int Id { get; private set; }

        protected override void Setup(int order)
        {
            Id = order;
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerUp, OnPointerUp);
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerDown, OnPointerDown);

            canDrag = true;

            EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem { operaClothesScrollItem = this });
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }

        private void GetEndDragBackItem(EventKey.OnEndDragBackItem obj)
        {
            if (obj.clothing != null)
            {
                if (obj.clothing != clothing) return;
                if (obj.clothing.Package != _myPackage) return;
                isInsidePackeEdge = GameManager.instance.Is_inside(obj.clothing.transform.position, packageEdges);
                //    if (Vector2.Distance(obj.clothing.transform.position, transform.position) < 1)
                if (isInsidePackeEdge && Vector2.Distance(_packageArea.position, obj.clothing.transform.position) < 3)
                {
                    OnSetClothingToPackage?.Invoke(transform);
                    SetInsideToPackage();
                }
            }
        }

        public void Setup(int id, Sprite frontSprite, Sprite behindSprite, Sprite foldingSprite, Transform packageArea, OperaClothesPulledPackage myPackage)
        {
            if (itemImg != null)
            {
                itemImg.sprite = frontSprite;
                itemImg.SetNativeSize();
            }

            _myPackage = myPackage;
            clothing.AssignItem(id, frontSprite, behindSprite, foldingSprite);
            clothing.AssignPackage(myPackage);
            clothing.gameObject.SetActive(false);

            transform.localScale = Vector3.one * 0.7f;
            startScale = transform.localScale;

            _packageArea = packageArea;
            Transform[] area = new Transform[packageArea.childCount];
            for (int i = 0; i < packageArea.childCount; i++)
            {
                area[i] = packageArea.GetChild(i).transform;
            }
            packageEdges = GameManager.instance.GetEdges(area);
        }
        public void SetInsideToPackage()
        {
            itemImg.enabled = true;

            clothing.AssignToCharacter(transform);
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(0.1f, () =>
            {
                clothing.gameObject.SetActive(false);
                _tween = transform.DOLocalRotate(Vector3.forward * 5, 0.15f).OnComplete(() =>
                {
                    _tween = transform.DOLocalRotate(Vector3.forward * -5, 0.3f).OnComplete(() =>
                    {
                        _tween = transform.DOLocalRotate(Vector3.zero, 0.15f);
                    });
                });
            });
        }
        public void SetOutsideToPackage()
        {
            itemImg.enabled = false;

            clothing.gameObject.SetActive(true);
            clothing.MoveToGround(true);
        }

        private void OnPointerDown(BaseEventData arg0)
        {
            if (!canDrag) return;

            clothing.AssignDrag();
        }

        private void OnPointerUp(BaseEventData eventData)
        {
            if (!canDrag) return;
            isInsidePackeEdge = GameManager.instance.Is_inside(transform.position, packageEdges);
            if (isInsidePackeEdge)
            {
                SetInsideToPackage();
            }
            else
            {
                SetOutsideToPackage();
            }
        }

        protected override void OnStartDragOut()
        {
            base.OnStartDragOut();

            if (!canDrag) return;
            _tween?.Kill();
            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }
}