using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Sink : BackItem
    {
        [SerializeField] ParticleSystem waterFx;
        private bool isOn;

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
            isOn = !isOn;

            if (isOn) waterFx.Play(); else waterFx.Stop();
            if (isOn) SoundManager.instance.PlayLoopingSfx(myClip);
            else SoundManager.instance.TurnOffLoop();
        }
    }
}