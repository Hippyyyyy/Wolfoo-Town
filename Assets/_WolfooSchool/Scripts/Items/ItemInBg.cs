using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace _WolfooSchool
{
    public class ItemInBg : MonoBehaviour
    {
        [SerializeField] Transform beginTrans;
        [SerializeField] Transform endTrans;
        private Tweener moveTween;

        private void Start()
        {
            OnMove();
        }
        private void OnDestroy()
        {
            if (moveTween != null) moveTween?.Kill();
        }
        void OnMove()
        {
            moveTween = transform.DOMoveX(endTrans.position.x, Random.Range(0.3f, 0.5f))
            .SetEase(Ease.Linear)
            .SetSpeedBased(true)
            .OnComplete(() =>
            {
                transform.position = new Vector3(beginTrans.position.x, transform.position.y, 0);
                OnMove();
            });
        }
    }
}
