using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterSlotsView : MonoBehaviour
    {
        [SerializeField] CharacterSlotView characterSlotViewPrefab;
        [SerializeField] List<CharacterSlotView> characterSlotViews;
        [SerializeField] ButtonAddx10 btnAddx10;
        [SerializeField] Sprite moreCharacterSprite;

        public static Action OnSelected;

        private void Awake()
        {
            //DataCharacterManager.Instance.LocalData.SortListCharacters();
        }

        private void Start()
        {
            Init();
            DataCharacterManager.Instance.LocalData.AddCategoryItemFeature();
            DataCharacterManager.Instance.LocalData.AddCategoryColorItemFeature();
            CheckBtnAddx10();
        }

        public void OnEnable()
        {
            OnSelected += SelectedCallBack;
            EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(OnWatchAds);
        }

        private void OnDisable()
        {
            OnSelected -= SelectedCallBack;
            EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(OnWatchAds);
        }

        private void OnDestroy()
        {
            OnSelected -= SelectedCallBack;
        }

        public void SelectedCallBack()
        {
            for (int i = 0; i < characterSlotViews.Count; i++)
            {
                var slotView = characterSlotViews[i];
                slotView.IsSelected = false;
                slotView.IsDragging = false;
                slotView.IsFirstClick = true;
                slotView.HideFloop();
            }
            CheckBtnAddx10();
        }

        void CheckBtnAddx10()
        {
            if (!DataCharacterManager.Instance.LocalData.IsWatchAdsAddSlot && !AdsManager.Instance.IsRemovedAds)
            {
                if (DataCharacterManager.Instance.LocalData.ListCharacters != null) 
                {
                    if (DataCharacterManager.Instance.LocalData.ListCharacters.Count == 5)
                    {
                        //
                        btnAddx10.transform.SetParent(characterSlotViews[4].ParAddx10);
                        btnAddx10.GetComponent<RectTransform>().DOAnchorPos(Vector3.zero, 0f);
                        btnAddx10.transform.DOScale(1f, 0.3f);
                        InitButtonAddx10();
                    }
                }
                   
            }
            else
            {
                btnAddx10.transform.DOScale(0f, 0f);
            }
        }

        void Init()
        {
            for (int i = 0; i < 10; i++)
            {
                var slotView = Instantiate(characterSlotViewPrefab, transform);
                characterSlotViews.Add(slotView);
                slotView.Id = i;
                if (i == 4)
                {
                    btnAddx10.transform.SetParent(slotView.ParAddx10);
                    btnAddx10.transform.DOScale(0f, 0f);
                }
            }
            characterSlotViews[0].IsSelected = true;
            characterSlotViews[0].IsFirstClick = false;
            characterSlotViews[0].IsDragging = true;
            for (int i = 0; i < 10; i++)
            {
                characterSlotViews[i].SetUp();
            }
            if (!DataCharacterManager.Instance.LocalData.IsWatchAdsAddSlot)
            {
                for (int i = 5; i < 10; i++)
                {
                    characterSlotViews[i].gameObject.SetActive(false);
                }
            }
            DOVirtual.DelayedCall(0.2f, () =>
            {
                CheckAddSlot();
            });
        }

        public void CheckAddSlot()
        {
            if (!DataCharacterManager.Instance.LocalData.IsWatchAdsAddSlot)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(2500f, transform.GetComponent<RectTransform>().sizeDelta.y);
            }
            else
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(5000f, transform.GetComponent<RectTransform>().sizeDelta.y);
            }
        }
        //
        public void InitButtonAddx10()
        {
            btnAddx10.Btn.onClick.AddListener(ShowAds);
        }
        /*public void CheckButtonAddx10()
        {
            var data = DataCharacterManager.Instance.LocalData.IsWatchAdsAddSlot;
            if (data)
            {
                btnAddx10.transform.DOScale(0f, 0f);
            }
            else
            {
                btnAddx10.transform.DOScale(1f, 0f);
                InitButtonAddx10();
            }
        }*/
        public void RemoveButtonAddx10()
        {
            btnAddx10.Btn.onClick.RemoveAllListeners();
        }
        void ShowAds()
        {
            if(AdsManager.Instance.IsRemovedAds)
            {
                Addx10();
            }
            else
            {
                EventDispatcher.Instance.Dispatch(new EventKey.InitAdsPanel()
                {
                    instanceID = GetInstanceID(),
                    adsType = "TextPanel",
                    title = "Do you want to open more character?",
                    spriteItem = moreCharacterSprite,
                    curPanel = "PlayerScroll",
                    nameObj = "Open_more_Character"
                });
                GUIManager.instance.OpenPanel(PanelType.Ads);
            }
        }

        private void OnWatchAds(EventKey.OnWatchAds obj)
        {
            if(obj.instanceID == GetInstanceID())
            {
                Addx10();
            }
        }
        void Addx10()
        {
            btnAddx10.transform.DOScale(0f, 0.3f).SetEase(Ease.OutBack);
            var data = DataCharacterManager.Instance.LocalData.IsWatchAdsAddSlot;
            if (!data)
            {
                transform.GetComponent<RectTransform>().sizeDelta = new Vector2(4900f, transform.GetComponent<RectTransform>().sizeDelta.y);
                for (int i = 5; i < 10; i++)
                {
                    var slotView = characterSlotViews[i];
                    slotView.Id = i;
                    slotView.IsAds = false;
                    slotView.gameObject.SetActive(true);
                    slotView.SetUp();
                }
                DataCharacterManager.Instance.LocalData.IsWatchAdsAddSlot = true;
                DataCharacterManager.Instance.LocalData.SortListCharacters();
                RemoveButtonAddx10();

            }
        }
    }
}
