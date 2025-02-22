﻿

using _WolfooCity;
using UnityEngine;
/** <summary> Base Panel in UI</summary> */
public class Panel : MonoBehaviour {
    [SerializeField] protected bool tapToHide = true;

#if UNITY_ANDROID
	[SerializeField] protected bool physicBackEnable = true;
#endif

    protected bool duplicated = false;
    protected UIPanel uiPanel;

    protected virtual void Awake()
    {
        if (uiPanel == null) uiPanel = GetComponentInChildren<UIPanel>(true);
    }

    protected virtual void Start() {

    }

    public virtual void Show(object data = null, bool duplicated = false) {
        this.duplicated = duplicated;
        gameObject.SetActive(true);
        uiPanel = GetComponentInChildren<UIPanel>(true);
    }

    public virtual void Hide(object data = null) {
        if (duplicated) Destroy(gameObject);
        else gameObject.SetActive(false);
    }

    public virtual void Back() {
#if UNITY_ANDROID
		if (PhysicBackEnable) Hide();
#endif
    }

    public virtual bool PhysicBackEnable {
        get {
#if UNITY_ANDROID
            return physicBackEnable;
#else
            return false;
#endif
        }
    }
}