using _Base;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class MashmallowGame : BackItem
    {
        [SerializeField] Mashmallow mashmallowPb;
        [SerializeField] Transform spawnZone;
        [SerializeField] MashmallowMachineAnimation machineAnimation;
        [SerializeField] Image maskImg;
        private Tween tweenDelay;
        private MashmallowData myData;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void InitData()
        {
            base.InitData();
            myData = DataSceneManager.Instance.ItemDataSO.MashmallowData;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            // Sound Machine Mashmallow Here
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Shaking);

            maskImg.gameObject.SetActive(false);
            machineAnimation.PlayExcute();
            tweenDelay = DOVirtual.DelayedCall(machineAnimation.GetTimeAnimation(MashmallowMachineAnimation.AnimState.Excute), () =>
            {
                machineAnimation.PlayIdle();
                maskImg.gameObject.SetActive(true);
                var mashmallow = Instantiate(mashmallowPb, spawnZone);
                mashmallow.AssingItem(myData.sprites[Random.Range(0, myData.sprites.Length)]);
                mashmallow.transform.localPosition = spawnZone.GetChild(0).localPosition;
                mashmallow.transform.DOLocalMoveY(spawnZone.GetChild(1).localPosition.y, 1).SetEase(Ease.OutBounce)
                .OnComplete(() =>
                {
                    // Sound Lack cack Here
                });

                tweenDelay = DOVirtual.DelayedCall(0.5f, () =>
                {
                    canClick = true;
                });
            });
        }

    }
}