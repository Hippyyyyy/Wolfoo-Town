using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class ToiletWorld : TransformClickItemWorld
    {
        [SerializeField] bool canTransform = true;
        [SerializeField] DoorWorld myDoor;
        [SerializeField] SeatWorld mySit;
        private bool isOpen = true;

        public override void Setup()
        {
            base.Setup();
            CanTransform = canTransform;

            if (myDoor != null)
            {
                AssignMyElement();
            }
        }
        protected override void GetBeginDragBackItem(BackItemWorld obj)
        {
            base.GetBeginDragBackItem(obj);
            if (obj.IsCharacter)
            {
                mySit.ReleaseItem(obj);
            }
        }
        protected override void GetEndDragBackItem(BackItemWorld obj)
        {
            base.GetEndDragBackItem(obj);

            if (obj.IsCharacter)
            {
                var isSuccess = mySit.PutItem(obj);
                if(isSuccess)
                {
                    EventDispatcher.Instance.Dispatch(new RoomEventKey.Chair
                    {
                        seat = mySit,
                        newCharacter = obj.GetComponent<CharacterWorld>(),
                        wolfooWorld = obj.GetComponent<CharacterWolfooWorld>()
                    });
                }
            }
        }
        protected override void OnKill()
        {
            base.OnKill();
            if (myDoor != null)
            {
                myDoor.OnChangeState -= OnDoorClicked;
                mySit.Assign(myDoor.IsOpen);
            }
        }
        private void AssignMyElement()
        {
            myDoor.OnChangeState += OnDoorClicked;

            mySit.Assign(myDoor.IsOpen);
            mySit.gameObject.SetActive(myDoor.IsOpen);
        }

        private void OnDoorClicked(bool isOpen)
        {
            mySit.Assign(myDoor.IsOpen);
            mySit.gameObject.SetActive(isOpen);
        }
        protected override void OnClick()
        {
            base.OnClick();

            isOpen = !isOpen;
            ChangeState(isOpen, false);
        }
    }
}
