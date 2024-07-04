using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class LampWorld : BackItemWorld, ILighting
    {
        [SerializeField] SpriteRenderer lightSpriteReder;

        [NaughtyAttributes.OnValueChanged("OnLighting")]
        public bool IsLight;

        public override void Setup()
        {
            IsClick = true;
            IsStandingOnTable = true;
            base.Setup();
        }
        protected override void OnClick()
        {
            base.OnClick();
            IsLight = !IsLight;
            OnLighting();
        }

        public void OnLighting()
        {
            lightSpriteReder.gameObject.SetActive(IsLight);
            Dance();
        }
    }
}
