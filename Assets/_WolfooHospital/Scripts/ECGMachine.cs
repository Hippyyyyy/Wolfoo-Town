using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ECGMachine : BackItem
    {
        [SerializeField] Animator _animator;
        [SerializeField] string playName;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            SoundManager.instance.PlayOtherSfx(myClip);
            _animator.Play(playName, 0, 0);
        }
    }
}