using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class BlueDoor : BackItem
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

            image.rectTransform.pivot = new Vector2(status == 1 ? 1 : 0, 0.5f);
            image.sprite = doorData.blueDoorSprites[status];
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (status == 0) return;

            if (item.backitem == null) return;

            curItem = null;
            if (item.money != null) curItem = item.money;
            if (item.backitem.IsBeverage) curItem = item.backitem;
            if (item.backitem.IsFood) curItem = item.backitem;

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

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            status = 1 - status;

            image.rectTransform.pivot = new Vector2(status == 1 ? 1 : 0, 0.5f);
            image.sprite = doorData.blueDoorSprites[status];

        }

    }
}