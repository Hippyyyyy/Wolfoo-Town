using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Carpet : BackItem
    {
        [SerializeField] ParticleSystem _smokeFx;
        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            OnPunchScale();
            _smokeFx.Play();
        }
    }
}