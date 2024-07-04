using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class CommingSoonPanel : Panel
    {
        [SerializeField] Button exitBtn;

        protected override void Start()
        {
            exitBtn.onClick.AddListener(OnBack);
        }

        private void OnBack()
        {
            base.Hide();
        }
    }
}