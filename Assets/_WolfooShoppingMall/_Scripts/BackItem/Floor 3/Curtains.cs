using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Curtains : BackItem
    {
        [SerializeField] Transform itemZone;
        private DoorData doorData;
        /// <summary>
        /// 1: isOn, 0: isOff
        /// </summary>
        [Range(0, 1)]
        [SerializeField] int status;
        private BackItem curItem;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

            doorData = DataSceneManager.Instance.BackItemDataSO.doorData;

            image.sprite = doorData.curtainsSprites[status];
            image.SetNativeSize();
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (status == 0) return;

            if (item.backitem == null) return;

            curItem = item.backitem;

            if (curItem == null) return;
            if (Vector2.Distance(curItem.transform.position, itemZone.position) > 1) return;

            curItem.KillDragging();
            curItem.transform.SetParent(itemZone);
            curItem.JumpToEndLocalPos(Vector3.zero);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            status = 1 - status;

            image.sprite = doorData.curtainsSprites[status];
            image.SetNativeSize();

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Scratch);
        }

    }
}