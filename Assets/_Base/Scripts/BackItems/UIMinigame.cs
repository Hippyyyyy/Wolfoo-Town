using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public abstract class UIMinigame : MonoBehaviour
    {
        [SerializeField] Button backBtn;
        [SerializeField] Transform bgHolder;

        private Tweener _tween;

        protected abstract void OnBack();
        public enum BackAnimType
        {
            Scale,
        }
        protected virtual void Start()
        {
            if(backBtn != null)
            {
                backBtn.onClick.AddListener(OnBack);
            }
            EventRoomBase.OnCreatedMinigame?.Invoke();
        }
        protected virtual void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
            EventRoomBase.OnCompletedMinigame?.Invoke();
        }
        protected void OnBackWithAnimation(BackAnimType animType = BackAnimType.Scale, System.Action OnSuccess = null)
        {
            switch (animType)
            {
                case BackAnimType.Scale:
                    _tween = bgHolder.DOScale(Vector3.one * 0.2f, 0.5f).SetEase(Ease.InBack).OnComplete(() =>
                    {
                        OnSuccess?.Invoke();
                        Destroy(this.gameObject);
                    });
                    break;
            }
        }
    }
}
