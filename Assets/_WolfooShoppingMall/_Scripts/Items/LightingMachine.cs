using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LightingMachine : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Sprite[] statusSprites;
    [SerializeField] bool CanClick;
    
    SpriteRenderer spriteRenderer;
    private int curIdx;
    private Tween delayTween;
    private Tween delayTween2;
    private Image image;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
            spriteRenderer.sprite = statusSprites[1];

        image = GetComponent<Image>();
        if(image != null)
        {
            image.sprite = statusSprites[curIdx];
            image.SetNativeSize();
        }
    }
    private void OnDestroy()
    {
        if (delayTween != null) delayTween?.Kill();
        if (delayTween2 != null) delayTween2?.Kill();
    }

    public void OnTwinkling(System.Action OnComplete = null)
    {
        if (delayTween != null) delayTween?.Kill();
        if (delayTween2 != null) delayTween2?.Kill();

        delayTween2 = DOVirtual.DelayedCall(0.1f, () =>
        {
            curIdx = 1 - curIdx;
            if (spriteRenderer != null)
                spriteRenderer.sprite = statusSprites[curIdx];
            if (image != null)
            {
                image.sprite = statusSprites[curIdx];
                image.SetNativeSize();
            }
        }).SetLoops(-1);

        delayTween = DOVirtual.DelayedCall(2, () =>
        {
            if (delayTween2 != null) delayTween2?.Kill();
            OnComplete?.Invoke();   
        });
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!CanClick) return;

        CanClick = false;
        OnTwinkling(() =>
        {
            CanClick = true;
        });
    }
}
