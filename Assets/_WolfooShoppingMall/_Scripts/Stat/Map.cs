using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Map : Floor
    {
        [SerializeField] DollToyMap doll;
        [SerializeField] PanelType floorPanelType;
        private ItemsDataSO data;
        private DollClothingData clothingData;

        protected override void Start()
        {
            base.Start();
            data = DataSceneManager.Instance.ItemDataSO;

            if (doll != null)
            {
                clothingData = data.DollClothingData;
                doll.AssignItem(clothingData.dressTopicData[clothingData.dollClothingDicts[0].curItemIdx],
                    clothingData.accessoryTopicData[clothingData.dollClothingDicts[1].curItemIdx],
                   clothingData.eyeHairTopicData[clothingData.dollClothingDicts[2].curItemIdx],
                   clothingData.accessoryPosData[clothingData.dollClothingDicts[1].curItemIdx],
                   clothingData.hairPosData[clothingData.dollClothingDicts[2].curItemIdx],
                   clothingData.dressPosData[clothingData.dollClothingDicts[0].curItemIdx]);
            }
            playerPanel.AssignBackFloor(floorPanelType, panelType);
        }
        protected override void OnEnable()
        {
            base.OnEnable();
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(InitDataCompleted);
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(InitDataCompleted);
        }
        void InitDataCompleted()
        {
        }

        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.dollClothing != null)
            {
                clothingData = data.DollClothingData;

                doll.AssignItem(clothingData.dressTopicData[clothingData.dollClothingDicts[0].curItemIdx],
                    clothingData.accessoryTopicData[clothingData.dollClothingDicts[1].curItemIdx],
                   clothingData.eyeHairTopicData[clothingData.dollClothingDicts[2].curItemIdx],
                   clothingData.accessoryPosData[clothingData.dollClothingDicts[1].curItemIdx],
                   clothingData.hairPosData[clothingData.dollClothingDicts[2].curItemIdx],
                   clothingData.dressPosData[clothingData.dollClothingDicts[0].curItemIdx]);
            }
        }
    }
}