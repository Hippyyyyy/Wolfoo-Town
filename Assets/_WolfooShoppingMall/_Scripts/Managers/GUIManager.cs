using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SCN.Common;
using SCN.Tutorial;
using SCN;
using _Base;
using System;
using _WolfooSchool;
using UnityEngine.EventSystems;
//using SCN.Ads

namespace _WolfooShoppingMall
{
    public class GUIManager : MonoBehaviour
    {
        public static GUIManager instance;

        [Header("----------------- PROPERTISE -----------------")]

        [SerializeField] bool isShowBanner;
        public Canvas canvasSpawnMode;

        [SerializeField] PanelType spawnFirstPanel;
        [SerializeField] Camera mainCamera;
        [SerializeField] LoadingPanel loadingPanelpb;
        public CityType CurrentMapController;
        [SerializeField] EventSystem eventSystem;
        [SerializeField] GraphicRaycaster[] raycasters; 

        List<Floor> curUIRooms = new List<Floor>();
        List<Map> curSubRooms = new List<Map>();
        List<RoomBase> curRoomWorlds = new List<RoomBase>();

        private UIManager myUI;
        private LoadingPanel loadingPanel;
        private bool sceneIsLoaded;

        public Camera MainCamera { get => mainCamera; }

        private float beginLevelTime;
        private float totalLevelTime;
        private float process => GameManager.instance.ProcessTouchedAllItem;
        private PanelType curLocation;

        private EndLevelState levelState
        {
            get
            {
                if (process == 1)
                {
                    return EndLevelState.Win;
                }

                return EndLevelState.Back;
            }
            set
            {
                levelState = value;
            }
        }
        private void OnApplicationQuit()
        {
            LogLevel(curLocation);
        }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            HUDSystem.Instance.CurCityVersion = CityVersion.Mall;
        }

        private void Start()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Input.multiTouchEnabled = false;

            myUI = FindAnyObjectByType<UIManager>();

            EventDispatcher.Instance.RegisterListener<EventKey.OnBackClick>(GetBackClick);
            EventDispatcher.Instance.RegisterListener<EventKey.OnBackClick>(GetBackClick);
            LoadSceneManager.Instance.OnLoadCompleted += LoadScene;

            if (isShowBanner) AdsManager.Instance.ShowBanner();
            //     LoadScene();
            _Base.FirebaseManager.instance.AssignPanel(CurrentMapController.ToString());

            Invoke("RegisterEventSystem", 2);
        }

        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnBackClick>(GetBackClick);
            LoadSceneManager.Instance.OnLoadCompleted -= LoadScene;

            if (isShowBanner) AdsManager.Instance.HideBanner();
        }

        public void SpawnTestingPanel()
        {
            OpenPanel(spawnFirstPanel);
        }

        private void RegisterEventSystem()
        {
            if (eventSystem != null)
                eventSystem.enabled = true;
            foreach (var item in raycasters)
            {
                item.enabled = true;
            }
        }

        private void SpawnFloorUI(PanelType panelType)
        {
            foreach (var floor in curSubRooms)
            {
                floor.gameObject.SetActive(false);
            }
            var hasFloor = false;
            foreach (var floor in curUIRooms)
            {
                if (floor.PanelType == panelType)
                {
                    floor.gameObject.SetActive(true);
                    hasFloor = true;
                }
                else
                {
                    floor.gameObject.SetActive(false);
                }
            }

            if (hasFloor) return;
            var floorPb = DataSceneManager.Instance.OrderFloor(panelType);
            if(floorPb != null)
            {
                var myFloor = Instantiate(floorPb, canvasSpawnMode.transform);
                myFloor.Register(CurrentMapController);
                curUIRooms.Add(myFloor);
            }
        }
        private void SpawnMap(PanelType panelType)
        {
            foreach (var floor in curUIRooms)
            {
                floor.gameObject.SetActive(false);
            }
            var hasFloor = false;
            foreach (var floor in curSubRooms)
            {
                if (floor.PanelType == panelType)
                {
                    floor.gameObject.SetActive(true);
                    hasFloor = true;
                }
                else
                {
                    floor.gameObject.SetActive(false);
                }
            }

            if (hasFloor) return;
            var mapPb = DataSceneManager.Instance.OrderFloor(panelType).GetComponent<Map>();
            if (mapPb != null)
            {
                var myRoom = Instantiate(mapPb, canvasSpawnMode.transform);
                myRoom.Register(CurrentMapController);
                curSubRooms.Add(myRoom);
            }
        }
        private void SpawnFloorWorld(PanelType panelType)
        {
            var floorPb = DataSceneManager.Instance.OrderWorldFloor(panelType);

            var hasFloor = false;
            foreach (var floor in curRoomWorlds)
            {
                if (floor.Panel == panelType)
                {
                    floor.gameObject.SetActive(true);
                    hasFloor = true;
                }
                else
                {
                    floor.gameObject.SetActive(false);
                }
            }

            if (hasFloor) return;
            if (floorPb != null)
            {
                var myFloor = Instantiate(floorPb, transform);
                curRoomWorlds.Add(myFloor);
                myUI.MyRoom = myFloor;
            }
        }

        public void LoadScene()
        {
            if (sceneIsLoaded) return;
            sceneIsLoaded = true;

            DataSceneManager.Instance.LoadData();
            OpenPanel(spawnFirstPanel);
            HUDSystem.Instance.Show<AdsPanel>(null, UIPanels<HUDSystem>.ShowType.Duplicate);
        }
        private void BeginCalculatingLevelTime()
        {
            beginLevelTime = Time.time;
            Debug.Log("begin Time: " + Time.time);
        }
        private float CalculatingLevelTime()
        {
            totalLevelTime = Time.time - beginLevelTime;
            Debug.Log("end Time: " + Time.time);
            Debug.Log("Time: " + TimeSpan.FromSeconds(totalLevelTime));
            return totalLevelTime;
        }

        public void OnRetryMode()
        {
            FirebaseManager.instance.LogEndLevel(CurrentMapController.ToString(), curLocation.ToString(), CalculatingLevelTime(), process, levelState); ;
        }

        private void GetBackClick(EventKey.OnBackClick item)
        {
            //OnBack(item.panelType);

            FirebaseManager.instance.LogEndLevel(CurrentMapController.ToString(), item.myPanelType.ToString(), CalculatingLevelTime(), process, levelState);

            //   Ads Spawn Here
            if (AdsManager.Instance.HasInters)
            {
                _Base.FirebaseManager.instance.LogWatchAds(AdsLogType.ad_inter_request.ToString(), item.myPanelType.ToString());
                AdsManager.Instance.ShowInterstitial(() =>
                {
                    _Base.FirebaseManager.instance.LogWatchAds(AdsLogType.ad_inter_success.ToString(), CurrentMapController.ToString());
                    OnBack(item.parentPanelType);
                });
            }
            else
            {
                _Base.FirebaseManager.instance.LogWatchAds(AdsLogType.ad_inter_fail.ToString(), item.myPanelType.ToString());
                OnBack(item.parentPanelType);
            }
        }

        void OnBack(PanelType panelType)
        {
            OpenPanel(panelType);
        }

        public void OnLoading(System.Action OnLoading = null, System.Action OnComplete = null)
        {
            if (loadingPanel == null)
            {
                loadingPanel = Instantiate(loadingPanelpb, UISetupManager.Instance.transform);
            }
            loadingPanel.Open(OnLoading, OnComplete);
        }
        private void LogLevel(PanelType panelType)
        {
            if (curLocation != PanelType.None)
            {
                FirebaseManager.instance.LogEndLevel(CurrentMapController.ToString(), curLocation.ToString(), CalculatingLevelTime(), process, levelState);
            }
            curLocation = panelType;
            FirebaseManager.instance.LogBeginLevel("school", curLocation.ToString());
            BeginCalculatingLevelTime();
        }

        public void OpenPanel(PanelType panelType)
        {
            switch (panelType)
            {
                #region ======== DIALOGS ========
                case PanelType.Ads:
                    HUDSystem.Instance.Show<_WolfooShoppingMall.AdsPanel>(null, UIPanels<HUDSystem>.ShowType.KeepCurrent);
                    break;
                case PanelType.Intro:
                    LoadSceneManager.Instance.GoToCityScene();
                    break;
                case PanelType.NoAds:
                    HUDSystem.Instance.Show<NoAdsPanel>(null, UIPanels<HUDSystem>.ShowType.KeepCurrent);
                    break;
                case PanelType.IAP:
                    HUDSystem.Instance.Show<IAPPanel>(null, UIPanels<HUDSystem>.ShowType.KeepCurrent);
                    break;
                case PanelType.Lose:
                    HUDSystem.Instance.Show<LosePanel>(null, UIPanels<HUDSystem>.ShowType.KeepCurrent);
                    break;
                case PanelType.VideoBanner:
                    HUDSystem.Instance.Show<VideoBannerPanel>(null, UIPanels<HUDSystem>.ShowType.KeepCurrent);
                    break;
                case PanelType.Elevator:
                    HUDSystem.Instance.Show<ElevatorPanel>(null, UIPanels<HUDSystem>.ShowType.KeepCurrent);
                    break;
                #endregion

                #region ======== FLOORS ========
                case PanelType.LivingRoom1:
                case PanelType.LivingRoom2:
                case PanelType.LivingRoom3:
                case PanelType.Floor1:
                case PanelType.Floor2:
                case PanelType.Floor3:
                case PanelType.HospitalRoom1:
                case PanelType.HospitalRoom2:
                case PanelType.HospitalRoom3:
                case PanelType.PlaygroundRoom1:
                case PanelType.OperaRoom1:
                    LogLevel(panelType);
                    SpawnFloorUI(panelType);
                    break;
                #endregion

                #region ======== ROOMS ========
                case PanelType.CinemaMap:
                case PanelType.ToyMap:
                case PanelType.BakeryMap:
                case PanelType.SuperMarketMap:
                    LogLevel(panelType);
                    SpawnMap(panelType);
                    break;
                #endregion

                #region  ===== FLOORS WORLD =====
                case PanelType.BeachVillaRoom1:
                case PanelType.CampingParkRoom1:
                case PanelType.MediterraneanRoom1:
                    LogLevel(panelType);
                    SpawnFloorWorld(panelType);
                    break;
                    #endregion
            }
        }
    }

    public enum LoadingType
    {
        Blur,
        Img,
    }
    public enum PanelType
    {
        None,
        Ads,
        Back,
        Setting,
        Intro,
        NoAds,
        Home,
        IAP,
        Lose,
        Tutorial,
        Loading,
        VideoBanner,
        Elevator,
        Floor1,
        Floor2,
        Floor3,
        CinemaMap,
        ToyMap,
        BakeryMap,
        SuperMarketMap,
        LivingRoom1,
        LivingRoom2,
        LivingRoom3,
        HospitalRoom1,
        HospitalRoom2,
        HospitalRoom3,
        PlaygroundRoom1,
        OperaRoom1,
        BeachVillaRoom1,
        CampingParkRoom1,
        MediterraneanRoom1,
    }

}