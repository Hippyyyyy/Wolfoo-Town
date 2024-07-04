using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Base
{
    [System.Serializable]
    public class LocalSaveLoadData : Singleton<LocalSaveLoadData>
    {
        public PlayerMe playerMe;
        public _WolfooShoppingMall.SaveLoadDataTutorial tutorialSaveLoadData;
        public _WolfooShoppingMall.DataStorage LocalDataStorage;

        public void Load()
        {
            Debug.Log(">>>>>>>>>>>>> BEGIN LOAD LOCAL DATA  <<<<<<<<<<<<<<<");
            playerMe = new PlayerMe();
            var _player = playerMe.Load();
            if (_player == null)
            {
                playerMe = new PlayerMe(false, false);
                playerMe.Save();
            }
            else playerMe = _player;

            tutorialSaveLoadData = new _WolfooShoppingMall.SaveLoadDataTutorial();
            var _tutorial = tutorialSaveLoadData.Load();
            if (_tutorial == null)
            {
                tutorialSaveLoadData.Init();
            }
            else tutorialSaveLoadData = _tutorial;

            LocalDataStorage = new _WolfooShoppingMall.DataStorage();
            var _dataStorage = LocalDataStorage.Load();
            if (_dataStorage == null)
            {
                LocalDataStorage.Init();
                LocalDataStorage.Setup();
            }
            else
            {
                LocalDataStorage = _dataStorage;
            }
            Debug.Log(">>>>>>>>>>>>> END LOAD LOCAL DATA  <<<<<<<<<<<<<<<");
        }
        public void SetCreated()
        {
            playerMe.Save();
        }
    }
}
