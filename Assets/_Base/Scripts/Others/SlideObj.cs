using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlideObj : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Action<PointerEventData> GetBeginDrag; 
    public Action<PointerEventData> GetDrag; 
    public Action<PointerEventData> GetEndDrag; 

    public void OnBeginDrag(PointerEventData eventData)
    {
   //     GetBeginDrag?.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        GetDrag?.Invoke(eventData);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }
    private void Update()
    {
#if UNITY_EDITOR
        if(Input.GetMouseButtonUp(0))
        {
            GetEndDrag?.Invoke(null);
        }
        if(Input.GetMouseButtonDown(0))
        {
            GetBeginDrag?.Invoke(null);
        }
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            // Handle finger movements based on TouchPhase
            switch (touch.phase)
            {
                case TouchPhase.Ended:
                    GetEndDrag?.Invoke(null);
                    break;
                case TouchPhase.Began:
                    GetBeginDrag?.Invoke(null);
                    break;
            }
        }
#endif
    }
}
