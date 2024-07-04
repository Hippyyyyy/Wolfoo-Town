using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class NewShelf : BackItem
    {
        [SerializeField] Transform[] limitZones;
        [SerializeField] Transform[] limitZones2;
        [SerializeField] Transform[] compareZones;
        private Edge[] myEdges1;
        private Edge[] myEdges2;

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.news != null)
            {
                for (int i = 0; i < compareZones.Length; i++)
                {
                    if (Vector2.Distance(compareZones[i].position, item.news.transform.position) < 2)
                    {
                        if (GameManager.instance.Is_inside(item.news.transform.position, i == 0 ? myEdges1 : myEdges2))
                        {
                            item.news.transform.SetParent(compareZones[i]);
                            item.news.JumpToEndLocalPos(Vector3.zero, null, DG.Tweening.Ease.OutBounce, 50, false, assignPriorityy);
                            item.news.Fold();
                            return;
                        }
                    }
                }
            }
        }

        protected override void InitData()
        {
            base.InitData();
            myEdges1 = GameManager.instance.GetEdges(limitZones);
            myEdges2 = GameManager.instance.GetEdges(limitZones2);
        }
    }
}