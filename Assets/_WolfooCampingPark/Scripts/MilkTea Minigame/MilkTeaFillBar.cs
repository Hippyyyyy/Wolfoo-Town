using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall.Minigame.MilkTea
{
    public class MilkTeaFillBar : MonoBehaviour
    {
        [SerializeField] Image loadingBarImg;
        [SerializeField] ParticleSystem lightingFx;
        [SerializeField] ParticleSystem sparkingfx;
        [SerializeField] Image[] partImgs;
        [SerializeField] Image[] fxImgs;

        private Tweener _tweenFill;
        private Tweener _tweenFade;
        private Tweener _tweenScale;

        private int totalItemPerPart;
        private int totalPart;
        private int countItemPerPart;
        private int countPart;
        private float countFillValue;
        private float totalFillValue;

        private void OnDestroy()
        {
            if (_tweenFill != null) _tweenFill?.Kill();
            if (_tweenFade != null) _tweenFade?.Kill();
            if (_tweenScale != null) _tweenScale?.Kill();
        }
        private void Start()
        {
            for (int i = 0; i < partImgs.Length; i++)
            {
                fxImgs[i].DOFade(0, 0);
            }
            loadingBarImg.fillAmount = 0;
        }
        public void Assign(int itemPerPart, int totalPart)
        {
            totalItemPerPart = itemPerPart;
            this.totalPart = totalPart;
            countItemPerPart = 0;
            countPart = 0;

            totalFillValue = totalItemPerPart * totalPart;
            countFillValue = 0;
        }
        public void Fill(System.Action OnSuccess)
        {
            if (countFillValue >= totalFillValue) return;

            // OnFilling
            countFillValue++;
            _tweenFill?.Kill();
            _tweenFill = loadingBarImg.DOFillAmount(countFillValue / totalFillValue, 0.5f).OnComplete(() =>
            {
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Correct);
                OnSuccess?.Invoke();
                OnCheckPartProcess();
            });
        }
        private void OnCheckPartProcess()
        {
            countItemPerPart++;
            if (countItemPerPart == totalItemPerPart)
            {
                countItemPerPart = 0;

                countPart++;
                var curPartIcon = partImgs[countPart - 1];
                var curPartFx = fxImgs[countPart - 1];
                _tweenFade = curPartFx.DOFade(1, 0.5f);
                _tweenScale = curPartIcon.transform.DOPunchScale(Vector3.one * 0.2f, 0.5f, 1);
                // Play Fx
                if(sparkingfx != null)
                {
                    sparkingfx.transform.position = curPartIcon.transform.position;
                    sparkingfx.Play();
                }
            }
        }
    }
}
