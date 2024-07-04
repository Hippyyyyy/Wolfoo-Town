using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Guitar : BackItem
    {
        [SerializeField] ParticleSystem musicFx;
        private bool isPlaying;

        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            IsCarry = true;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this });
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            isPlaying = !isPlaying;
            if (isPlaying && musicFx != null)
            {
                musicFx.Play();
                PlaySound();
            }
            else
                if (musicFx != null)
            {
                musicFx.Stop();
            }
        }

    }
}