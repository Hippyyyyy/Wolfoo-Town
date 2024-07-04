using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Led : MonoBehaviour
    {
        [SerializeField] Image lightImg;
        [SerializeField] Image blurLightImg;
        [SerializeField] Color[] colors;
        private Color myColor;
        private Tween _tween;

        private float speed = 0.2f;
        public float Speed { get => speed; }

        public void Init()
        {
            myColor = colors[Random.Range(0, colors.Length)];
            lightImg.color = myColor;
            blurLightImg.color = myColor;

            transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        public void Shine(float delayTime)
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(delayTime, () =>
            {
                blurLightImg.gameObject.SetActive(true);
                _tween = DOVirtual.DelayedCall(speed, () =>
                {
                    blurLightImg.gameObject.SetActive(false);
                });
            });
        }
        public void TurnOff()
        {
            _tween?.Kill();
            blurLightImg.gameObject.SetActive(false);
        }
    }
}