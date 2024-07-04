using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace _Base
{
    public class BaseDataManager : MonoBehaviour
    {
        [SerializeField] int totalData;
        [SerializeField] DataSetting dataSetting;

        public static BaseDataManager Instance { get; private set; }

        public System.Action<List<object>> OnLoadDataCompleted { get; private set; }

        public PlayerMe playerMe { get => LocalSaveLoadData.Instance.playerMe; }
        public _WolfooShoppingMall.SaveLoadDataTutorial tutorialSaveLoadData { get => LocalSaveLoadData.Instance.tutorialSaveLoadData; }
        public _WolfooShoppingMall.DataStorage LocalGameStorageData { get => LocalSaveLoadData.Instance.LocalDataStorage; }

        public bool IsMuteMusic { get => LocalSaveLoadData.Instance.playerMe.IsMuteMusic; }
        public bool IsMuteSound { get => LocalSaveLoadData.Instance.playerMe.IsMuteSound; }

        public _WolfooShoppingMall.IAPDataManager IAPDataManager;

        public List<Panel> uiPanelSchoolVers = new List<Panel>();
        public List<Panel> uiPanelMallVers = new List<Panel>();

        public int CountAdsSuccess
        {
            get
            {
                return playerMe.TotalAdsSuccess;
            }
            set
            {
                playerMe.TotalAdsSuccess++;
                playerMe.Save();
            }
        }

        public bool IsResetDay
        {
            get
            {
                var sleepTime = DateTime.Now - playerMe.LastOpenTime;
                if (sleepTime.Days >= 1)
                {
                    return true;
                }
                return false;
            }
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            Debug.Log("Addressable Initialize Async!");
            Addressables.InitializeAsync().Completed += (data) =>
            {
                Debug.Log("Addressable Initialize Complete!");
                LocalSaveLoadData.Instance.Load();
             //   CheckAdsTiers();
                StartCoroutine(LoadAllDialog());
            };
        }

        private void OnDestroy()
        {
        }

        public GameObject GetPanelObj<K>(CityVersion cityVersion) where K: Panel
        {
            switch (cityVersion)
            {
                case CityVersion.School:
                    var panel1 = uiPanelSchoolVers.Find(t => t.GetType().Name.Equals(typeof(K).Name));
                    return panel1?.gameObject;
                case CityVersion.Mall:
                    var panel2 = uiPanelMallVers.Find(t => t.GetType().Name.Equals(typeof(K).Name));
                    return panel2?.gameObject;
            }
            return null;
        }

        private IEnumerator LoadAllDialog()
        {
            yield return new WaitForSeconds(3);

            Addressables.LoadAssetsAsync<GameObject>(dataSetting.DialogMallTag, (callback) =>
            {
                var panel = callback.GetComponent<Panel>();
                if (panel != null)
                    uiPanelMallVers.Add(panel);
            });

            yield return new WaitForSeconds(6);
            Addressables.LoadAssetsAsync<GameObject>(dataSetting.DialogSchoolTag, (callback) =>
            {
                var panel = callback.GetComponent<Panel>();
                if (panel != null)
                    uiPanelSchoolVers.Add(panel);
            });
        }

        private void CheckAdsTiers()
        {
            if (IsResetDay)
            {
                Debug.Log("Your day is Reseting...");
                playerMe.TotalAdsSuccess = 0;
                playerMe.LastOpenTime = DateTime.Now;
                playerMe.Save();
            }
        }
        public void SetMute(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Music:
                    playerMe.IsMuteMusic = true;
                    break;
                case SoundType.SFx:
                    playerMe.IsMuteSound = true;
                    break;
            }
        }
        public void SetUnMute(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Music:
                    playerMe.IsMuteMusic = false;
                    break;
                case SoundType.SFx:
                    playerMe.IsMuteSound = false;
                    break;
            }
        }

        public bool AddressableResourceExists(object key, Type type)
        {
            foreach (var l in Addressables.ResourceLocators)
            {
                IList<IResourceLocation> locs;
                if (l.Locate(key, type, out locs))
                    return true;
            }
            return false;
        }

        private IEnumerator StartOrder<T>(System.Action<T> OnSuccess, System.Action OnFail) where T : class
        {
            Debug.Log(">>>>>>>>>>>>>>>> Order Data BEGIN  <<<<<<<<<<<<<<<<<");
            foreach (var item in dataSetting.AddressDatas)
                {
                var isKeyVaild = AddressableResourceExists(item, typeof(T));
                if (!isKeyVaild) continue;

                var handle = Addressables.LoadAssetAsync<T>(item);
                if (!handle.IsValid())
                {
                    Debug.LogError("Asset Reference " + item + " is NOT Valid");
                    OnFail?.Invoke();
                    continue;
                }

                while (!handle.IsDone)
                {
                    var status = handle.PercentComplete;
                    yield return null;
                }

                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    if (handle.Result.GetType().Equals(typeof(T)))
                    {
                        Debug.Log("Order Succeeded Data: " + handle.Result);
                        OnSuccess?.Invoke(handle.Result);
                    }
                }
                else
                {
                    Debug.LogError("Order Error Data: " + item);
                    Debug.LogError(handle.OperationException.Message);
                    OnFail?.Invoke();
                }
            }
            Debug.Log(">>>>>>>>>>>>>>>> Order Data Complete  <<<<<<<<<<<<<<<<<");
        }
        public void Order<T>(System.Action<T> OnSuccess, System.Action OnFail) where T : class
        {
            StartCoroutine(StartOrder<T>(OnSuccess, OnFail));
        }

        public void GetDataAddressable(string sceneName, System.Action<List<object>> OnCompleted)
        {
            OnLoadDataCompleted = OnCompleted;
           // StartCoroutine(OnReleaseData());
            StartCoroutine(GetDataAddressable(sceneName));
        }

        private IEnumerator GetDataAddressable(string sceneName)
        {
            List<string> data = new List<string>();
            foreach (var item in dataSetting.TownDatas)
            {
                if (!item.scenePath.Equals(sceneName)) continue;
                data = item.dataAssetPath;
                break;
            }

            List<object> loadedDatas = new List<object>(data.Count);

            Debug.Log("########### Load Data Config BEGIN  ###########");
            foreach (var reference in data)
            {
                var handle = Addressables.LoadAssetAsync<object>(reference);
                if(!handle.IsValid())
                {
                    Debug.LogError("Asset Reference " + reference + " is NOT Valid");
                    continue;
                }

                while (!handle.IsDone)
                {
                    var status = handle.PercentComplete;
                    yield return null;
                }

                yield return handle;

                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Load Succeeded Data: " + handle.Result);
                    loadedDatas.Add(handle.Result);
                }
                else
                {
                    Debug.LogError("Load Error Data: " + reference);
                    Debug.LogError(handle.OperationException.Message);
                }
            }
            Debug.Log("########### Load Data Config COMPLETED  ###########");
            yield return new WaitForEndOfFrame();
            OnLoadDataCompleted?.Invoke(loadedDatas);
          //  EventDispatcher.Instance.Dispatch(new _WolfooCity.EventKey.OnLoadCompleted());
        }
    }
}