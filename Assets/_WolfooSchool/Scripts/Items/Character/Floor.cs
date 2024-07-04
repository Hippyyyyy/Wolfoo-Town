using _Base;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace _WolfooSchool
{
    public class Floor : BackItem
    {
        [SerializeField] ScrollRect scrollRect;
        [SerializeField] PanelType panelType;
        private void Awake()
        {
            scrollRect.content.gameObject.SetActive(false);
        }

        protected override void Start()
        {
            base.Start();
            skinType = SkinBackItemType.Floor;
        }
        private void OnDestroy()
        {
        }

    }
}