using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CommonItemFunction : MonoBehaviour
    {
        [SerializeField] bool isDance;
        private Vector3 startScale;
        private Tweener tweenPunch;

        private void Awake()
        {
            startScale = transform.localScale;
        }

        private void Start()
        {
            if (isDance) Danncing();
        }
        private void OnDestroy()
        {
            KillScalling();
        }
        void KillScalling()
        {
            if (tweenPunch != null)
                tweenPunch?.Kill();
            transform.localScale = startScale;

        }
        public void OnPunchScale()
        {
            KillScalling();
            tweenPunch = transform.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.5f, 5).OnComplete(() =>
            {
            });
        }
        public void Danncing()
        {
            tweenPunch = transform.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.5f, 5).SetDelay(3).OnComplete(() =>
            {
                Danncing();
            });
        }
    }
}