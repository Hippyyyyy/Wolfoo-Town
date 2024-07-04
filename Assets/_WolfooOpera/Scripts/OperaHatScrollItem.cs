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
    public class OperaHatScrollItem : ScrollItemBase
    {
        [SerializeField] Image itemImg;
        [SerializeField] OperaHat hat;

        private Vector3 startScale;
        private Transform _packageArea;
        private Tweener scaleTween;
        private bool isInsidePackeEdge;
        private bool canDrag;
        private Edge[] packageEdges;
        private Tween _tween;

        public static Action<Transform> OnSetHatToPackage;
        private OperaHatPulledPackage _myHatPackage;

        public int TopicIdx { get; private set; }
        public int Id { get; private set; }

        protected override void Setup(int order)
        {
            Id = order;
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerUp, OnPointerUp);
            Master.AddEventTriggerListener(EventTrigger, EventTriggerType.PointerDown, OnPointerDown);

            canDrag = true;

            EventDispatcher.Instance.Dispatch(new EventKey.OnInitItem { operaHatScrollItem = this });
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }

        private void GetEndDragBackItem(EventKey.OnEndDragBackItem obj)
        {
            if(obj.operaHat != null)
            {
                if (obj.operaHat != hat) return;
                if (obj.operaHat.Package != _myHatPackage) return;
                isInsidePackeEdge = GameManager.instance.Is_inside(obj.operaHat.transform.position, packageEdges);
                //    if (Vector2.Distance(obj.clothing.transform.position, transform.position) < 1)
                if (isInsidePackeEdge && Vector2.Distance(_packageArea.position, obj.operaHat.transform.position) < 3)
                {
                    OnSetHatToPackage?.Invoke(transform);
                    SetInsideToPackage();
                }
            }
        }

        public void Setup(int id, Sprite skinSprite, Vector3 wearingLocalPos, Transform packageArea, OperaHatPulledPackage myPackage)
        {
            if (itemImg != null)
            {
                itemImg.sprite = skinSprite;
                itemImg.SetNativeSize();
            }

            _myHatPackage = myPackage;

            hat.Setup(skinSprite, wearingLocalPos);
            hat.AssignItem(myPackage);
            hat.gameObject.SetActive(false);

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
            hat.AssignToCharacter(transform);
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(0.1f, () =>
            {
                hat.gameObject.SetActive(false);
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
            hat.gameObject.SetActive(true);
            hat.MoveToGround(true);
        }

        private void OnPointerDown(BaseEventData arg0)
        {
            if (!canDrag) return;

            hat.AssignDrag();
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