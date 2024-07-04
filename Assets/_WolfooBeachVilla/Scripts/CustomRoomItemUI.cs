using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CustomRoomItemUI : MonoBehaviour
    {
        [SerializeField] Button removeBtn;
        [SerializeField] Button rotateBtn;
        [SerializeField] Animator _animator;
        [SerializeField] string playName;
        [SerializeField] string closeName;
        [SerializeField] Canvas myCanvas;

        public Action OnClickRemove;
        public Action OnClickRotate;

        private void Start()
        {
            removeBtn.onClick.AddListener(OnClickRemoveBtn);
            rotateBtn.onClick.AddListener(OnClickRotateBtn);

            var pos = myCanvas.transform.localPosition;
            myCanvas.transform.localPosition = new Vector3(pos.x, pos.y, -5);
        }

        public void SortingOrder(int order)
        {
            myCanvas.sortingOrder = order;
            var zPos = -order / 50f;
            transform.position = new Vector3(transform.position.x, transform.position.y, zPos < -5 ? -5 : zPos);
        }

        private void OnClickRotateBtn()
        {
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            OnClickRotate?.Invoke();
        }

        private void OnClickRemoveBtn()
        {
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Incorrect);
            OnClickRemove?.Invoke();
        }

        void PlayShowAnim()
        {
            if (_animator != null) _animator.Play(playName, 0, 0);
        }
        void PlayHideAnim()
        {
            if (_animator != null) _animator.Play(closeName, 0, 0);
        }

        public void Show()
        {
            //   transform.localScale = Vector3.one;
            PlayShowAnim();
        }
        public void Hide()
        {
            //   transform.localScale = Vector3.zero;
            PlayHideAnim();
        }
    }
}
