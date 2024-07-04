using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class FlowerGround : BackItem
    {
        [SerializeField] Transform[] flowerAreas;
        [SerializeField] Flower flowerPb;
        private int countIdx;

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if(item.fertilizer != null)
            {
                var verifiedIdx = GetClosetPotIdx(item.fertilizer.transform);
                if(verifiedIdx > -1)
                {
                    PlantingFlower(item.fertilizer, verifiedIdx);
                }
            }

            if(item.waterProvider != null)
            {
                var verifiedIdx = GetClosetPotIdx(item.waterProvider.WaterPouringArea);
                if (verifiedIdx > -1)
                {
                    GrowthPlant(item.waterProvider, verifiedIdx);
                }
            }

            if(item.shovel != null)
            {
                if(item.shovel.HasSoil)
                {
                    var idx = GetClosetPotIdx(item.shovel.transform);
                    if (idx >= 0 && flowerAreas[idx])
                    {
                        item.shovel.transform.SetParent(transform);
                        item.shovel.Pour(transform.position, transform);
                    }
                }
                else
                {
                    var idx = GetClosetPotIdx(item.shovel.transform);
                    if (idx >= 0 && flowerAreas[idx])
                    {
                        var flowers = flowerAreas[idx].GetComponentsInChildren<Flower>();
                        if (flowers.Length > 0)
                        {
                            foreach (var flower in flowers)
                            {
                                if (flower.IsGrowth)
                                {
                                    flowerAreas[idx].SetAsLastSibling();
                                    item.shovel.transform.SetParent(flowerAreas[idx]);
                                    item.shovel.Dig(transform.position, flower);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            item.shovel.transform.SetParent(transform);
                            item.shovel.Dig(transform.position);
                        }
                    }
                }
            }
        }


        private int GetClosetPotIdx(Transform item)
        {
            float verifiedDistance = 1;
            var verifiedIdx = -1;
            for (int i = 0; i < flowerAreas.Length; i++)
            {
                var distance = Vector2.Distance(item.position, flowerAreas[i].position);
                if (distance < verifiedDistance)
                {
                    verifiedDistance = distance;
                    verifiedIdx = i;
                }
            }

            return verifiedIdx;
        }
        private void GrowthPlant(WaterProvider waterProvider, int potIdx)
        {
            var flowerArea = flowerAreas[potIdx];
            waterProvider.transform.SetParent(flowerArea);
            waterProvider.PouringWater(flowerArea.position, () =>
            {
                var flowers = flowerAreas[potIdx].GetComponentsInChildren<Flower>();
                foreach (var flower in flowers)
                {
                    flower.Growth();
                }
            });
        }
        private void PlantingFlower(Fertilizer fertilizer, int potIdx)
        {
            var flowerArea = flowerAreas[potIdx];
            fertilizer.transform.SetParent(flowerArea);
            fertilizer.Pouring(flowerArea.position, (spriteData) =>
            {
                var flower = Instantiate(flowerPb, flowerArea);
                flower.AssginToPeanut(spriteData, fertilizer is FruitFertilizer);
            });
        }

        private void GrowthAllPlant(WaterProvider waterProvider, bool isBegining)
        {
            countIdx = isBegining ? 0 : countIdx;
            var flowerArea = flowerAreas[countIdx];
            waterProvider.transform.SetParent(flowerArea);
            waterProvider.PouringWater(flowerArea.position, () =>
            {
                var flowers = flowerAreas[countIdx].GetComponentsInChildren<Flower>();
                foreach (var flower in flowers)
                {
                    flower.Growth();
                }
                countIdx++;
                if (countIdx == flowerAreas.Length)
                {
                    return;
                }

                GrowthAllPlant(waterProvider, false);
            });
        }

        private void PlantingAllFlower(Fertilizer fertilizer, bool isBegining)
        {
            countIdx = isBegining ? 0 : countIdx;
            var flowerArea = flowerAreas[countIdx];
            fertilizer.transform.SetParent(flowerArea);
            fertilizer.Pouring(flowerArea.position, (spriteData) =>
            {
                var flower = Instantiate(flowerPb, flowerArea);
                flower.AssginToPeanut(spriteData, fertilizer is FruitFertilizer);
                countIdx++;
                if (countIdx == flowerAreas.Length)
                {
                    return;
                }

                PlantingAllFlower(fertilizer, false);
            });
        }
    }
}
