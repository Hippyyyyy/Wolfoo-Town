using _Base;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class BreadMachine : BackItem
    {
        [SerializeField] Transform itemZone;
        [SerializeField] Cake cakePb;
        [SerializeField] BreadMachineAnimation machineAnimation;
        [SerializeField] Image iconImg;
        [SerializeField] Image maskImg;

        private Tween tweenDelay;
        private Tweener fadeTween;
        private int curCakeIdx;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

            iconImg.DOFade(0, 0);
        }

        void OnShowIcon()
        {
            iconImg.sprite = data.CakeData.cakeSprites[curCakeIdx];
            iconImg.SetNativeSize();
            GameManager.instance.ScaleImage(iconImg, 160, 160);

            fadeTween = iconImg.DOFade(0.8f, 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.BreadMachine);
            maskImg.gameObject.SetActive(false);
            curCakeIdx = Random.Range(0, data.CakeData.cakeSprites.Length);
            machineAnimation.PlayExcute();
            tweenDelay = DOVirtual.DelayedCall(machineAnimation.GetTimeAnimation(BreadMachineAnimation.AnimState.Excute), () =>
            {
                OnShowIcon();
                machineAnimation.PlayExcute2();
                maskImg.gameObject.SetActive(true);

                var cake = Instantiate(cakePb, itemZone);
                cake.AssignItem(data.CakeData.cakeSprites[curCakeIdx]);
                cake.transform.localPosition = itemZone.GetChild(0).localPosition;
                cake.OnMaking(itemZone.GetChild(1).localPosition, () =>
                {
                    fadeTween?.Kill();
                    iconImg.DOFade(0, 0);
                    machineAnimation.PlayIdle();
                    canClick = true;
                });
            });
        }
    }
}