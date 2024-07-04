using _WolfooShoppingMall;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class FloorButton : MonoBehaviour
    {
        public void OnChangeFloor(int index = -1)
        {
            if (index == -1)
            {
                GUIManager.instance.OpenPanel(PanelType.Elevator);
            }
        }
    }
}