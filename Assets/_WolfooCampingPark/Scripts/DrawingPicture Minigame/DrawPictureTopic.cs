using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class DrawPictureTopic : MonoBehaviour
    {
        [SerializeField] Button button;
        [SerializeField] Image showImg;
        [SerializeField] Image hideImg;
        [SerializeField] DrawingScrollView drawingScrollPb;
        [SerializeField] Transform scrollHolder;
        [SerializeField] int countItem;

        public System.Action<DrawPictureTopic> OnClick;
        private DrawingScrollView myScroll;
        private Tweener _tween;

        private void Start()
        {
            button.onClick.AddListener(OnPress);
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }

        public void Hide()
        {
            if (myScroll != null) myScroll.gameObject.SetActive(false);
            showImg.gameObject.SetActive(false);
            hideImg.gameObject.SetActive(true);
            _tween?.Kill();
            transform.localScale = Vector3.one;
        }
        public void Show()
        {
            if (myScroll != null) myScroll.gameObject.SetActive(true);
            showImg.gameObject.SetActive(true);
            hideImg.gameObject.SetActive(false);
        }

        public void OnPress()
        {
            if (myScroll == null)
            {
                myScroll = Instantiate(drawingScrollPb, scrollHolder);
                myScroll.Setup(countItem, this);
            }

            _tween?.Kill();
            transform.localScale = new Vector3(1.2f, 1.1f, 0);
            _tween = transform.DOPunchScale(Vector3.one * 0.1f, 0.3f, 2);
           
            OnClick?.Invoke(this);
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }
    }
}
