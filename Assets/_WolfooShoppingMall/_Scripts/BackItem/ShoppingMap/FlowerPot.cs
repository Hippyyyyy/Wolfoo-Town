using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class FlowerPot : BackItem
    {
        [SerializeField] ParticleSystem starFx;
        [SerializeField] Transform flowerZone;
        [SerializeField] Flower flowerPb;

        private List<Peanut> peanuts = new List<Peanut>();

        private void PlantingFlower(Fertilizer fertilizer)
        {
            fertilizer.transform.SetParent(flowerZone);
            fertilizer.Pouring(flowerZone.position, (spriteData) =>
            {
                var flower = Instantiate(flowerPb, flowerZone);
                flower.AssginToPeanut(spriteData, fertilizer is FruitFertilizer);
            });
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (item.peanut != null)
            {
                if (item.peanut.IsGrowth) return;
                if (Vector2.Distance(item.peanut.transform.position, flowerZone.position) > 2) return;
                item.peanut.OnJUmpToPot(flowerZone.position, flowerZone, () =>
                {
                    if (!item.peanut.IsGrowth)
                        peanuts.Add(item.peanut);

                    starFx.time = 0;
                    starFx.Play();

                    KillScalling();
                    tweenScale = transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.5f, 2);
                });
            }
            if (item.flower != null)
            {
                if (item.flower.IsTied) return;
                if (Vector2.Distance(item.flower.transform.position, flowerZone.position) > 1) return;

                if (!item.flower.IsGrowth)
                {
                    item.flower.OnJUmpToPot(flowerZone.position, flowerZone, () =>
                    {
                        starFx.time = 0;
                        starFx.Play();

                        KillScalling();
                        tweenScale = transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.5f, 2);
                    });
                }
                else
                {
                    var endPos = item.flower.GetImage().rectTransform.sizeDelta.y / 2;
                    item.flower.OnJUmpToPot(Vector3.up * endPos, flowerZone);
                    starFx.Play();
                }
            }

            if (item.waterProvider != null)
            {
                if (Vector2.Distance(flowerZone.position, item.waterProvider.transform.position) > 1 &&
                    Vector2.Distance(item.waterProvider.WaterPouringArea.position, flowerZone.position) > 1) return;

                if (!item.waterProvider.IsNewModel)
                {
                    item.waterProvider.OnPourWater(transform.localPosition, transform.parent, () =>
                    {
                        foreach (var peanut in peanuts)
                        {
                            if (peanut.IsGrowth) return;
                            peanut.OnGrowth(() =>
                            {
                                peanuts.Remove(peanut);
                            });
                        }
                    });
                }
                else
                {
                    item.waterProvider.transform.SetParent(flowerZone);
                    item.waterProvider.PouringWater(flowerZone.position, () =>
                    {
                        foreach (var flower in flowerZone.GetComponentsInChildren<Flower>())
                        {
                            flower.Growth();
                        }
                    });
                }
            }

            if (item.fertilizer != null)
            {
                if (Vector2.Distance(flowerZone.position, item.fertilizer.transform.position) < 1)
                {
                    PlantingFlower(item.fertilizer);
                }
                return;
            }

            if (item.shovel != null)
            {
                if (Vector2.Distance(flowerZone.position, item.shovel.transform.position) < 1)
                {
                    if (item.shovel.HasSoil)
                    {
                        item.shovel.transform.SetParent(flowerZone);
                        item.shovel.Pour(flowerZone.position, flowerZone);
                    }
                    else
                    {
                        var flowers = flowerZone.GetComponentsInChildren<Flower>();
                        if (flowers.Length > 0)
                        {
                            foreach (var flower in flowers)
                            {
                                if (flower.IsGrowth)
                                {
                                    item.shovel.transform.SetParent(flowerZone);
                                    item.shovel.Dig(flowerZone.position, flower);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            item.shovel.transform.SetParent(flowerZone);
                            item.shovel.Dig(flowerZone.position);
                        }
                    }
                }
                return;
            }
        }
    }
}