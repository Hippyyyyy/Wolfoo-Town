using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class DicingGameItem : BackItem
    {
        [SerializeField] GameObject modePb;
        private GameObject curMode;

        protected override void InitItem()
        {
            canClick = true;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            curMode = Instantiate(modePb, GUIManager.instance.canvasSpawnMode.transform);
        }
    }
}