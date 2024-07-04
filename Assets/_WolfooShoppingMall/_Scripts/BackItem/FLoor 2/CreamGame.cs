using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class CreamGame : BackItem
    {
        [SerializeField] IceScreamMode screamMode;
        [SerializeField] Transform itemZone;
        [SerializeField] ParticleSystem rainbowFx;
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
            if (obj.iceScream != null)
            {
                obj.food.transform.SetParent(transform);
                obj.food.transform.position = itemZone.position;
                obj.food.transform.localScale = Vector3.one * 0.25f;
                obj.food.AssignDrag();

                rainbowFx.time = 0;
                rainbowFx.Play();
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            Instantiate(screamMode, GUIManager.instance.canvasSpawnMode.transform);

            Dancing(() =>
            {
                canClick = true;
            });
        }

        void Dancing(Action OnComplete)
        {
            transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.2f, 1).OnComplete(() =>
            {
                transform.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.2f, 1).OnComplete(() =>
                {
                    OnComplete?.Invoke();
                });
            });
        }
    }
}