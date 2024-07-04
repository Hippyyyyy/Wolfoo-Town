using SCN.Common;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpandedSizeItem : MonoBehaviour
{
    [SerializeField] Button btn;
    [SerializeField] bool isClick = false;
    [SerializeField] List<GameObject> listGameObj;
    RectTransform rect;
    [SerializeField] LayoutElement layoutElement;
    public enum Direction { HORIZONTAL, VERTICAL }

    [SerializeField] bool _expanded = false;

    [SerializeField] float _expandedSize = 0f;

    [SerializeField] float _nonExpandedSize = 0f;

    [SerializeField] float _widthSize = 0f;

    [SerializeField] float _nonWidthSize = 0f;

    [SerializeField] float _animTime = 1f;

    [SerializeField] Direction _direction = Direction.HORIZONTAL;

    [SerializeField] bool _reBuildNearestScrollRectParentDuringAnimation = false;

    float PreferredSize
    {
        get { return _direction == Direction.HORIZONTAL ? layoutElement.preferredWidth : layoutElement.preferredHeight; }
        set { if (_direction == Direction.HORIZONTAL) layoutElement.preferredWidth = value; else layoutElement.preferredHeight = value; }
    }

    public bool IsClick { get => isClick; set => isClick = value; }

    [SerializeField] ScrollRect _NearestScrollRectInParents;

    private void Start()
    {

    }
    public void Init()
    {
        btn.onClick.AddListener(PointerDown);
    }
    public void Remove()
    {
        btn.onClick.RemoveListener(PointerDown);
    }




    private void PointerDown()
    {
        if (!IsClick)
        {
            CharacterPaletteController.Ins.ClickItemToShow();
            IsClick = !IsClick;
            ToggleExpandedState();
            Remove();
        }
    }

    public void ToggleExpandedState()
    {
        bool expandedToSet = !_expanded;
        float from = PreferredSize;
        float to = expandedToSet ? _expandedSize : _nonExpandedSize;
        float time = expandedToSet ? 0.5f : 0.2f;
        float width = expandedToSet ? _widthSize : _nonWidthSize;
        StartCoroutine(StartAnimating(from, to, () =>
        {
            _expanded = expandedToSet;
            Init();
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

    public void HideItem()
    {
        float from = PreferredSize;
        float to = _nonExpandedSize;
        float width = _nonWidthSize;
        StartCoroutine(StartAnimating(from, to, () =>
        {
            _expanded = false;
            isClick = false;
        }));
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (btn == null) btn = transform.GetChild(1).GetComponent<Button>();
        if (layoutElement == null) layoutElement = transform.GetComponent<LayoutElement>();
    }
#endif
}
