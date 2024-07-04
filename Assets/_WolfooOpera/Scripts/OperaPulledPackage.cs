using DG.Tweening;
using SCN;
using SCN.UIExtend;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class OperaPulledPackage : BackItem
    {
        [SerializeField] Image pullBarImg;
        [SerializeField] Vector2 limitPullValue;
        [SerializeField] protected HorizontalScrollInfinity horizontalScroll;
        [SerializeField] protected Transform packagedArea;
        [SerializeField] Animator _openAnim;

        private bool isOpen;
        private Tweener _tweenDoor;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
            if (isOpen) pullBarImg.rectTransform.sizeDelta = new Vector2(limitPullValue.y, pullBarImg.rectTransform.sizeDelta.y);
            else pullBarImg.rectTransform.sizeDelta = new Vector2(limitPullValue.x, pullBarImg.rectTransform.sizeDelta.y);
        }
        protected virtual void OnDestroy()
        {
            if (_tweenDoor != null) _tweenDoor?.Kill();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            _openAnim.enabled = false;

            isOpen = !isOpen;
            PlayDoorAnim();
        }

        private void PlayDoorAnim()
        {
            SoundOperaManager.Instance.PlayOtherSfx(SoundTown<SoundOperaManager>.SFXType.SlideDoor);
            if (isOpen)
            {
                horizontalScroll.gameObject.SetActive(true);
                horizontalScroll.PlayAutoMove();
                _tweenDoor?.Kill();
                _tweenDoor = DOVirtual.Float(pullBarImg.rectTransform.sizeDelta.x, limitPullValue.y, 0.5f, (value) =>
                {
                    pullBarImg.rectTransform.sizeDelta = new Vector2(value, pullBarImg.rectTransform.sizeDelta.y);
                });
            }
            else
            {
                _tweenDoor?.Kill();
                _tweenDoor = DOVirtual.Float(pullBarImg.rectTransform.sizeDelta.x, limitPullValue.x, 0.5f, (value) =>
                {
                    pullBarImg.rectTransform.sizeDelta = new Vector2(value, pullBarImg.rectTransform.sizeDelta.y);
                }).OnComplete(() =>
                {
                    horizontalScroll.gameObject.SetActive(false);
                });
            }
        }
    }
}