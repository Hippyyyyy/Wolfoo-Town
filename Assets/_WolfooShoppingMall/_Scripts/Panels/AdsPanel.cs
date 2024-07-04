using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using SCN;
using _WolfooCity;
//using SCN.Ads;

namespace _WolfooShoppingMall
{
    public class AdsPanel : Panel
    {
        [SerializeField] Button backBtn;
        [SerializeField] Button confirmBtn;
        [SerializeField] Button cancelBtn;
        [SerializeField] Text title;
        [SerializeField] Image picture;

        private int curInstanceId;
        int curIdx;
        private int curSubIdx;
        private string _nameObj;
        private string _curPanel;
        bool isWatchAds;
        Vector3 startScale;
        bool isStart = true;
        private CharacterItem curCharacterItem;
        private CharacterPaletteColorItem curCharacterPaletteColorItem;

        protected override void Start()
        {
            InitEvent();
            if (isStart)
            {
                base.Hide();
                isStart = false;
            }
        }

        private void OnDestroy()
        {
            backBtn.onClick.RemoveListener(OnCancelClick);
            confirmBtn.onClick.RemoveListener(OnConfirmClick);
            cancelBtn.onClick.RemoveListener(OnCancelClick);

            EventDispatcher.Instance.RemoveListener<EventKey.InitAdsPanel>(GetInitAds);
        }

        void InitEvent()
        {
            backBtn.onClick.AddListener(OnCancelClick);
            confirmBtn.onClick.AddListener(OnConfirmClick);
            cancelBtn.onClick.AddListener(OnCancelClick);

            EventDispatcher.Instance.RegisterListener<EventKey.InitAdsPanel>(GetInitAds);
        }

        private void GetInitAds(EventKey.InitAdsPanel item)
        {
            Debug.Log($"Init Ads {item.instanceID}");

            curInstanceId = item.instanceID;
            curIdx = item.idxItem;
            curSubIdx = item.idxSubItem;
            _nameObj = item.nameObj;
            _curPanel = item.curPanel;

            if (item.filmMachine != null || item.clipItem != null)
            {
                picture.sprite = item.spriteItem;
                picture.color = Color.white;
                picture.gameObject.SetActive(true);

                title.text = "WATCH A VIDEO TO UNLOCK VIDEO";
                return;
            }

            picture.sprite = item.spriteItem;
            picture.color = Color.white;
            picture.gameObject.SetActive(true);
            //
            title.text = "WATCH A VIDEO TO UNLOCK ITEM";
            if (item.title != null) title.text = item.title;

            if (item.characterItem != null)
            {
                curCharacterItem = item.characterItem;
                picture.color = item.color;
            }
            if (item.characterPaletteColorItem != null)
            {
                curCharacterPaletteColorItem = item.characterPaletteColorItem;
                picture.color = item.color;
            }
        }

        private void OnCancelClick()
        {
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);
            }
            else if (SoundBaseRoomManager.Instance != null)
            {
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            }
            if (uiPanel == null) uiPanel = GetComponent<UIPanel>();
            uiPanel.Hide(() =>
            {
                base.Hide();
            });
        }

        private void OnConfirmClick()
        {
            if(SoundManager.instance != null)
            {
                SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);
            }
            else if(SoundBaseRoomManager.Instance != null)
            {
                SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
            }
            _Base.FirebaseManager.instance.LogWatchAds(_Base.AdsLogType.ad_rv_click.ToString(), 
                GUIManager.instance.CurrentMapController.ToString(), 
                _curPanel, 
                _nameObj);
            OnWatchAds();
        }
        void OnWatchAds()
        {
            //EventDispatcher.Instance.Dispatch(new EventKey.OnWatchAds
            //{
            //    instanceID = curInstanceId,
            //    idxItem = curIdx,
            //    idxSubItem = curSubIdx
            //});
            //base.Hide();

            Debug.Log($"Click Wath Ads {curInstanceId}");
            if (AdsManager.Instance.HasRewardVideo)
            {
                AdsManager.Instance.ShowRewardVideo(() =>
                {
                    uiPanel.Hide(() =>
                    {
                        base.Hide();
                    });
                    _Base.FirebaseManager.instance.LogWatchAds(_Base.AdsLogType.ad_rv_success.ToString(), 
                        GUIManager.instance.CurrentMapController.ToString(), 
                        _curPanel, 
                        _nameObj);
                    EventDispatcher.Instance.Dispatch(new EventKey.OnWatchAds
                    {
                        instanceID = curInstanceId,
                        idxItem = curIdx,
                        idxSubItem = curSubIdx,
                        characterItem = curCharacterItem,
                        characterPaletteColorItem = curCharacterPaletteColorItem,
                    });
                    Debug.Log($"Wath Ads {curInstanceId} is Successfull !");
                });
            }
            else
            {
                base.Hide();
                _Base.FirebaseManager.instance.LogWatchAds(_Base.AdsLogType.ad_rv_failed.ToString(), GUIManager.instance.CurrentMapController.ToString(), 
                    _curPanel, 
                    _nameObj);
                GUIManager.instance.OpenPanel(PanelType.NoAds);
            }
        }
    }
}