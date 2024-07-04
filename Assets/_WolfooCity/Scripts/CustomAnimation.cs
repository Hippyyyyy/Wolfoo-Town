using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAnimation : MonoBehaviour
{
    [SerializeField] string animName = "House-Dancing";
    [SerializeField] float delayTimePlay;
    private Animator _anim;
    private Tween _tween;

    private void Start()
    {
        _anim = GetComponent<Animator>();
        _anim.enabled = false;
        _tween = DOVirtual.DelayedCall(delayTimePlay, () =>
        {
            _anim.enabled = true;
            _anim.Play(animName, 0, 0);
        });
    }
    private void OnDestroy()
    {
        if (_tween != null) _tween?.Kill();
    }

    public void OnDanced()
    {
        _anim.enabled = false;
        _tween = DOVirtual.DelayedCall(delayTimePlay, () =>
        {
            _anim.enabled = true;
        });
    }
}
