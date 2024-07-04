using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class PlassticCup : BackItem
    {
        [SerializeField] Image beverageImg;
        [SerializeField] bool isOverturned;
        [SerializeField] Image maskLid;
        [SerializeField] Transform lidZone;
        [SerializeField] Transform strawZone;

        public bool IsHasWater { get; private set; }
        public bool IsOverturned { get => isOverturned; }

        private BeverageLid curBeverageLid;

        public bool IsHasLid { get; private set; }

        private Tweener fillTween;
        private Tweener rotateTween;
        private bool isHasStraw;

        protected override void Start()
        {
            base.Start();

            beverageImg.type = Image.Type.Filled;
            beverageImg.fillMethod = Image.FillMethod.Vertical;
            beverageImg.fillOrigin = (int)Image.OriginVertical.Bottom;
            beverageImg.fillAmount = 0;

            maskLid.gameObject.SetActive(false);
            maskLid.raycastTarget = false;

        }
        private void OnDestroy()
        {
            if (fillTween != null) fillTween?.Kill();
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { plasticup = this, backitem = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            isOverturned = false;
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);

            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.forward * 0, 0.25f);
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.beverageLid != null)
            {
                if (item.beverageLid == curBeverageLid)
                {
                    maskLid.gameObject.SetActive(false);
                    curBeverageLid = null;
                }
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.straw != null)
            {
                if (IsOverturned) return;
                if (Vector2.Distance(item.straw.transform.position, transform.position) > 1) return;
                item.straw.OnPlugin(strawZone);
                isHasStraw = true;
            }
            if (item.beverageLid != null)
            {
                if (IsOverturned) return;
                if (curBeverageLid != null) return;
                if (Vector2.Distance(item.beverageLid.transform.position, transform.position) > 1) return;
                item.beverageLid.OnPlugin(lidZone, () =>
                {
                    maskLid.gameObject.SetActive(true);
                });
                curBeverageLid = item.beverageLid;
            }

            if(item.waterBottle != null)
            {
                if (Vector2.Distance(item.waterBottle.transform.position, transform.position) > 1) return;
                item.waterBottle.PourWater(transform, () =>
                {
                    IsHasWater = true;
                    fillTween?.Kill();
                    fillTween = beverageImg.DOFillAmount(1, 2);
                    maskLid.gameObject.SetActive(true);
                });
            }
        }

        public void OnPour(Transform area)
        {
            if (!IsHasWater) return;

            IsAssigned = true;
            KillDragging();
            fillTween?.Kill();
            fillTween = beverageImg.DOFillAmount(0, 1).OnComplete(() =>
            {
                MoveToGround(true);
            });
        }

        public void OnRotateDownTo(Vector3 _endPos, Transform _endParent)
        {
            if (isHasStraw) return;
            if (IsHasWater) return;

            isOverturned = true;
            if (rotateTween != null) rotateTween?.Kill();
            rotateTween = transform.DORotate(Vector3.forward * 180, 0.25f);
            transform.SetParent(_endParent);
            JumpToEndLocalPos(_endPos, null, Ease.Flash);
        }

        public void OnGetBeverage(float totalTime, Vector3 _endPos, int idxVerified, Transform _endParent)
        {
            if (IsHasWater) return;
            IsHasWater = true;
            canDrag = false;
            IsBeverage = true;

            //   transform.SetParent(_endParent);
            JumpToEndPos(_endPos, transform.parent, () =>
            {
                beverageImg.sprite = data.plasticCupData.beverageColorSprites[idxVerified];
                beverageImg.SetNativeSize();
                beverageImg.fillAmount = 0;
                fillTween = beverageImg.DOFillAmount(1, totalTime).OnComplete(() =>
                {
                    AssignDrag();
                });
            });
        }
        public void OnPourWater(float totalTime)
        {
            IsHasWater = false;
            fillTween = beverageImg.DOFillAmount(0, totalTime);
        }

        protected override void InitItem()
        {
            isScaleDown = true;
            canDrag = true;
            isComparePos = true;
            IsCarry = true;
            scaleIndex = 0.6f;
        }
    }
}