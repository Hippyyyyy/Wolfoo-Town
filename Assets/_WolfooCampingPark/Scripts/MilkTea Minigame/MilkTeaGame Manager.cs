using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall.Minigame.MilkTea
{
    public class MilkTeaGameManager : UIMinigame
    {
        [SerializeField] Straw straw;
        [SerializeField] MilkTeaFillBar fillBar;
        [SerializeField] MilkTeaMap[] mapPbs;
        [SerializeField] Transform mapHolder;

        private int countMilkTeaSuccess;
        private int countMap;
        private MilkTeaMap curMap;

        public Action<int> OnCompleteMap;

        protected override void Start()
        {
            base.Start();

            straw.OnCollisionWithBall += OnCollisionWithBall;
            fillBar.Assign(5, 6);

            GameManager.GetScreenRatio(null, null, () =>
            {
                mapHolder.localScale = Vector3.one * 0.9f;
            });
            OnChangeMap();
        }

        private void OnCollisionWithBall(MilkTeaPearl ball)
        {
            ball.Hide();
            SoundCampingParkManager.Instance.PlayOtherSfx(SoundTown<SoundCampingParkManager>.SFXType.Suck);
            straw.Assign(ball.Sprite);
            straw.SuckBall(() =>
            {
                fillBar.Fill(() =>
                {
                    countMilkTeaSuccess++;
                    if(countMilkTeaSuccess == 5)
                    {
                        countMilkTeaSuccess = 0;
                        if (countMap > mapPbs.Length - 1)
                        {
                            OnBack();
                        }
                        else
                        {
                            OnChangeMap();
                        }
                    }
                });
            });
        }

        private void OnChangeMap()
        {
            var map = Instantiate(mapPbs[countMap], mapHolder);
            map.Spawn();
            if (curMap != null)
                curMap.Hide();
            curMap = map;
            countMap++;
            SoundCampingParkManager.Instance.PlayOtherSfx(SoundTown<SoundCampingParkManager>.SFXType.Spawn);
        }

        protected override void OnBack()
        {
            OnBackWithAnimation(BackAnimType.Scale, () =>
            {
                OnCompleteMap?.Invoke(countMap - 1);
            });
        }
    }
}
