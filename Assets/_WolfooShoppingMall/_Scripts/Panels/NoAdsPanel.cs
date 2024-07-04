using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace _WolfooShoppingMall
{
    public class NoAdsPanel : Panel
    {
        [SerializeField] Button closeBtn;

        protected override void Start()
        {
            base.Start();

            closeBtn.onClick.AddListener(() =>
            {
                uiPanel.Hide(() =>
                {
                    base.Hide();
                });
            });
        }
    }
}