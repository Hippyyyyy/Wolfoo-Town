using _Base;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class ElevatorPanel : Panel
    {
        [SerializeField] Button backBtn;
        [SerializeField] ElevatorPanelType[] panels;

        Button[] buttons;
        Image[] impressionImgs;

        [System.Serializable]
        public struct ElevatorPanelType
        {
            public CityType cityType;
            public GameObject obj;
            public Button[] buttons;
            public Image[] impressionImgs;
        }

        protected override void Start()
        {
            base.Start();

            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].obj.SetActive(panels[i].cityType == GameManager.instance.City);
                if (panels[i].cityType == GameManager.instance.City)
                {
                    buttons = panels[i].buttons;
                    impressionImgs = panels[i].impressionImgs;
                }
            }

            for (int i = 0; i < buttons.Length; i++)
            {
                var idx = i;
                buttons[idx].onClick.AddListener(() => OnMoveToFloor(idx));
            }
            backBtn.onClick.AddListener(OnBack);

            for (int i = 0; i < impressionImgs.Length; i++)
            {
                impressionImgs[i].gameObject.SetActive(i == 0);
            }

            EventDispatcher.Instance.RegisterListener<EventKey.OnSelect>(GetClickItem);
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnSelect>(GetClickItem);
        }

        private void OnEnable()
        {
            gameObject.transform.SetAsLastSibling();
        }

        private void GetClickItem(EventKey.OnSelect obj)
        {
            if (obj.mapController != null)
            {
                for (int i = 0; i < impressionImgs.Length; i++)
                {
                    impressionImgs[i].gameObject.SetActive(i == obj.idx);
                }
            }
        }

        private void OnMoveToFloor(int v)
        {
            for (int i = 0; i < impressionImgs.Length; i++)
            {
                impressionImgs[i].gameObject.SetActive(i == v);
            }

            uiPanel.Hide(() =>
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnSelect { idx = v, elevatorPanel = this });
                base.Hide();
            });
        }
        void OnBack()
        {
            uiPanel.Hide(() =>
            {
                base.Hide();
            });
        }
    }
}