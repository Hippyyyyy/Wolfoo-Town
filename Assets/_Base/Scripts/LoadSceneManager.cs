using _WolfooShoppingMall;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace _Base
{
    public enum CityType
    {
        School,
        Mall,
        House,
        Hospital,
        Playground,
        Opera,
        BeachVilla,
        CityTown,
        CampingPark,
        None,
        CharacterCreator,
        Mediterranean,

    }
    public enum CityVersion
    {
        School,
        Mall,
        NewCity,
    }

    public class LoadSceneManager : MonoBehaviour
    {
        [SerializeField] _WolfooShoppingMall.LoadingPanel loadingPanel;
        [SerializeField] NoInternetPanel noInternetDialog;

        public System.Action OnLoadCompleted;
        public System.Action OnCloseScene;
        public List<object> sceneData;
        //
        public static LoadSceneManager Instance;
        private string sceneHandleName;
        private AssetLabelReference curDataLabel;
        private AsyncOperationHandle<SceneInstance> lastSceneHandler;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            Application.targetFrameRate = 60;
        }
        private void Update()
        {
            if (!HasInternet()) { noInternetDialog.Show(); }
        }
        bool HasInternet()
        {
            return Application.internetReachability == NetworkReachability.NotReachable ? false : true;
        }

        void OnChangeScene()
        {
            loadingPanel.Close(0.5f, () =>
            {
                OnCloseScene?.Invoke();
            });
        }
        public void ReloadScene()
        {
            GUIManager.instance.OnRetryMode();
            OnLoadScene(sceneHandleName, curDataLabel);
        }
        public void GoToCityScene()
        {
            OnLoadScene("WolfooCity_Scene", null);
        }
        public void GoToCharacterScene()
        {
            OnLoadScene("CharacterCreater_Scene", null);
        }
        public void OnLoadScene(string name, AssetLabelReference dataLabel)
        {
            loadingPanel.gameObject.SetActive(true);
            loadingPanel.Open(true);
            
            Debug.Log($"Begin Load Scene {dataLabel}");
            sceneHandleName = name;
            curDataLabel = dataLabel;


            DataTownTransporter.ReleaseRoomData();
            if(dataLabel == null)
            {
                OnLoading(sceneHandleName, () =>
                {
                    if (lastSceneHandler.IsValid()) Addressables.UnloadSceneAsync(lastSceneHandler);
                    StartCoroutine(PlayScreen());
                });
            }
            else
            {
                OnLoading(sceneHandleName, () =>
                {
                    DataSceneManager.Instance.LoadRoomData(dataLabel, () =>
                    {
                        StartCoroutine(PlayScreen());
                    });
                });
            }
        }

        IEnumerator PlayScreen()
        {
            yield return new WaitForEndOfFrame();

            OnLoadCompleted?.Invoke();
            Debug.Log("Load Scene Completed");
            OnChangeScene();
        }
        private void OnLoading(string name_, System.Action OnCompleted)
        {
            Addressables.LoadSceneAsync(name_, LoadSceneMode.Single, true).Completed += (data) =>
            {
                OnCompleted?.Invoke();
                lastSceneHandler = data;
            };
        }
    }
}