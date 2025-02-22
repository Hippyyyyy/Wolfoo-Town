using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SCN.Common;
using DG.Tweening;
using _Base;
using SCN;

namespace _WolfooSchool
{
    public class DataSceneManager : SingletonBind<DataSceneManager>
    {
        [SerializeField] List<CharacterAnimation> characterSprites;
        [SerializeField] List<Sprite> bagSprites;
        [SerializeField] List<Sprite> chairSprites;
        [SerializeField] List<Sprite> wallSprites;
        [SerializeField] List<Sprite> carpetSprites;
        [SerializeField] List<Sprite> floorSprites;
        [SerializeField] List<Sprite> fruitDecorItems;
        [SerializeField] List<Sprite> shapeTopicItems;
        public bool IsForceUnlockAll;

        public DataStorage LocalDataStorage;
        private string keyName;

        public enum StorageKey
        {
            Data,
            Setting,
        }

        void Start()
        {
            EventManager.OnCompleteCutBox += GetCompleteCutBox;

            keyName = StorageKey.Data.ToString() + "_WolfooSchool";
            EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(InitData);
        }

        private void InitData(EventKey.OnLoadDataCompleted obj)
        {
            if (obj.isCompletedAll) return;

            GetItem(StorageKey.Data);
            if (IsForceUnlockAll)
            {
                LocalDataStorage.isRemoveAds = true;
                AdsManager.Instance.SetRemovedAds();
            }

            DOVirtual.DelayedCall(0.5f, () =>
            {
                EventManager.OnInitCoin?.Invoke(LocalDataStorage.totalCoin);
                EventDispatcher.Instance.Dispatch(new EventKey.OnLoadDataCompleted { isCompletedAll = true });
            });
        }

        protected override void OnDestroy()
        {
            //
            EventManager.OnCompleteCutBox -= GetCompleteCutBox;
            EventDispatcher.Instance.RemoveListener<EventKey.OnLoadDataCompleted>(InitData);
        }

        public void UpdateNextBoardShape(int id)
        {
            LocalDataStorage.unlockShapesBoard[id] = true;
            SaveItem(StorageKey.Data);
        }
        public void ResetBoardShape()
        {
            for (int i = 0; i < LocalDataStorage.unlockShapesBoard.Count; i++)
            {
                LocalDataStorage.unlockShapesBoard[i] = false;
            }
            SaveItem(StorageKey.Data);
        }

        private void GetCompleteCutBox()
        {
            int colorIdx = GameManager.instance.curIdxColor;
            int shapeIdx = GameManager.instance.curShapeIdx;

            LocalDataStorage.shapeGlocerys[shapeIdx].unlockShapes = true;
            LocalDataStorage.shapeGlocerys[shapeIdx].idx = new Vector2(shapeIdx, colorIdx);
            SaveItem(StorageKey.Data);
        }

        public void GetData()
        {
            GetItem(StorageKey.Data);
        }

        void InitItem()
        {
            LocalDataStorage = new DataStorage();
            LocalDataStorage.totalCoin = 0;
            InitShape();
            InitShapeBoard();
            InitCharacter();
            InitBag();
            InitChair();
            InitWall();
            InitCarpet();
            InitFloor();
            InitFruitDecor();
            InitShapeTopic();
            InitClayMode();
        }

        private void InitClayMode()
        {
            LocalDataStorage.unlockFlowerPots = new List<bool>();
            //    var length = GameManager.instance.ClayDataSO.potSprites.Length;
            var length = GameManager.instance.ClayDataSO.potSprites.Length;
            for (int i = 0; i < length; i++)
            {
                LocalDataStorage.unlockFlowerPots.Add(i < 2);
            }

            LocalDataStorage.unlockAlphas = new List<bool>();
            //      var length2 = GameManager.instance.AlphaLearningDataSO.uppercaseSprites.Count;
            var length2 = GameManager.instance.AlphaLearningDataSO.uppercaseSprites.Count;
            for (int i = 0; i < length2; i++)
            {
                LocalDataStorage.unlockAlphas.Add(i < 5);
            }
        }

        private void InitShapeTopic()
        {
            var length = shapeTopicItems.Count;
            var lenIsFree = 2;
            LocalDataStorage.unlockShapeTopics = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockShapeTopics.Add(true);
                    continue;
                }
                LocalDataStorage.unlockShapeTopics.Add(false);
            }
        }

        public void ResetShapeBoard()
        {
            for (int i = 0; i < LocalDataStorage.unlockShapesBoard.Count; i++)
            {
                LocalDataStorage.unlockShapesBoard[i] = false;
            }
            SaveItem(StorageKey.Data);
        }
        public void ResetShapeGloceris()
        {
            for (int i = 0; i < LocalDataStorage.shapeGlocerys.Count; i++)
            {
                LocalDataStorage.shapeGlocerys[i].unlockShapes = false;
            }
            SaveItem(StorageKey.Data);
        }
        private void InitFruitDecor()
        {
            var length = fruitDecorItems.Count;
            var lenIsFree = 2;
            LocalDataStorage.unlockFruitDecorTopics = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockFruitDecorTopics.Add(true);
                    continue;
                }
                LocalDataStorage.unlockFruitDecorTopics.Add(false);
            }
        }

        private void InitFloor()
        {
            var length = floorSprites.Count;
            var lenIsFree = floorSprites.Count;
            LocalDataStorage.unlockFloorSkins = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockFloorSkins.Add(true);
                    continue;
                }
                LocalDataStorage.unlockFloorSkins.Add(false);
            }
        }
        private void InitCarpet()
        {
            var length = carpetSprites.Count;
            var lenIsFree = carpetSprites.Count;
            LocalDataStorage.unlockCarpetSkins = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockCarpetSkins.Add(true);
                    continue;
                }
                LocalDataStorage.unlockCarpetSkins.Add(false);
            }
        }
        private void InitWall()
        {
            var length = wallSprites.Count;
            var lenIsFree = wallSprites.Count;
            LocalDataStorage.unlockWallSkins = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockWallSkins.Add(true);
                    continue;
                }
                LocalDataStorage.unlockWallSkins.Add(false);
            }
        }
        private void InitChair()
        {
            var length = chairSprites.Count;
            var lenIsFree = chairSprites.Count;
            LocalDataStorage.unlockChairSkins = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockChairSkins.Add(true);
                    continue;
                }
                LocalDataStorage.unlockChairSkins.Add(false);
            }
        }
        private void InitCharacter()
        {
            var length = characterSprites.Count;
            var lenIsFree = 2;
            LocalDataStorage.unlockCharacters = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockCharacters.Add(true);
                    continue;
                }
                LocalDataStorage.unlockCharacters.Add(false);
            }
        }
        private void InitBag()
        {
            var length = bagSprites.Count;
            var lenIsFree = bagSprites.Count;
            LocalDataStorage.unlockBagSkins = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                if (i < lenIsFree)
                {
                    LocalDataStorage.unlockBagSkins.Add(true);
                    continue;
                }
                LocalDataStorage.unlockBagSkins.Add(false);
            }
        }

        void InitShapeBoard()
        {
            var length = 8;
            LocalDataStorage.unlockShapesBoard = new List<bool>();
            for (int i = 0; i < length; i++)
            {
                LocalDataStorage.unlockShapesBoard.Add(false);
            }
        }
        void InitShape()
        {
            LocalDataStorage.shapeGlocerys = new List<ShapeStruct>();
            //GameManager.instance.GetDataSO(DataSOType.Shape, () =>
            //{
            //});
            // var length = GameManager.instance.ShapeDataSO.emptySliceSprites.Count;
            var length = GameManager.instance.ShapeDataSO.emptySliceSprites.Count;
            for (int i = 0; i < length; i++)
            {
                LocalDataStorage.shapeGlocerys.Add(new ShapeStruct() { idx = Vector2.zero, unlockShapes = false });
            }
        }

        void GetItem(StorageKey key)
        {
            var dataJson = PlayerPrefs.GetString(keyName);
            Debug.Log($"Get {keyName} : " + dataJson.ToString());
            if (dataJson == null || dataJson == "")
            {
                InitItem();

                SaveItem(key);
                GetItem(key);
                return;
            }
            switch (key)
            {
                case StorageKey.Data:
                    LocalDataStorage = JsonUtility.FromJson<DataStorage>(dataJson);
                    break;
            }
        }
        public void SaveItem(StorageKey key)
        {
            string dataJson = key switch
            {
                StorageKey.Data => JsonUtility.ToJson(LocalDataStorage),
                _ => "",
            };

            Debug.Log("Save: " + dataJson.ToString());
            PlayerPrefs.SetString(keyName, dataJson);
            PlayerPrefs.Save();
        }

        public void UpdateCoin(int coin, bool isPlus, System.Action OnComplete = null, System.Action OnFail = null)
        {
            var coin_ = isPlus ? LocalDataStorage.totalCoin + coin : LocalDataStorage.totalCoin - coin;
            if (coin_ < 0)
            {
                OnFail?.Invoke();
            }
            else
            {
                LocalDataStorage.totalCoin = coin_;
                OnComplete?.Invoke();
                SaveItem(StorageKey.Data);
                EventManager.OnInitCoin?.Invoke(coin_);
            }
        }
    }

    [Serializable]
    public class DataStorage
    {
        public List<ShapeStruct> shapeGlocerys;
        public List<bool> unlockShapesBoard;
        public List<bool> unlockCharacters;
        public List<bool> unlockBagSkins;
        public List<bool> unlockChairSkins;
        public List<bool> unlockDoorSkins;
        public List<bool> unlockWallSkins;
        public List<bool> unlockCarpetSkins;
        public List<bool> unlockFloorSkins;
        public List<bool> unlockFruitDecorTopics;
        public List<bool> unlockFlowerPots;
        public List<bool> unlockAlphas;
        /// <summary>
        /// Shape Item of ScrollView in ShapeMode
        /// </summary>
        public List<bool> unlockShapeTopics;
        public bool isTryFree;
        public bool isRemoveAds;
        public bool FirstOpen = true;
        public int totalCoin;
    }

    [Serializable]
    public class ShapeStruct
    {
        public bool unlockShapes;
        /// <summary>
        /// X: Idx Color, Y: Idx Type Shape 
        /// </summary>
        public Vector2 idx;
    }
}