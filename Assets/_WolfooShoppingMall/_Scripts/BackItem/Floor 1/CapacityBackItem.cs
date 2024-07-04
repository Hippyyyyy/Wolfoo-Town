using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CapacityBackItem : BackItem
    {
        [SerializeField] protected Transform itemZone;
        [SerializeField] protected Transform[] limitZones;

        protected Edge[] edges;

        protected override void Start()
        {
            base.Start();
            edges = GameManager.instance.GetEdges(limitZones);
        }

        protected override void InitItem()
        {
        }
    }
}