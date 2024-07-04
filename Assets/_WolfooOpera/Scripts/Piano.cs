using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Piano : BackItem
    {
        [SerializeField] ParticleSystem musicFx;
        private bool isPlaying;
        private AudioSource myAus;
        private Tweener _tweenScale;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;
            if(myClip != null)
            {
               myAus = SoundOperaManager.Instance.CreateNewAus(new SoundBase<SoundOperaManager>.Item(GetInstanceID(), "AUS - Piano", myClip, true));
            }
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (!canClick) return;

            Dance();
            isPlaying = !isPlaying;
            if (isPlaying)
            {
                musicFx.Play();
                myAus.Play();
            }
            else
            {
                musicFx.Stop();
                myAus.Stop();
            }
        }

        private void Dance()
        {
            _tweenScale?.Kill();
            transform.localScale = startScale;
            _tweenScale = transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 1);
        }
    }
}