using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using SCN;
using _Base;

namespace _WolfooSchool
{
    public class AdsPanel : Panel
    {
        [SerializeField] Button backBtn;
        [SerializeField] Button confirmBtn;
        [SerializeField] Button buyWithCoin;
        [SerializeField] Button cancelBtn;
        [SerializeField] Button confirmWithNoCoinBtn;
        [SerializeField] Text title;
        [SerializeField] Image picture;
        [SerializeField] Image background;
        [SerializeField] Text coinText;
        [SerializeField] Text textItem;

        private int curInstanceId;
        int curIdx;
        private PriceItem priceItem;
        bool isWatchAds;
        Vector3 startScale;
        bool isStart = true;
        private int curSubIdx;
        private string _nameObj;
        private string _curPanel;
        private string minigameName;

        protected override void Start()
        {
            InitEvent();
            if (isStart)
            {
                base.Hide();
                isStart = false;
            }
        }
        private void OnEnable()
        {
            coinText.text = priceItem.price + "";
            transform.SetAsLastSibling();

            if (isStart) return;
            //      GUIManager.instance.SetLayerHUD(100);
        }
        private void OnDisable()
        {
            //      GUIManager.instance.SetLayerHUD(1);
        }

        void InitEvent()
        {
            backBtn.onClick.AddListener(OnCancelClick);
            confirmBtn.onClick.AddListener(OnConfirmClick);
            confirmWithNoCoinBtn.onClick.AddListener(OnConfirmClick);
            buyWithCoin.onClick.AddListener(OnBuyWithCoin);
            cancelBtn.onClick.AddListener(() => base.Hide());

            EventManager.InitAdsPanelWithNoCoin += GetInitAdsWithNoCoin;
            EventDispatcher.Instance.RegisterListener<EventKey.InitAdsPanel>(GetInitAds);
        }
        private void OnDestroy()
        {
            EventManager.InitAdsPanelWithNoCoin -= GetInitAdsWithNoCoin;
            EventDispatcher.Instance.RemoveListener<EventKey.InitAdsPanel>(GetInitAds);
        }

        private void GetInitAds(EventKey.InitAdsPanel item)
        {
            _curPanel = item.curPanel;
            _nameObj = item.nameObj;

            if (item.textStr != null && !item.textStr.Equals(""))
            {
                curInstanceId = item.instanceID;
                curIdx = item.idxItem;

                picture.gameObject.SetActive(false);
                textItem.text = item.textStr;
                textItem.gameObject.SetActive(true);

                cancelBtn.gameObject.SetActive(true);
                confirmWithNoCoinBtn.gameObject.SetActive(true);
                confirmBtn.gameObject.SetActive(false);
                buyWithCoin.gameObject.SetActive(false);
                title.text = "WATCH A VIDEO TO UNLOCK ITEM";
                return;
            }
            curInstanceId = item.instanceID;
            curIdx = item.idxItem;
            picture.sprite = item.sprite;
            GameManager.instance.ScaleImage(picture, 500, 390);
            picture.color = Color.white;
            picture.gameObject.SetActive(true);
            textItem.gameObject.SetActive(false);

            cancelBtn.gameObject.SetActive(true);
            confirmWithNoCoinBtn.gameObject.SetActive(true);
            confirmBtn.gameObject.SetActive(false);
            buyWithCoin.gameObject.SetActive(false);

            title.text = "WATCH A VIDEO TO UNLOCK ITEM";
        }

        private void GetInitAdsWithNoCoin(int id_, Sprite sprite)
        {
            _curPanel = GUIManager.instance.CurModeType.ToString();
            _nameObj = sprite.name;

            curIdx = id_;
            picture.sprite = sprite;
            picture.SetNativeSize();
            picture.sprite = sprite;
            GameManager.instance.ScaleImage(picture, 500, 390);
            picture.color = Color.white;
            picture.gameObject.SetActive(true);
            textItem.gameObject.SetActive(false);

            cancelBtn.gameObject.SetActive(true);
            confirmWithNoCoinBtn.gameObject.SetActive(true);
            confirmBtn.gameObject.SetActive(false);
            buyWithCoin.gameObject.SetActive(false);

            title.text = "WATCH A VIDEO TO UNLOCK ITEM";
        }

        void OnBuyWithCoin()
        {
            //   SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            // Set up coin
            var skinCoin = GameManager.instance.GetCoin(curIdx);
            DataSceneManager.Instance.UpdateCoin(skinCoin, false, () =>
                {
                    base.Hide();
                    EventManager.OnWatchAds?.Invoke(curIdx, priceItem);
                },
                () =>
                {
                    OnFailBuyWithCoin();
                });
        }
        void OnFailBuyWithCoin()
        {
            EventManager.OpenPanel?.Invoke(PanelType.BuyFailPanel);
        }

        private void OnCancelClick()
        {
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);
            base.Hide();
        }

        void ParseMiniGameName()
        {
            if (_curPanel == PanelType.ShapeMode.ToString()) minigameName = "geometry";
            else if (_curPanel == PanelType.FruitMode.ToString()) minigameName = "fruit_decor";
            else if (_curPanel == PanelType.ClayMode.ToString()) minigameName = "clay_decor";

            if (_curPanel == PanelType.None.ToString()) minigameName = "NULL";
        }

        private void OnConfirmClick()
        {
            //   FirebaseManager.instance.LogWatchAds("Bam_nut");
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);
            ParseMiniGameName();

            _Base.FirebaseManager.instance.LogWatchAds(AdsLogType.ad_rv_click.ToString(), "school", minigameName, _nameObj);
            OnWatchAds();
        }
        void OnWatchAds()
        {
            //base.Hide();
            //EventManager.OnWatchAds?.Invoke(curIdx, priceItem);
            //EventDispatcher.Instance.Dispatch(new EventKey.OnWatchAds { instanceID = curInstanceId, idxItem = curIdx });

            if (AdsManager.Instance.HasRewardVideo)
            {
                base.Hide();
                _Base.FirebaseManager.instance.LogWatchAds(AdsLogType.ad_rv_request.ToString(), "school", minigameName, _nameObj);
                AdsManager.Instance.ShowRewardVideo(() =>
                {
                    _Base.FirebaseManager.instance.LogWatchAds(AdsLogType.ad_rv_success.ToString(), "school", minigameName, _nameObj);
                    EventManager.OnWatchAds?.Invoke(curIdx, priceItem);
                    EventDispatcher.Instance.Dispatch(new EventKey.OnWatchAds { instanceID = curInstanceId, idxItem = curIdx });
                });
            }
            else
            {
                _Base.FirebaseManager.instance.LogWatchAds(AdsLogType.ad_rv_failed.ToString(), "school", minigameName, _nameObj);
                EventManager.OpenPanel?.Invoke(PanelType.NoAds);
            }
        }
    }
}