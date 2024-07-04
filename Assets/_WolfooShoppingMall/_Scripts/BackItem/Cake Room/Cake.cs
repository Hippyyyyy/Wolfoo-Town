using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Cake : BackItem
    {
        [SerializeField] bool isAssign = true;
        protected override void InitItem()
        {
            if (!isAssign) return;

            isComparePos = true;
            isScaleDown = true;
            canDrag = true;
            IsFood = true;
            IsCarry = true;
        }
        protected override void Start()
        {
            base.Start();
            gameObject.name = name.ToString();
        }
        public void Assign()
        {
            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;

            startScale = transform.localScale;
        }
        public void AssignItem(Sprite sprite)
        {
            image.sprite = sprite;
            Assign();
        }

        public void OnBaking()
        {

        }

        public void OnMaking(Vector3 _endPos, System.Action OnComplete = null)
        {
            MoveToEndLocalPos(_endPos, () =>
            {
                isComparePos = true;
                isScaleDown = true;
                canDrag = true;
                IsFood = true;

                OnComplete?.Invoke();
            }, Ease.Linear, false, 2);
        }
        public void JumpToStove(Transform _parent)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(_parent);
            tweenJump = transform.DOLocalJump(Vector3.zero, 100, 1, 0.5f)
            .OnComplete(() =>
            {
                //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
            });
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { cake = this, backitem = this });
        }
    }
}