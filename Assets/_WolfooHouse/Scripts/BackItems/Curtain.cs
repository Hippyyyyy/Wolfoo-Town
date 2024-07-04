using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Curtain : BackItem
    {
        [SerializeField] bool isOpen;
        [SerializeField] CurtainAnimation _animation;
        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
            PlayAnim();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            isOpen = !isOpen;
            PlayAnim();
        }

        void PlayAnim()
        {
            if (isOpen) _animation.Open();
            if (!isOpen) _animation.Close();
        }
    }
}