using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Elevator : BackItem
    {
        [SerializeField] Transform itemZone;
        [SerializeField] ElevatorAnimation elevatorAnimation;
        [SerializeField] Button button;
        [SerializeField] Direction direction;
        [SerializeField] Button levelUpStairBtn;
        [SerializeField] Animator _animator;

        /// <summary>
        /// 1: isOn, 0: isOff
        /// </summary>
        [Range(0, 1)]
        [SerializeField] int status;
        private BackItem curItem;
        private Vector3 startItemScale;
        private Tweener scaleItemTween;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            status = 1;
            if (elevatorAnimation != null)
                elevatorAnimation.PlayOpenAnim(null, () =>
                {
                    itemZone.gameObject.SetActive(true);
                });
            if(_animator != null)
            {
                status = 0;
            }

        }
        protected override void Start()
        {
            base.Start();

            if (levelUpStairBtn != null)
            {
                levelUpStairBtn.onClick.AddListener(() => GUIManager.instance.OpenPanel(PanelType.Elevator));
                levelUpStairBtn.transform.DOPunchPosition(Vector3.up * 30, 1, 3).SetLoops(-1, LoopType.Restart);
            }

            if (button != null)
            {
                startItemScale = button.transform.localScale;
                scaleItemTween = button.transform.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.5f, 1).SetLoops(-1, LoopType.Restart);
                button.onClick.AddListener(() =>
                {
                    GUIManager.instance.OpenPanel(PanelType.Elevator);
                    if (scaleItemTween != null) scaleItemTween?.Kill();
                    button.transform.localScale = startItemScale;
                });
            }


            if(elevatorAnimation != null)
            {
                if (status == 0)
                {
                    elevatorAnimation.PlayCloseAnim(null, () =>
                    {
                        itemZone.gameObject.SetActive(false);
                    });
                }
                else
                {
                    elevatorAnimation.PlayOpenAnim(null, () =>
                    {
                        itemZone.gameObject.SetActive(true);
                    });
                }
            }
        }
        public void OnClosed()
        {
            status = 0;
            GUIManager.instance.OpenPanel(PanelType.Elevator);
        }
        public void OnOpened()
        {
            status = 1;
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (status == 0) return;

            if (item.backitem == null) return;

            curItem = item.backitem;

            if (curItem == null) return;
            if (Vector2.Distance(curItem.transform.position, itemZone.position) > 1) return;

            curItem.JumpToEndPos(itemZone.position, itemZone, null, assignPriorityy);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            status = 1 - status;
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Elevator);

            if(elevatorAnimation != null)
            {
                if (status == 0)
                {
                    canClick = false;
                    UISetupManager.Instance.maskBg.gameObject.SetActive(true);
                    elevatorAnimation.PlayCloseAnim(() =>
                    {
                        GUIManager.instance.OpenPanel(PanelType.Elevator);
                     //   EventDispatcher.Instance.Dispatch(new EventKey.OnSelect { elevator = this, direction = direction });
                        canClick = true;
                        UISetupManager.Instance.maskBg.gameObject.SetActive(false);
                    },
                    () =>
                    {
                        itemZone.gameObject.SetActive(false);
                    });
                }
                else
                {
                    elevatorAnimation.PlayOpenAnim(null, () =>
                    {
                        itemZone.gameObject.SetActive(true);
                    });
                }
            }
            else
            {
                itemZone.gameObject.SetActive(status == 1);
            }
        }

    }
}