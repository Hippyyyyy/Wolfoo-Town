using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using UnityEngine.EventSystems;
using SCN.Common;
using SCN.UIExtend;

namespace _WolfooShoppingMall
{
    public class ButtonResetCharacter : MonoBehaviour
    {
        public SCN.UIExtend.CustomButton CustomButton;
        public Image fillImage;
        public float maxFillValue = 1.0f;
        public float fillDuration = 1.0f;
        public EventTrigger eventTrigger;
        private Coroutine fillCoroutine;
        private Tween scale;
        bool isScaleDone = false;

        private void Start()
        {
            fillImage.fillAmount = 0f;
        }

        public void OnPointerDown(Action onDone)
        {
            if (fillCoroutine != null) StopCoroutine(fillCoroutine);
            fillCoroutine = StartCoroutine(FillImage(onDone));
            scale.Kill();
            scale = transform.DOScale(0.6f, 0.4f);
        }

        public void OnPointerUp()
        {
            if (!isScaleDone)
            {
                scale.Kill();
                scale = transform.DOScale(0.5f, 0.2f);
            }
            if (fillCoroutine != null) StopCoroutine(fillCoroutine);
            fillCoroutine = StartCoroutine(ResetFill());
            
        }


        private IEnumerator FillImage(Action onDone)
        {
            float elapsedTime = 0f;

            while (elapsedTime < fillDuration)
            {
                float fillValue = Mathf.Clamp01(elapsedTime / fillDuration) * maxFillValue;
                fillImage.fillAmount = fillValue;

                elapsedTime += Time.deltaTime;
                yield return null;
            }
            onDone();
        }

        private IEnumerator ResetFill()
        {
            float startFillValue = fillImage.fillAmount;

            float elapsedTime = 0f;

            while (elapsedTime < fillDuration)
            {
                float fillValue = Mathf.Clamp01(startFillValue - (elapsedTime / fillDuration)) * maxFillValue;
                fillImage.fillAmount = fillValue;

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            fillImage.fillAmount = startFillValue;
        }
        
        public void ResetFillAmount()
        {
            fillImage.fillAmount = 0f;
        }
        public void ScaleIn(Action onDone)
        {
            isScaleDone = true;
            if (isScaleDone)
            {
                transform.DOScale(0f, 0.4f).SetEase(Ease.InBack).OnComplete(() => {
                    onDone();
                });
            }
        }
        #if UNITY_EDITOR
                private void OnValidate()
                {
                    eventTrigger = GetComponent<EventTrigger>();
                }
        #endif
    }
}
