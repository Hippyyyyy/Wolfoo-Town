using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ClawmachineGame : BackItem
    {
        [SerializeField] ClawMachineMode modePb;
        [SerializeField] Transform itemZone;
        [SerializeField] Transform[] moveZones;
        [SerializeField] Toy toyPb;
        List<int> curIdToys = new List<int>();
        private MachineToyData myData;

        protected override void InitItem()
        {
            canClick = true;
            isDance = true;
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void InitData()
        {
            base.InitData();
            myData = DataSceneManager.Instance.ItemDataSO.MachineToyData;
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener<EventKey.OnSuccess>(GetSuccess);
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener<EventKey.OnSuccess>(GetSuccess);
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
        }

        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.clawMachineMode != null && curIdToys.Count > 0)
            {
                for (int i = 0; i < curIdToys.Count; i++)
                {
                    var toy = Instantiate(toyPb, itemZone);
                    toy.transform.position = moveZones[0].position;
                    toy.transform.localScale = Vector3.zero;
                    toy.AssignItem(myData.toySprites[curIdToys[i]]);

                    toy.transform.SetParent(itemZone);
                    toy.transform.DOScale(0.6f, 0.5f).SetDelay(i);
                    toy.transform.DOLocalMove(moveZones[1].localPosition, 0.5f).SetEase(Ease.Linear).SetDelay(i)
                    .OnComplete(() =>
                    {
                        toy.transform.DOLocalMove(moveZones[2].localPosition, 0.5f).SetEase(Ease.OutBounce)
                        .OnComplete(() =>
                        {
                            toy.AssignItem();
                            toy.enabled = true;
                            canClick = true;
                        });
                    });
                }
                curIdToys.Clear();
                //  Destroy(obj.clawMachineMode.gameObject);
            }
        }

        private void GetSuccess(EventKey.OnSuccess obj)
        {
            if (obj.toy != null)
            {
                obj.toy.transform.SetParent(transform);
                curIdToys.Add(obj.id);
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;

            Instantiate(modePb, GUIManager.instance.canvasSpawnMode.transform.parent);

        }
    }
}