using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Type type;

    private Tweener scaleTween;
    private Vector3 startScale;

    public enum Type
    {
        Dance,
        Scale,
        Vibrate
    }
    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void Start()
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (scaleTween != null) scaleTween?.Kill();
        transform.localScale = startScale;

        switch (type)
        {
            case Type.Scale:
                scaleTween = transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 1, 0.5f);
                break;
            case Type.Dance:
                scaleTween = transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.5f, 1, 0.5f);
                break;
            case Type.Vibrate:
                scaleTween = transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.5f, 8, 0.1f);
                break;
        }
    }

}
