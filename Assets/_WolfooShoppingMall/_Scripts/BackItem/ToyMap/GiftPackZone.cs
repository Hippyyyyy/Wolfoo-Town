using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

namespace _WolfooShoppingMall
{
    public class GiftPackZone : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] GiftDecal giftDecalPb;
        [SerializeField] HorizontalLayoutGroup giftDecalsZone;
        [SerializeField] int totalDecal;

        private List<BackItem> curItems = new List<BackItem>();
        private List<GiftDecal> giftDecals = new List<GiftDecal>();
        private int curIdxDecal;
        bool canClick;
        private float distance;
        private Tweener floatTween;
        private GiftDecalData data;
        private Tween delayTween;
        private bool isGenerating;

        private void Start()
        {
            data = DataSceneManager.Instance.ItemDataSO.GiftDecalData;

            GenerateDecal();
        }

        void GenerateDecal(Action OnComplete = null)
        {
            if (giftDecals.Count > 0)
            {
                giftDecals.Clear();
                //  Debug.Log("End");
            }
            curIdxDecal = totalDecal - 1;
            giftDecalsZone.spacing = -650;

            for (int i = 0; i < totalDecal; i++)
            {
                int idx = i >= data.decalSprites.Length ? 0 : i;
                var giftDecal = Instantiate(giftDecalPb, giftDecalsZone.transform);
                
                if (giftDecal.IsFlower)
                {
                    giftDecal.AssignItem(i, data.decalSprites[idx], data.tiedFlowerSprites, DataSceneManager.Instance.ItemDataSO.smokeFxPb);
                }
                else
                {
                    giftDecal.AssignItem(i, data.decalSprites[idx], data.packageSprites[idx], DataSceneManager.Instance.ItemDataSO.smokeFxPb);
                }
                giftDecals.Add(giftDecal);
            }

            floatTween = DOVirtual.Float(-650, -380, 0.5f, (progress) =>
            {
                giftDecalsZone.spacing = progress;
            })
            .OnComplete(() =>
            {

                OnComplete?.Invoke();
            });
        }

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }

        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
        }

        private void GetEndDragBackItem(EventKey.OnEndDragBackItem obj)
        {
            BackItem curItem = null;
            if (obj.toy != null) curItem = obj.toy;
            if (obj.flower != null)
            {
                curItem = obj.flower;
                if (obj.flower.IsTied) return;
            }
            if (obj.peanut != null && obj.peanut.IsGrowth) curItem = obj.peanut;

            if (curItem == null) return;

            distance = Vector2.Distance(giftDecalsZone.transform.position, curItem.transform.position);
            if (distance < 2)
            {
                //canClick = true;
                //if (curItems.Contains(curItem)) return;
                //curItems.Add(curItem);

                if (isGenerating) return;

                if (giftDecals[curIdxDecal].IsFlower)
                {
                    giftDecals[curIdxDecal].OnPackaging(curItem,
                        transform,
                        UnityEngine.Random.Range(0, data.tiedFlowerSprites[curIdxDecal].tiedFlowerSprites.Length));
                }
                else
                {
                    giftDecals[curIdxDecal].OnPackaging(curItem, transform);
                }

                curIdxDecal--;
                if (curIdxDecal < 0)
                {
                    isGenerating = true;
                    delayTween = DOVirtual.DelayedCall(0.2f, () =>
                    {
                        GenerateDecal(() =>
                        {
                            isGenerating = false;
                        });
                    });
                }
            }
            else
            {
                //if (curItems.Contains(curItem))
                //{
                //    curItems.Remove(curItem);
                //}
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
        }
    }
}