using _WolfooShoppingMall;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Speaker : BackItem
{
    [SerializeField] ParticleSystem musicFx;
    private bool isTurnOn;

    protected override void InitItem()
    {
        canClick = true;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        if (!canClick) return;

        isTurnOn = !isTurnOn;
        OnCheck();
    }
    void OnCheck()
    {
        if(isTurnOn)
        {
         //   SoundManager.instance.MuteSound(SoundType.Music);
            musicFx.Play();

            if (tweenScale != null) tweenScale?.Kill();
            transform.localScale = startScale;
            tweenScale = transform.DOPunchScale(new Vector3(0.1f, -.1f, 0), 1, 3)
                .SetLoops(-1, LoopType.Restart);
        }
        else
        {
        //    SoundManager.instance.EnableSound(SoundType.Music);
            musicFx.Stop();
            if (tweenScale != null) tweenScale?.Kill();
            transform.localScale = startScale;
        }
    }
    public void PlayMusic()
    {
        isTurnOn = true;
        OnCheck();
    }
    public void StopMusic()
    {
        isTurnOn = false;
        OnCheck();
    }
}
