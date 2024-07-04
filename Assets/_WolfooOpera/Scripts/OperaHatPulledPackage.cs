using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class OperaHatPulledPackage : OperaPulledPackage
    {
        [SerializeField] OperaHatSkinHolder _skinHolder;
        private Tween _tween;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;

            _tween = DOVirtual.DelayedCall(0.2f, () =>
            {
                horizontalScroll.Setup(_skinHolder.skinSprites.Length, this);
                horizontalScroll.gameObject.SetActive(false);
                horizontalScroll.PlayAutoMove();
            });

        }
        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
            OperaHatScrollItem.OnSetHatToPackage += GetHatToPackage;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
            OperaHatScrollItem.OnSetHatToPackage -= GetHatToPackage;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_tween != null) _tween?.Kill();
        }

        private void GetHatToPackage(Transform item)
        {
            var package = item.GetComponent<OperaHatScrollItem>();
            if (package == null) return;
            horizontalScroll.ScrollTo(item);
        }
        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.operaHatScrollItem != null)
            {
                var id = obj.operaHatScrollItem.Id;
                obj.operaHatScrollItem.Setup(
                    id,
                    _skinHolder.skinSprites[id],
                    _skinHolder.wearingLocalPoss[id],
                    packagedArea,
                    this);
            }
        }
    }
}