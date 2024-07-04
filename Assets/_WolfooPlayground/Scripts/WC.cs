using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class WC : BackItem
    {
        [SerializeField] Door door;
        [SerializeField] Door toiletLid;
        [SerializeField] Transform sitZone;
        [SerializeField] Table standZonee;
        [SerializeField] ParticleSystem lightingFx;
        [SerializeField] Transform indoorArea;

        private float distance_;
        private BackItem myItem;

        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if(item.character != null && myItem != null)
            {
                if (item.character == myItem) myItem = null;
            }
            if(item.newCharacter != null && myItem != null)
            {
                if (item.newCharacter == myItem) myItem = null;
            }
        }
        protected override void InitData()
        {
            base.InitData();
           standZonee.IsEnable = door.IsOpen;
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            door.OnTouched += GetDoorTouched;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            door.OnTouched -= GetDoorTouched;
        }

        private void GetDoorTouched(Door door)
        {
            indoorArea.gameObject.SetActive(door.IsOpen);
            standZonee.IsEnable = door.IsOpen;
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.character != null)
            {
                if (!door.IsOpen) return;
                if(toiletLid.IsOpen)
                {
                    if (myItem != null) return;
                    distance_ = Vector2.Distance(item.character.transform.position, sitZone.position);
                    if (distance_ <= 1)
                    {
                        myItem = item.character;
                        item.character.OnSitToChair(sitZone.position, sitZone, true);
                        lightingFx.Play();
                    }
                }
            }
            if (item.newCharacter != null)
            {
                if (!door.IsOpen) return;
                if(toiletLid.IsOpen)
                {
                    if (myItem != null) return;
                    distance_ = Vector2.Distance(item.newCharacter.transform.position, sitZone.position);
                    if (distance_ <= 1)
                    {
                        myItem = item.newCharacter;
                        item.newCharacter.OnSitToChair(sitZone.position, sitZone, true);
                        lightingFx.Play();
                    }
                }
            }
        }
    }
}