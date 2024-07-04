using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooCity
{
    public class GUIManager : MonoBehaviour
    {
        [SerializeField] ScrollRect scrollRect;
        public static GUIManager Instance { get; private set; }
        private UIPanel[] panels;

        private void Awake()
        {
            if (Instance == null) Instance = this;
        }

        private void Start()
        {
            scrollRect.GetComponent<RectTransform>().sizeDelta = Vector3.zero;
            scrollRect.transform.localPosition = Vector3.zero;

            panels = GetComponentsInChildren<UIPanel>();

            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].gameObject.SetActive(false);
            }
        }

        public void OpenPanel(PanelType panelType)
        {
            foreach (var panel in panels)
            {
                if (panel.PanelType == panelType)
                {
                    panel.gameObject.SetActive(true);
                    return;
                }
            }
        }
    }

    public enum PanelType
    {
        CommingSoon,
        Setting,
        PremiumOneDay,
    }
}