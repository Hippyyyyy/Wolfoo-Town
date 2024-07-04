using SCN.UIExtend;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static _WolfooShoppingMall.SelfHouseDataSO;

namespace _WolfooShoppingMall
{
    public class DecorItemViews : MonoBehaviour
    {
        [SerializeField] Button decorBtn;
        [SerializeField] Image iconCoverImg;
        [SerializeField] Image iconImg;
        [SerializeField] Color clickColor;
        [SerializeField] Color unClickColor;
        [SerializeField] OptionScrollView scrollViewPb;
        [SerializeField] Transform scrollViewHolder;

        public Action<DecorItemViews> OnClick;

        public int OptionId { get; private set; }

        OptionScrollView myScrollView;

        public int Id { get; private set; }

        private void Start()
        {
            decorBtn.onClick.AddListener(OnItemClick);
        }
        private void OnDestroy()
        {
        }

        public void GetClick(DecorItemViews obj)
        {
            if (obj.Id == Id && obj.OptionId == OptionId)
            {
                ShowScroll();
            }
            else
            {
                HideScroll();
            }
        }

        public void HideScroll()
        {
            SetUnClickState();
            myScrollView.Hide();
        }
        public void ShowScroll()
        {
            SetClickState();
            myScrollView.Show();
            myScrollView.PlayAutoMove();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Assign(int id, OptionData optionData, int parentId)
        {
            Id = id;
            OptionId = parentId;
            iconImg.sprite = optionData.Icon;

            scrollViewPb.gameObject.SetActive(false);
            myScrollView = Instantiate(scrollViewPb, scrollViewHolder);
            myScrollView.gameObject.SetActive(true);
            myScrollView.Setup(optionData.ItemSprites.Length, optionData.ItemSprites, optionData.OptionScrollItems, optionData.ItemPrefabs);
            myScrollView.gameObject.SetActive(false);
        }

        public void SetUnClickState()
        {
            iconCoverImg.color = unClickColor;
            iconImg.color = clickColor;
        }
        public void SetClickState()
        {
            iconCoverImg.color = clickColor;
            iconImg.color = unClickColor;
            myScrollView.gameObject.SetActive(true);
        }

        private void OnItemClick()
        {
            OnClick?.Invoke(this);
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }
    }
}
