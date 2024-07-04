using _WolfooShoppingMall;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingToy : BackItem
{
    [SerializeField] Transform[] items;
    [SerializeField] Transform[] masks;
    private int curIdx;

    protected override void InitItem()
    {
        canClick = true;
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (!canClick) return;
        canClick = false;

        curIdx = 0;

        transform.DOPunchScale(new Vector3(0.1f, -0.1f, 0), 0.5f, 1, 0.5f);
        DOVirtual.DelayedCall(0.25f, () =>
        {
            OnPlaying(() =>
            {
                canClick = true;
            });
        });
    }

    void OnPlaying(System.Action OnComplete)
    {
        if(curIdx == items.Length)
        {
            OnComplete?.Invoke();
            return;
        }
        masks[curIdx].DOPunchPosition(Vector3.up * 100, 0.5f, 1);
        items[curIdx].DOPunchPosition(Vector3.up * 100, 0.5f, 1);
        DOVirtual.DelayedCall(0.1f, () =>
        {
            OnPlaying(OnComplete);
        });
        curIdx++;
    }
}
