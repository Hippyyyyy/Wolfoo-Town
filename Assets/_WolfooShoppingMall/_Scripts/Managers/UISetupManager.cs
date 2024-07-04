using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class UISetupManager : MonoBehaviour
    {
        [Header("----------------- PROPERTISE -----------------")]
        public Image maskBg;
        public Image blackBg;
        public Image blackBgNoMask;

        [Header("----------------- POINT -----------------")]
        public Transform center;
        public Transform outsideLeft;
        public Transform outsideDown;
        public Transform pivotLeft;
        public Transform pivotRight;
        public List<Transform> moveFloorTrans;
        public List<Transform> moveFloor2Trans;
        public Transform LeftButtonZone;

        private static UISetupManager instance;
        private Tween delayTween;

        public static UISetupManager Instance { get => instance; }

        private void Awake()
        {
            if (instance == null) instance = this;
            if (maskBg != null) maskBg.gameObject.SetActive(false);
            if (blackBg != null) blackBg.gameObject.SetActive(false);
        }

        private void Start()
        {
            if (maskBg == null) return;
            maskBg.gameObject.SetActive(true);
            delayTween = DOVirtual.DelayedCall(0.5f, () =>
            {
                maskBg.gameObject.SetActive(false);
            });
        }
    }
}