using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class OperaClothesManagerItem : MonoBehaviour
    {
        [SerializeField] Clothing clothing;
        [SerializeField] OperaClothesScrollItem scrollItem;
        private Edge[] packageEdges;
        private bool isInsidePackeEdge;

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragItem>(GetEndDragItem);
        }
        private void OnDisable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragItem>(GetEndDragItem);
        }

        public void Setup(Transform packageArea)
        {
            Transform[] area = new Transform[packageArea.childCount];
            for (int i = 0; i < packageArea.childCount; i++)
            {
                area[i] = packageArea.GetChild(i).transform;
            }
            packageEdges = GameManager.instance.GetEdges(area);
        }

        public void SetInsideToPackage()
        {
            clothing.enabled = false;
            scrollItem.enabled = true;
            clothing.KillDragging();
        }
        public void SetOutsideToPackage()
        {
            clothing.enabled = true;
            scrollItem.enabled = false;
            clothing.MoveToGround();
        }

        private void GetEndDragItem(EventKey.OnEndDragItem item)
        {
            if (item.operaClothesItem == null) return;

            isInsidePackeEdge = GameManager.instance.Is_inside(transform.position, packageEdges);
            if(!isInsidePackeEdge)
            {
                SetOutsideToPackage();
            }
        }

        private void GetEndDragBackItem(EventKey.OnEndDragBackItem item)
        {
            if (item.clothing != null) return;

            isInsidePackeEdge = GameManager.instance.Is_inside(transform.position, packageEdges);
            if (isInsidePackeEdge)
            {
                SetInsideToPackage();
            }
        }
    }
}