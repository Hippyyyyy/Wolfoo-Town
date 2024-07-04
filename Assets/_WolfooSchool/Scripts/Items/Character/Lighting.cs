using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using _WolfooShoppingMall;

namespace _WolfooSchool
{
    public class Lighting : BackItem
    {
        [SerializeField] List<Sprite> sprites;
        bool isTurnOn;
        private Tweener rotaeTween;
        private Vector3 startPos;
        private System.Action OnChangeState;

        enum State
        {
            On,
            Off
        }
        protected override void Start()
        {
            base.Start();
            startPos = transform.localPosition;
            isTurnOn = !GUIManager.instance.darkBlurScrene.activeSelf;
            OnChangeState += ChangeState;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            OnChangeState -= ChangeState;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            isTurnOn = !isTurnOn;
            GetClick();
        }

        void ChangeState()
        {
            image.sprite = sprites[isTurnOn ?  1 : 0];
            image.SetNativeSize();
        }

        void GetClick()
        {
            if (rotaeTween != null)
            {
                rotaeTween?.Kill();
                image.transform.localPosition = startPos;
            }

            ChangeState();
            GUIManager.instance.darkBlurScrene.gameObject.SetActive(!isTurnOn);

            rotaeTween = image.transform.DOPunchPosition(Vector3.down * 40, 1, 2);
            OnChangeState?.Invoke();
        }
    }
}