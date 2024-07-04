using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CampingParkSakura : BackItemWorld
    {
        [SerializeField] ParticleSystem lightingFx;
        [SerializeField] SpriteRenderer colorWater;
        [SerializeField] CampingParkStatue[] myStatues;
        [SerializeField] SpriteRenderer tree;
        private int countStatueTrasnformed;
        private Tweener tweenFade;
        private Tweener tweenFade2;

        protected override void RegisterEvent()
        {
            base.RegisterEvent();

            foreach (var statue in myStatues)
            {
                statue.OnTransform += OnStatueTransform;
            }
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            foreach (var statue in myStatues)
            {
                statue.OnTransform -= OnStatueTransform;
            }
        }
        protected override void OnKill()
        {
            base.OnKill();
            if (tweenFade != null) tweenFade?.Kill();
            if (tweenFade2 != null) tweenFade2?.Kill();
        }

        private void OnStatueTransform(bool isGold)
        {
            if (isGold)
            {
                countStatueTrasnformed++;
                countStatueTrasnformed = countStatueTrasnformed > myStatues.Length ? myStatues.Length : countStatueTrasnformed;
            }
            else
            {
                countStatueTrasnformed--;
                countStatueTrasnformed = countStatueTrasnformed < 0 ? 0 : countStatueTrasnformed;
                tweenFade = colorWater.DOFade(0, 0.25f);
                tweenFade2 = tree.DOFade(0, 0.25f);
                lightingFx.Stop();
            }

            if (countStatueTrasnformed == myStatues.Length)
            {
                tweenFade?.Kill();
                tweenFade2?.Kill();
                colorWater.DOFade(0, 0);
                tree.DOFade(0, 0);
                tweenFade2 = tree.DOFade(1, 0.25f);
                tweenFade = colorWater.DOFade(1, 1).OnComplete(() =>
                {
                    lightingFx.Play();
                    SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Lighting);
                });
            }
        }

        protected override void GetEndDragBackItem(BackItemWorld obj)
        {
            base.GetEndDragBackItem(obj);
            var statue = obj.GetComponent<CampingParkStatue>();
            if(statue != null)
            {

            }
        }
    }
}
