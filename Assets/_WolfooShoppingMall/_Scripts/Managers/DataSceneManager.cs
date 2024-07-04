using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SCN.Common;
using DG.Tweening;
using _Base;
using SCN;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using _WolfooSchool;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace _WolfooShoppingMall
{
    public class DataSceneManager : SingletonBind<DataSceneManager>
    {
        bool isForceUnlockAll;

        public DataStorage LocalDataStorage => BaseDataManager.Instance.LocalGameStorageData;

        public bool IsForceUnlockAll { get => isForceUnlockAll; }
        public ItemsDataSO ItemDataSO;
        public BackItemDataSO BackItemDataSO;
        public CharacterDataSO MainCharacterData;
        public NewCharacterDataSO NewCharacterData;
        public BeachVillaDataSO BeachVillaData;
        public MediterraneanDataSO MediterraneanData;
        public TutorialConfigData TutorialData;
        public HouseDataSO HouseData;
        private AsyncOperationHandle<IList<GameObject>> lastAssetOperation;

        public Floor OrderFloor(PanelType panelType)
        {
            var data = DataTownTransporter.roomDatas;
            foreach (var item in data)
            {
                var floor = item.GetComponent<Floor>();
                if(floor != null && floor.PanelType == panelType)
                {
                    Debug.Log("Order Floor >>> " + floor);
                    return floor;
                }
            }
            Debug.Log("Order Floor is NULL !!!");
            return null;
        }
        public RoomBase OrderWorldFloor(PanelType panelType)
        {
            var data = DataTownTransporter.roomDatas;
            foreach (var item in data)
            {
                var floor = item.GetComponent<RoomBase>();
                if(floor != null && floor.Panel == panelType)
                {
                    return floor;
                }
            }
            return null;
        }

        public void UnlockVideo(int idEpisode, int idVideo)
        {
            LocalDataStorage.unlockEpisodes[idEpisode].unlockVideos[idVideo] = true;
            LocalDataStorage.Save();
        }
        public void UnlockCharacter(int id)
        {
            LocalDataStorage.unlockCharacters[id] = true;
            LocalDataStorage.Save();
        }
        public void SetRemoveAds()
        {
            LocalDataStorage.isRemoveAds = true;
            LocalDataStorage.Save();
        }
        public void SetTryFree()
        {
            LocalDataStorage.isTryFree = true;
            LocalDataStorage.Save();
        }

        private IEnumerator LoadLocalData()
        {
            yield return null;

            if (IsForceUnlockAll) { ForceUnlockAll(); }
            Debug.Log("End Load Data Item in scene!");
            EventDispatcher.Instance.Dispatch(new EventKey.OnLoadDataCompleted());
        }
        public void ForceUnlockAll()
        {
            LocalDataStorage.Setup(true);
        }

        public void LoadData()
        {
            Debug.Log("Begin Load Data Item in scene....");
#if UNITY_EDITOR
            var isTest = GameController.Instance.IsTestingData;
            isForceUnlockAll = isTest;
            if (isTest)
            {
                StartCoroutine(LoadLocalData());
            }
            else
            {
                StartCoroutine(LoadLocalData());
            }
#else
                StartCoroutine(LoadLocalData());
#endif
        }

        internal void LoadRoomData(AssetLabelReference dataLabel, Action OnCompleted)
        {
            if (lastAssetOperation.IsValid())
            {
                Addressables.Release(lastAssetOperation);
                //
            }

            var assetOperation = Addressables.LoadAssetsAsync<GameObject>(dataLabel, (callBack) =>
            {
                DataTownTransporter.AddRoomData(callBack);
                Debug.Log("Loading Asset Scene >>>>" + callBack.name);

            });
            lastAssetOperation = assetOperation;
            assetOperation.Completed += (data) =>
            {
                OnCompleted?.Invoke();
            };
        }
    }


}