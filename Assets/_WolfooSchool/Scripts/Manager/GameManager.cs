using _Base;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] CityType city;

        public static GameManager instance;

        public int curFlipIdx;
        public bool isCompare;
        public int curChooseItemIdx;
        public int curIdxMode;
        public GameObject curMode;
        private float ratio;
        public int curShapeIdx;
        public int curIdxColor;
        public DataSOType curModeType;
        public int curIdxSession = 0;
        public int curAlphaIdx;
        public int totalStar;
        public CaculateType curCaculate;
        public int curRange;
        public int lastSiblingIndex;
        public int countId = 0;
        public MainPanel mainPanel;
        public bool isChooseCharacerOpen; 
        public Transform ChooseCharacterZone;
        public GameObject MaskRotatePb;
        private int countBackItemTouched;

        List<int> idxCakeItems = new List<int>();

        private Vector3 newpos;
        private Vector3 pos;
        private Vector3 curPosition;
        public bool isLoadCompleted;

        public ItemsDataSO ItemDataSO;
        public ShapeModeDataSO ShapeDataSO;
        public AlphaLearningDataSO AlphaLearningDataSO;
        public ClayModeDataSO ClayDataSO;
        public CityType City { get => city; }
        public float ProcessTouchedAllItem
        {
            get
            {
                float tempCount = countBackItemTouched;
                countBackItemTouched = 0;
                return tempCount / countId;
            }
        }

        private void Awake()
        {
            if (instance == null)
                instance = this;
        }
        private void Start()
        {
            //  StartCoroutine(IAPManager.Instance.IEStart());
            Input.multiTouchEnabled = false;
            EventDispatcher.Instance.RegisterListener<EventKey.OnTouchBackItem>(GetTouchBackItem);

            // LoadData();
        }
        private void LoadData()
        {
            foreach (var item in LoadSceneManager.Instance.sceneData)
            {
                if (ItemDataSO == null)
                {
                    try
                    {
                        ItemDataSO = (ItemsDataSO)item;
                    }
                    catch (System.Exception) { }
                }
                if (ClayDataSO == null)
                {
                    try
                    {
                        ClayDataSO = (ClayModeDataSO)item;
                    }
                    catch (System.Exception) { }
                }
                if (ShapeDataSO == null)
                {
                    try
                    {
                        ShapeDataSO = (ShapeModeDataSO)item;
                    }
                    catch (System.Exception) { }
                }
                if (AlphaLearningDataSO == null)
                {
                    try
                    {
                        AlphaLearningDataSO = (AlphaLearningDataSO)item;
                    }
                    catch (System.Exception) { }
                }
            }
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnTouchBackItem>(GetTouchBackItem);
        }
        private void Update()
        {
            CheckLoadData();
        }
        void CheckLoadData()
        {
            if (isLoadCompleted) return;
            if (ItemDataSO == null) return;
            if (ClayDataSO == null) return;
            if (AlphaLearningDataSO == null) return;
            if (ShapeDataSO == null) return;
             
            isLoadCompleted = true;
            EventDispatcher.Instance.Dispatch(new EventKey.OnLoadDataCompleted { isCompletedAll = false });
        }
        private void GetTouchBackItem(EventKey.OnTouchBackItem obj)
        {
            if (obj.backItem != null)
                Debug.Log("Touched Item: " + obj.backItem.name);
            countBackItemTouched++;
        }

        public void GetDataSO(DataSOType type, System.Action OnComplete)
        {
            curModeType = type;
            OnComplete?.Invoke();
        }

        public int GetCoin(int idx)
        {
            if (ItemDataSO == null) GetDataSO(DataSOType.Items, null);
            return ItemDataSO.charactersPrice[idx].price;
        }
        public void GetCurrentPosition(Transform _transform)
        {
            curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPosition.z = 0;
            _transform.position = curPosition;
        }

        public void ScaleImage(Image item, float width, float height)
        {
            item.SetNativeSize();
            if (item.rectTransform.rect.height > item.rectTransform.rect.width)
            {
                ratio = item.rectTransform.rect.height / item.rectTransform.rect.width; // Scale with Max Height
                width = height / ratio;
                item.rectTransform.sizeDelta = new Vector2(width, height);
            }
            else
            {
                ratio = item.rectTransform.rect.width / item.rectTransform.rect.height;  // Scale with Max Width
                height = width / ratio;
                item.rectTransform.sizeDelta = new Vector2(width, height);
            }
        }
        public void CheckPos(Transform transform)
        {
            pos = Camera.main.WorldToScreenPoint(transform.position);
            if (pos.x > (Screen.safeArea.xMax))
            {
                newpos = new Vector3(Screen.safeArea.xMax, pos.y, pos.z);
                transform.position = new Vector3(Camera.main.ScreenToWorldPoint(newpos).x, transform.position.y, transform.position.z);
            }
            if (pos.x < Screen.safeArea.xMin)
            {
                newpos = new Vector3(Screen.safeArea.xMin, pos.y, pos.z);
                transform.position = new Vector3(Camera.main.ScreenToWorldPoint(newpos).x, transform.position.y, transform.position.z);
            }
            if (pos.y > Screen.safeArea.yMax)
            {
                newpos = new Vector3(pos.x, Screen.safeArea.yMax, pos.z);
                transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(newpos).y, transform.position.z);
            }
            if (pos.y < Screen.safeArea.yMin)
            {
                newpos = new Vector3(pos.x, Screen.safeArea.yMin, pos.z);
                transform.position = new Vector3(transform.position.x, Camera.main.ScreenToWorldPoint(newpos).y + 0.5f, transform.position.z);
            }
        }
    }
    public enum DataSOType
    {
        Items,
        Shape,
        AlphaLearning,
        ClayMode,
        GameDataSO,
    }
    public enum SkinBackItemType
    {
        None,
        Bag,
        Door,
        Wall,
        Carpet,
        Floor,
        Character
    }

    [System.Serializable]
    public struct PriceItem
    {
        public SkinBackItemType skinType;
        public int price;
    }


}