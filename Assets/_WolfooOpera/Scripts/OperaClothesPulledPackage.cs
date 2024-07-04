using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class OperaClothesPulledPackage : OperaPulledPackage
    {
        private Tween _tween1;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;

            _tween1 = DOVirtual.DelayedCall(0.2f, () =>
            {
                horizontalScroll.Setup(characterData.CharacterData.frontSkinSprite.Length, this);
                horizontalScroll.gameObject.SetActive(false);
                horizontalScroll.PlayAutoMove();
            });
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
            OperaClothesScrollItem.OnSetClothingToPackage += GetClothingToPackage;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
            OperaClothesScrollItem.OnSetClothingToPackage -= GetClothingToPackage;
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_tween1 != null) _tween1?.Kill();
        }
        private void GetClothingToPackage(Transform item)
        {
            var package = item.GetComponent<OperaClothesScrollItem>();
            if (package == null) return;
            horizontalScroll.ScrollTo(item);
        }
        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.operaClothesScrollItem != null)
            {
                var character = characterData.CharacterData;
                var id = obj.operaClothesScrollItem.Id;
                obj.operaClothesScrollItem.Setup(
                    id,
                    character.frontSkinSprite[id],
                    character.behindSkinSprite[id],
                    character.foldSkinSprite[id],
                    packagedArea,
                    this);
            }
        }
    }
}