using _WolfooCity;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Base
{
    public class PopupManager : SingletonBind<PopupManager>
    {
        [SerializeField] UIPanel[] uIPanels;

        private void Start()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OpenPanel>(GetPanel);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            EventDispatcher.Instance.RemoveListener<EventKey.OpenPanel>(GetPanel);
        }

        private void GetPanel(EventKey.OpenPanel obj)
        {
            OpenPanel(obj.panelType);
        }

        public void OpenPanel(PanelType panelType)
        {
            foreach (var item in uIPanels)
            {
                if(item.PanelType == panelType)
                {
                    item.Show();
                }
            }
        }
    }
}
