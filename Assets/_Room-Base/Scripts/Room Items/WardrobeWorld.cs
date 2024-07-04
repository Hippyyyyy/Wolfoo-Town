using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class WardrobeWorld : BackItemWorld
    {
        [SerializeField] DoorWorld[] doors;
        private TableWorld[] tables;

        private bool isClosedAllDoor;

        public override void Setup()
        {
            IsClick = true;
            if (!IsAssigned)
            {
                tables = GetComponentsInChildren<TableWorld>(true);
            }
            base.Setup();
        }
        protected override void OrderLayerIndex(bool isMax = false)
        {
            base.OrderLayerIndex(isMax);
            foreach (var door in doors)
            {
                door.LayerOrder = LayerOrder + 10;
            }
        }
        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            foreach (var door in doors)
            {
                door.OnChangeState += OnDoorChanged;
            }
            EventSelfHouseRoom.OnClickDecorOption += OnClickDecorOption;
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            foreach (var door in doors)
            {
                door.OnChangeState -= OnDoorChanged;
            }
            EventSelfHouseRoom.OnClickDecorOption -= OnClickDecorOption;
        }

        private void OnClickDecorOption(bool isOpen)
        {
            Debug.Log("OnClickDecorOption");
            foreach (var table in tables)
            {
                foreach (var customItem in table.GetComponentsInChildren<CustomRoomItem>())
                {
                    if (isOpen)
                    {
                        customItem.Enable();
                    }
                    else
                    {
                        customItem.Disable();
                    }
                }
            }
        }

        private void OnDoorChanged(bool isOpen)
        {
            isClosedAllDoor = true;
            foreach (var door in doors)
            {
                isClosedAllDoor = !door.IsOpen;
                if (!isClosedAllDoor) break;
            }

            foreach (var table in tables)
            {
                table.gameObject.SetActive(!isClosedAllDoor);
            }
        }

        protected override void OnClick()
        {
            base.OnClick();

            foreach (var door in doors)
            {
                door.ForceClick(!isClosedAllDoor);
            }
        }
    }
}
