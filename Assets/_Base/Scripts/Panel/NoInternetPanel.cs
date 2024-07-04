using _Base;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class NoInternetPanel : Panel
    {
        private bool isShowing;

        public void OnPressTryagain()
        {
            uiPanel.Hide(() =>
            {
                base.Hide();
                isShowing = false;
            });
        }
        public override void Hide(object data = null)
        {
            uiPanel.Hide(() =>
            {
                base.Hide();
            });
        }
        public void Show()
        {
            if (isShowing) return;
            isShowing = true;
            Invoke("OnShowing", 5);
        }
        private void OnShowing()
        {
            base.Show();
            uiPanel.Show();
        }
    }
}