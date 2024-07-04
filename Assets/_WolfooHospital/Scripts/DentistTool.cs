using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class DentistTool : BackItem
    {
        [SerializeField] Animator animator;
        [SerializeField] string animName;
        [SerializeField] HospitalAnimAction animAction;
        Vector2 initPos;
        Transform initParent;
        private Transform awakeParent;

        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            initPos = transform.position;
            initParent = transform.parent;
            isComparePos = true;
            isScaleDown = true;

            awakeParent = transform.parent;
            animAction.OnCompleted = OnActionCompleted;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            if (Vector2.Distance(awakeParent.position, transform.position) < 1)
            {
                transform.SetParent(awakeParent);
                JumpToEndLocalPos(startLocalPos, null, DG.Tweening.Ease.OutBounce, 50, false, assignPriorityy);
            }
            else
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, dentisTool = this });
            }
        }

        public void Action(Vector2 _endPos)
        {
            canDrag = false;
            IsAssigned = true;
            KillDragging();

            transform.position = _endPos;
            animator.enabled = true;
            animator.Play(animName, 0, 0);
            SoundManager.instance.PlayOtherSfx(myClip);
        }

        void OnActionCompleted()
        {
            animator.enabled = false;
            canDrag = true;

            IsAssigned = false;
            transform.SetParent(initParent);
            JumpToEndLocalPos(startLocalPos);
            //JumpToEndPos(startPos, () =>
            //{
            //    transform.SetParent(initParent);
            //    transform.localPosition = startLocalPos;
            //});
        }
    }
}