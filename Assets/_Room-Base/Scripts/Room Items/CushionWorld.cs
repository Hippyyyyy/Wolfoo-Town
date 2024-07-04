using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CushionWorld : BackItemWorld
    {
        public override void Setup()
        {
            IsCarryItem = true;
            IsDragable = true;
            base.Setup();
        }
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            BedWorld.OnCushionVerified += OnDistanceVerified;
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            BedWorld.OnCushionVerified -= OnDistanceVerified;
        }

        private void OnDistanceVerified(BedWorld bed, CushionWorld arg2)
        {
            if(arg2 == this)
            {
                JumpTo(bed.CushionArea.position, () =>
                {
                    transform.SetParent(bed.transform);
                    LayerOrder = 0;
                });
            }
        }
    }
}
