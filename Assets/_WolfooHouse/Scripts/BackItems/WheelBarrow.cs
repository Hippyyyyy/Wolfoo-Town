using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class WheelBarrow : BackItem
    {
        [SerializeField] Transform tray;
        [SerializeField] Transform plantArea;
        private Edge[] trayArea;

        protected override void InitItem()
        {
            canDrag = true;
            base.InitItem();
        }
        protected override void Start()
        {
            base.Start();
            trayArea = GameManager.instance.GetEdges(tray.GetComponentsInChildren<Transform>());
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if(item.shovel != null)
            {
                if (item.shovel.HasSoil)
                {
                    if (GameManager.instance.Is_inside(item.shovel.transform.position, trayArea))
                    {
                        item.shovel.transform.SetParent(plantArea);
                        item.shovel.Pour(plantArea.position, plantArea);
                    }
                }
                else
                {
                    if (GameManager.instance.Is_inside(item.shovel.transform.position, trayArea))
                    {
                        var flowers = plantArea.GetComponentsInChildren<Flower>();
                        if (flowers.Length > 0)
                        {
                            foreach (var flower in flowers)
                            {
                                if (flower.IsGrowth)
                                {
                                    item.shovel.transform.SetParent(plantArea);
                                    item.shovel.Dig(plantArea.position, flower);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            item.shovel.transform.SetParent(plantArea);
                            item.shovel.Dig(plantArea.position);
                        }
                    }
                }
            }

            if(item.flower != null)
            {
                if (GameManager.instance.Is_inside(item.flower.transform.position, trayArea))
                {
                    item.flower.transform.SetParent(plantArea);
                    item.flower.JumpToEndLocalPos(item.flower.transform.localPosition);
                }
            }
        }
    }
}
