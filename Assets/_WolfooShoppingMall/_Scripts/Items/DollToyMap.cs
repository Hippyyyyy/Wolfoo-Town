using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class DollToyMap : BackItem
    {
        [SerializeField] Image dressImg;
        [SerializeField] Image accessoryImg;
        [SerializeField] Image hairImg;
        [SerializeField] DollClothingMode clothingMode;
        [SerializeField] ParticleSystem smokeFx;

        public void AssignItem(
            Sprite dressSprite,
            Sprite accessorySprite,
            Sprite hairSprite,
            Vector3 accessoryPos,
            Vector3 hairPos,
            Vector3 dressPos)
        {
            dressImg.sprite = dressSprite;
            dressImg.SetNativeSize();
            dressImg.transform.localPosition = dressPos;

            accessoryImg.sprite = accessorySprite;
            accessoryImg.SetNativeSize();
            accessoryImg.transform.localPosition = accessoryPos;

            hairImg.sprite = hairSprite;
            hairImg.SetNativeSize();
            hairImg.transform.localPosition = hairPos + new Vector3(462 - 40, 353 + 400 - 10, 0);

            smokeFx.Play();
        }

        protected override void InitItem()
        {
            canClick = true;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            DisableDance();
            Instantiate(clothingMode, GUIManager.instance.canvasSpawnMode.transform);

            DOVirtual.DelayedCall(1, () =>
            {
                canClick = true;
            });
        }
    }
}