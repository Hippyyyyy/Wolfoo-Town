using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class MilkTeaPearl : MonoBehaviour
    {
        private Tween _tweenAnim;
        private Sprite mySprite;
        private bool isAssigned;

        public Sprite Sprite { get => mySprite; }

        private void Start()
        {
            mySprite = GetComponent<SpriteRenderer>().sprite;
            if (!isAssigned) transform.localScale = Vector3.zero;
        }
        private void OnDestroy()
        {
            if (_tweenAnim != null) _tweenAnim?.Kill();
        }
        private void PlayAnim()
        {
            var rd = Random.Range(1f, 3f);
            _tweenAnim = DOVirtual.DelayedCall(rd, () =>
            {
                _tweenAnim = transform.DOPunchScale(new Vector3(0.3f, -0.3f, 0), 0.25f, 4).OnComplete(() =>
                {
                    PlayAnim();
                });
            });
        }
        private void PlayAnim2()
        {

        }
        public void Spawn()
        {
            isAssigned = true;
            transform.localScale = Vector3.zero;
            _tweenAnim = transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                PlayAnim();
            });
        }
        private void KillAnim()
        {
            if (_tweenAnim != null) _tweenAnim?.Kill();
        }
        public void Hide()
        {
            gameObject.SetActive(false);
            KillAnim();
        }
    }
}
