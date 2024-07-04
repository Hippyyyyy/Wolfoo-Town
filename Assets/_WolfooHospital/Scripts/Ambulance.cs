using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Ambulance : BackItem
    {
        [SerializeField] Door myDoor;
        [SerializeField] Transform itemZone;
        [SerializeField] Table[] standZones;
        protected override void InitData()
        {
            base.InitData();
            myDoor.OnTouched += GetDoorTouched;
        }
        private void OnDestroy()
        {
            myDoor.OnTouched -= GetDoorTouched;
        }

        private void GetDoorTouched(Door door)
        {
            if (myDoor.IsOpen) { itemZone.gameObject.SetActive(true); }
            else { itemZone.gameObject.SetActive(false); }

            foreach (var stand in standZones)
            {
                stand.IsEnable = myDoor.IsOpen;
            }
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (!myDoor.IsOpen) return;

        }
    }
}