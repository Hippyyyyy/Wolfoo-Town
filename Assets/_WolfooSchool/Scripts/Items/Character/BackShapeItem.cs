using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;

namespace _WolfooSchool
{
    public class BackShapeItem : BackItem
    {
        protected override void Start()
        {
            base.Start();
            startScale = transform.localScale;
            RenderData();
        }

        private void RenderData()
        {

        }
        public void AssignItem(Sprite sprite)
        {
            if (image == null) image = GetComponent<Image>();
            image.sprite = sprite;
            image.SetNativeSize();
            GameManager.instance.ScaleImage(image, 500, 500);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnPunchScale();
        }
    }
}