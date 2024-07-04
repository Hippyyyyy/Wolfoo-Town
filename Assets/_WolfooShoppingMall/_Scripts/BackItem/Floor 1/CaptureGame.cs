using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CaptureGame : BackItem
    {
        [SerializeField] CaptureMode modePb;
        [SerializeField] Transform itemZone;
        [SerializeField] Transform[] moveZones;
        [SerializeField] Picture picture;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

        }
        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
        }

        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.captureMode != null)
            {
                for (int i = 0; i < obj.sprites.Count; i++)
                {
                    var item = Instantiate(picture, itemZone);
                    item.AssignItem(obj.sprites[i]);
                    OnReleasePicture(item, i - 0.5f);
                }
            }
        }
        void OnReleasePicture(Picture obj, float delayTime)
        {
            obj.transform.SetParent(itemZone);
            obj.transform.rotation = Quaternion.Euler(Vector3.forward * 90);
            obj.transform.position = moveZones[0].position;

            obj.transform.DOLocalMove(moveZones[1].localPosition, 1).SetEase(Ease.OutBounce);
            obj.transform.DORotate(Vector3.zero, 01f).OnComplete(() =>
            {
                obj.AssignItem();
                obj.enabled = true;
            });
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            Instantiate(modePb, GUIManager.instance.canvasSpawnMode.transform);

        }
    }
}