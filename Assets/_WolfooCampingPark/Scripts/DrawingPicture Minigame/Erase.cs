using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class Erase : CampingParkBrush
    {
        private Vector3 startPos;
        private Sequence _tweenMove;

        protected override void Start()
        {
            base.Start();
            startPos = transform.position;
            ChangeColor(Color.white);

        }
        public new void ChangeColor(Color color)
        {
            curColor = color;
        }
        protected override void GetBeginDrag()
        {
            _tweenMove?.Kill();
            OnBeginErase?.Invoke();
            base.GetBeginDrag();
        }
        protected override void GetEndDrag()
        {
            base.GetEndDrag();
            OnBack();
        }
        private void OnBack()
        {
            _tweenMove?.Kill();
            _tweenMove = transform.DOJump(startPos, 3, 1, 0.5f);
        }
    }
}
