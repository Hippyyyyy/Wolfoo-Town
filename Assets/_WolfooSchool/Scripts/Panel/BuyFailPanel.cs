using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class BuyFailPanel : Panel
    {
        [SerializeField] Button exitBtn;
        [SerializeField] Button cancelBtn;
        [SerializeField] Button getMore;

        protected override void Start()
        {
            exitBtn.onClick.AddListener(OnBack);
            cancelBtn.onClick.AddListener(OnBack);
            if (getMore != null) getMore.onClick.AddListener(OnGetMoreCoin);
        }

        private void OnEnable()
        {
            transform.SetAsLastSibling();
        }

        private void OnBack()
        {
            base.Hide();
        }

        private void OnGetMoreCoin()
        {
        }
    }
}