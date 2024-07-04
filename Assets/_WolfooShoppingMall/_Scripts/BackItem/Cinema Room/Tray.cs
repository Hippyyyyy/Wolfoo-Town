using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Tray : BackItem
    {
        [SerializeField] Transform plasticZone;
        [SerializeField] Transform lidZone;

        private int countPlasticPos;
        private float distance;
        private int countLidPos;

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem obj)
        {
            if (obj.plasticup != null)
            {
                if (obj.plasticup.IsHasWater) return;
                distance = Vector2.Distance(obj.plasticup.transform.position, plasticZone.position);
                if (distance <= 1)
                {
                    CheckPriority(() =>
                    {
                        for (int i = 0; i < plasticZone.childCount; i++)
                        {
                            if (plasticZone.GetChild(i).childCount == 0)
                            {
                                obj.plasticup.OnRotateDownTo(Vector3.zero, plasticZone.GetChild(i));
                                return;
                            }
                        }
                    });
                }
            }
            if (obj.beverageLid != null)
            {
                distance = Vector2.Distance(obj.beverageLid.transform.position, lidZone.position);
                if (distance <= 1)
                {
                    CheckPriority(() =>
                    {
                        for (int i = 0; i < lidZone.childCount; i++)
                        {
                            if (lidZone.GetChild(i).childCount == 0)
                            {
                                obj.beverageLid.transform.SetParent(lidZone.GetChild(i));
                                obj.beverageLid.JumpToEndLocalPos(Vector3.zero,
                                    null,
                                    DG.Tweening.Ease.Flash);
                                return;
                            }
                        }
                    });
                }
            }
        }

        protected override void InitItem()
        {
        }
    }

}