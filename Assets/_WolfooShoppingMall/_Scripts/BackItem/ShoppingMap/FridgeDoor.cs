using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class FridgeDoor : BackItem
    {
        [SerializeField] DoorAnimation doorAnimation;
        [Range(0, 1)]
        [SerializeField] int status;
        private bool isOpen;

        public bool IsOpen { get => isOpen; }

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

            if (status == 1)
                doorAnimation.PlayExcute();
            else
                doorAnimation.PlayIdle();

            isOpen = status == 1;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;
            //Debug.Log("clicked");
            doorAnimation.SkeletonAnim.raycastTarget = false;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            status = 1 - status;
            if (status == 1)
                doorAnimation.PlayExcute();
            else
                doorAnimation.PlayIdle();
            isOpen = status == 1;

            EventDispatcher.Instance.Dispatch(new EventKey.OnClickBackItem { fridgeDoor = this });
        }
    }
}