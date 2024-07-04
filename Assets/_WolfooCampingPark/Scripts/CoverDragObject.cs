using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall.Minigame.DrawingPicture
{
    public class CoverDragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public Action BeginDrag;
        public Action Drag;
        public Action EndDrag;

        public void OnBeginDrag(PointerEventData eventData)
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            Drag?.Invoke();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            BeginDrag?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            EndDrag?.Invoke();
        }
    }
}
