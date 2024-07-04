using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Perfume : BackItem
    {
        [SerializeField] ParticleSystem _particleSystem;
        [SerializeField] Transform smokePerfumeArea;
        private AudioSource myAus;

        public Transform SmokePerfumeArea { get => smokePerfumeArea; }

        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            _particleSystem.gameObject.SetActive(false);

            if(myClip != null)
            {
                myAus = SoundOperaManager.Instance.CreateNewAus(new SoundBase<SoundOperaManager>.Item(GetInstanceID(), "AUS - Perfume", myClip, true));
            }
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            if (!canDrag) return;
            _particleSystem.gameObject.SetActive(true);

            if (myAus != null && !myAus.isPlaying) myAus.Play();

            EventDispatcher.Instance.Dispatch(new EventKey.OnDragBackItem { backItem = this, perfume = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            _particleSystem.gameObject.SetActive(false);
            if(myAus != null) myAus.Stop();
        }
    }
}