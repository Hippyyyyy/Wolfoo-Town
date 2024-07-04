using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class FireExtinguisher : Toy
    {
        [SerializeField] ParticleSystem fx;
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            if (fx != null) { fx.Stop(); }
            image.sprite = idleSprite;
            image.SetNativeSize();
            SoundManager.instance.TurnOffLoop();
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;
            if (fx != null) { fx.Play(); }
            SoundManager.instance.PlayLoopingSfx(myClip);
            image.sprite = transformSprite;
            image.SetNativeSize();
        }
    }
}