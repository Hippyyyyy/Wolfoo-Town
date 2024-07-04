using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CampingParkStatue : BackItemWorld
    {
        [SerializeField] Sprite idleSprite;
        [SerializeField] Sprite transformSprite;
        [SerializeField] Transform mySit;

        public System.Action<bool> OnTransform;
        private Tweener _tweenFade;
        [SerializeField] bool isGold;

        public override void Setup()
        {
            IsDragable = true;
            IsStandingOnTable = true;
            base.Setup();
            if (isGold)
                OnTransform?.Invoke(isGold);
        }
        protected override void OnBeginDrag()
        {
            base.OnBeginDrag();
            if (isGold)
            {
                isGold = false;
                Transforming(false);
                OnTransform?.Invoke(false);
            }
        }

        protected override void OnEndDrag()
        {
            base.OnEndDrag();
            var distance = Vector2.Distance(transform.position, mySit.position);

            if (distance < 1)
            {
                isGold = true;
                JumpTo(mySit.position, () =>
                {
                    Transforming(true);
                    OnTransform?.Invoke(true);
                });
            }
        }

        private void Transforming(bool isGold)
        {
            _tweenFade?.Kill();
            MySprite.DOFade(0.5f, 0);
            if (isGold)
            {
                MySprite.sprite = transformSprite;
            }
            else
            {
                MySprite.sprite = idleSprite;
            }
            _tweenFade = MySprite.DOFade(1, 0.5f);
        }
    }
}
