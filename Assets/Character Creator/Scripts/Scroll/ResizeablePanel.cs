using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ResizeablePanel : MonoBehaviour
    {
        public enum Direction { HORIZONTAL, VERTICAL }

        [SerializeField] bool _expanded = false;

        [SerializeField] float _expandedSize = 0f;

        [SerializeField] float _nonExpandedSize = 0f;

        [SerializeField] float _animTime = 1f;

        [SerializeField] Direction _direction = Direction.HORIZONTAL;

        [SerializeField] bool _reBuildNearestScrollRectParentDuringAnimation = false;

        float PreferredSize
        {
            get { return _direction == Direction.HORIZONTAL ? _LayoutElement.preferredWidth : _LayoutElement.preferredHeight; }
            set { if (_direction == Direction.HORIZONTAL) _LayoutElement.preferredWidth = value; else _LayoutElement.preferredHeight = value; }
        }

        LayoutElement _LayoutElement;

        ScrollRect _NearestScrollRectInParents;

        public static Action<bool> onExpandedStateChanged = null;

        void Start()
        {
            Canvas.ForceUpdateCanvases();
            _LayoutElement = GetComponent<LayoutElement>();

            if (_expanded) _expandedSize = (_expandedSize == -1f) ? PreferredSize : _expandedSize;
            else _nonExpandedSize = (_nonExpandedSize == -1f) ? PreferredSize : _nonExpandedSize;

            FindNearestScrollRectInParents();
        }
        void FindNearestScrollRectInParents()
        {
            Transform parent = transform.parent;
            while (parent != null && !_NearestScrollRectInParents)
            {
                _NearestScrollRectInParents = parent.GetComponent<ScrollRect>();
                parent = parent.parent;
            }
        }
        public void ToggleExpandedState()
        {
            bool expandedToSet = !_expanded;
            float from = PreferredSize;
            float to = expandedToSet ? _expandedSize : _nonExpandedSize;

            StartCoroutine(StartAnimating(from, to, () =>
            {
                _expanded = expandedToSet;
                onExpandedStateChanged?.Invoke(_expanded);
            }));
        }

        IEnumerator StartAnimating(float from, float to, System.Action onDone)
        {
            float startTime = Time.unscaledTime;
            float elapsed;
            float t01;

            do
            {
                yield return null;

                elapsed = Time.unscaledTime - startTime;
                t01 = Mathf.Clamp01(elapsed / _animTime);
                t01 = Mathf.Sqrt(t01);

                PreferredSize = Mathf.Lerp(from, to, t01);

                if (_reBuildNearestScrollRectParentDuringAnimation && _NearestScrollRectInParents)
                    _NearestScrollRectInParents.OnScroll(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
            }
            while (t01 < 1f);

            if (onDone != null)
                onDone();
        }



        /* public void ToggleExpandedState()
         {
             bool expandedToSet = !_expanded;
             float from = PreferredSize;
             float to = expandedToSet ? _expandedSize : _nonExpandedSize;

             StartAnimating(from, to, () =>
             {
                 _expanded = expandedToSet;
                 onExpandedStateChanged?.Invoke(_expanded);
             });
         }

         void StartAnimating(float from, float to, System.Action onDone)
         {
             float elapsed = 0f;

             // Sử dụng DOTween để tạo tween
             DOTween.To(() => 0f, t => elapsed = t, 1f, _animTime)
                 .OnUpdate(() =>
                 {
                     float t01 = Mathf.Sqrt(elapsed);
                     float newSize = Mathf.Lerp(from, to, t01);
                     PreferredSize = newSize;

                     if (_reBuildNearestScrollRectParentDuringAnimation && _NearestScrollRectInParents)
                     {
                         _NearestScrollRectInParents.OnScroll(new UnityEngine.EventSystems.PointerEventData(UnityEngine.EventSystems.EventSystem.current));
                     }
                 })
                 .OnComplete(() =>
                 {
                 // Gọi hàm callback khi hoàn thành tween
                 onDone?.Invoke();
                 })
                 .SetUpdate(true);
         }*/

    }
    
}
