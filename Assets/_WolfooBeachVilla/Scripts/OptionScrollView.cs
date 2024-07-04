using DG.Tweening;
using SCN.UIExtend;
using System;
using System.Collections;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class OptionScrollView : VerticalScrollInfinity
    {
        [SerializeField] Transform itemHolder;
        private Tween _tween;
        private CustomRoomItem curCustomItem;
        private OptionScrollItem[] scrollitems;
        private PaintingWorld curPainting;

        private void Start()
        {
            //    BeachVillaEventManager.OnInitScrollItem += GetInitScrollItem;
        }

        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
            //   BeachVillaEventManager.OnInitScrollItem -= GetInitScrollItem;
            if (scrollitems != null)
                for (int i = 0; i < scrollitems.Length; i++)
                {
                    scrollitems[i].OnBeginDrag -= OnScrollItemBeginDrag;
                    scrollitems[i].OnEndDrag -= OnScrollItemEndDrag;
                    scrollitems[i].OnDrag -= OnScrollItemDrag;
                }
        }
        public void Setup(int order, Sprite[] itemSprites, OptionScrollItem optionScrollItemPb, GameObject[] itemObjs)
        {
            prefab = optionScrollItemPb;
            Setup(order, this);

            _tween = DOVirtual.DelayedCall(0.5f, () =>
            {
                scrollitems = maskTrans.GetComponentsInChildren<OptionScrollItem>();
                for (int i = 0; i < scrollitems.Length; i++)
                {
                    var id = scrollitems[i].Id;
                    var customItem = itemObjs[id].GetComponent<CustomRoomItem>();
                    var paintingItem = itemObjs[id].GetComponent<PaintingWorld>();
                    if (customItem != null)
                    {
                        scrollitems[i].Assign(itemSprites[id], itemObjs.Length > 0 ? customItem : null);
                    }
                    else if (paintingItem != null)
                    {
                        scrollitems[i].Assign(itemSprites[id], itemObjs.Length > 0 ? paintingItem : null);
                    }
                    scrollitems[i].OnBeginDrag += OnScrollItemBeginDrag;
                    scrollitems[i].OnEndDrag += OnScrollItemEndDrag;
                    scrollitems[i].OnDrag += OnScrollItemDrag;
                }
            });
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
        public void Show()
        {
            gameObject.SetActive(true);
        }

        private void OnScrollItemDrag(OptionScrollItem item)
        {
            if (curCustomItem != null) curCustomItem.Drag();
            if (curPainting != null) curPainting.Drag();
        }

        private void OnScrollItemEndDrag(OptionScrollItem item)
        {
            if (curCustomItem != null) curCustomItem.EndDrag();
            if (curPainting != null) curPainting.EndDrag();
        }

        private void OnScrollItemBeginDrag(OptionScrollItem item)
        {
            if (item.RoomItemPb != null)
            {
                curCustomItem = Instantiate(item.RoomItemPb, itemHolder);
                curCustomItem.gameObject.SetActive(true);
                StartCoroutine(AssignItem());
            }
            if(item.PaintingItemPb != null)
            {
                curPainting = Instantiate(item.PaintingItemPb, itemHolder);
            }
            OnScrollItemDrag(item);
            SoundBeachVillaManager.Instance.PlayOtherSfx(SoundTown<SoundBeachVillaManager>.SFXType.Select);
        }
        IEnumerator AssignItem()
        {
            curCustomItem.Assign();
            curCustomItem.Enable(true);
            yield return new WaitForEndOfFrame();
            curCustomItem.BeginDrag();
        }
    }
}
