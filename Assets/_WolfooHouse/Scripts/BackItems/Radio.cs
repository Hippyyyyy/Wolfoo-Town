using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Radio : BackItem
    {
        [SerializeField] ParticleSystem soundFx;
        private bool isOn;

        protected override void InitItem()
        {
            canClick = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            isOn = !isOn;
            if (isOn) soundFx.Play(); else soundFx.Stop();
        }
    }
}