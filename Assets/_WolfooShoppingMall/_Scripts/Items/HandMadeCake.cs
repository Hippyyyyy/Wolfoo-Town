using _WolfooShoppingMall;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandMadeCake : MonoBehaviour
{
    [SerializeField] Image[] itemImgs;
    [SerializeField] List<Image> hintImg = new List<Image>();

    private Cake cake;
    private Tweener fadeTween;

    public Image[] ItemImgs { get => itemImgs; }

    private void Awake()
    {
        cake = GetComponent<Cake>();
        var count = 0;
        foreach (var item in itemImgs)
        {
            item.transform.localScale = Vector3.zero;
            hintImg[count].DOFade(0, 0);
            count++;
       //     hintImg.Add(item.transform.parent.GetComponent<Image>());
        }
    }
    private void Start()
    {
    }
    private void OnDestroy()
    {
        if (fadeTween != null) fadeTween?.Kill();
    }

    public void AssignItem(Sprite sprite, int idx)
    {
        itemImgs[idx].sprite = sprite;
        itemImgs[idx].SetNativeSize();
    }

    public void AssignCake()
    {
        cake.enabled = true;
        cake.Assign();
        enabled = false;
    }

    public void GetHint(int idx)
    {
        if (fadeTween != null) fadeTween?.Kill();
        hintImg[idx].DOFade(0, 0);
        fadeTween = hintImg[idx].DOFade(0.8f, 0.5f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
    public void OnColoring(int idx)
    {
        if (fadeTween != null) fadeTween?.Kill();
        hintImg[idx].DOFade(0, 0.25f);
    }
}
