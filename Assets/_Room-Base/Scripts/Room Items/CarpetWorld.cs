using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CarpetWorld : BackItemWorld
    {
        [SerializeField] ParticleSystem smokeFx;
        private Tweener _tweenDance;

        public override void Setup()
        {
            IsClick = true;
            base.Setup();
        }
        protected override void OnClick()
        {
            base.OnClick();
            smokeFx.Play();
            Dance();
        }
    }
}
