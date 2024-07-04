using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static _WolfooShoppingMall.SelfHouseDataSO;

namespace _WolfooShoppingMall
{
    public class OptionViews : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] DecorItemViews itemViewPb;
        [SerializeField] Transform itemViewHolder;

        List<DecorItemViews> decorItemViews = new List<DecorItemViews>();
        public Action<OptionViews> OnClick;

        public int TopicId { get; private set; }

        private void Start()
        {
            button.onClick.AddListener(OnClickIcon);
        }
        private void OnDestroy()
        {
            foreach (var item in decorItemViews)
            {
                item.OnClick -= GetItemViewClick;
            }
        }

        private void GetItemViewClick(DecorItemViews obj)
        {
           // OnClickIcon();
            foreach (var item in decorItemViews)
            {
                item.GetClick(obj);
            }
        }

        public void GetClickOptionView(int id)
        {
            for (int i = 0; i < decorItemViews.Count; i++)
            {
                if (id == TopicId)
                {
                    decorItemViews[i].Show();
                    if (i == 0) decorItemViews[i].ShowScroll();
                    else decorItemViews[i].HideScroll();
                }
                else
                {
                    decorItemViews[i].Hide();
                    decorItemViews[i].HideScroll();
                }
            }
        }
        //public void GetClickOptionView(int id)
        //{
        //    for (int i = 0; i < decorItemViews.Count; i++)
        //    {
        //        if (decorType == Type)
        //        {
        //            decorItemViews[i].Show();
        //            if (i == 0) decorItemViews[i].ShowScroll();
        //            else decorItemViews[i].HideScroll();
        //        }
        //        else
        //        {
        //            decorItemViews[i].Hide();
        //            decorItemViews[i].HideScroll();
        //        }
        //    }
        //}

        public void Assign(int parentId, DecoratedData decoratedData)
        {
            TopicId = parentId;

            itemViewPb.gameObject.SetActive(false);
            for (int i = 0; i < decoratedData.OptionDatas.Length; i++)
            {
                var item = decoratedData.OptionDatas[i];
                if (item.ItemSprites.Length == 0) continue;
                var itemView = Instantiate(itemViewPb, itemViewHolder);
                itemView.Assign(i, item, TopicId);
                itemView.OnClick += GetItemViewClick;
                decorItemViews.Add(itemView);
            }
        }


        private void OnClickIcon()
        {
            OnClick?.Invoke(this);
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }
    }
}
