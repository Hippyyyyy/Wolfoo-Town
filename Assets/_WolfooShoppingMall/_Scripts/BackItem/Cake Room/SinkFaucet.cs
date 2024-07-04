using _WolfooShoppingMall;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SinkFaucet : BackItem
{
    [SerializeField] ParticleSystem waterFx;
    private int status;

    protected override void InitItem()
    {
        canClick = true;
        isScaleDown = true;
        scaleIndex = 0.5f;
    }
    protected override void Start()
    {
        base.Start();
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (!canClick) return;

        status = 1 - status;
        if(status == 1)
        {
            waterFx.Play();
        }
        else
        {
            waterFx.Stop();
        }
    }
}
