using _Base;
using SCN;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class GameManager : MonoBehaviour
    {
        [NaughtyAttributes.OnValueChanged("OnChangeTestingValue")]
        [SerializeField] RoomFloorConfig roomConfig;
        [SerializeField] public GameObject RotateMaskItempb;
        [SerializeField] Food breadPb;

        private UIManager uiManager;

        public static GameManager instance;
        private float ratio;
        public bool CanMoveFloor { get; set; } = true;

        public int countId;
        public int CurPriority;
        public int CurFloorIdx;

        private Vector3 newpos;
        private Vector3 pos;
        private Vector3 curPosition;
        private Edge[] edges;
        private PanelType parentPanelType;
        private PanelType curPanel;
        private int countBackItemTouched;

        public CityType City { get => GUIManager.instance.CurrentMapController; }

        public RoomFloorConfig RoomConfig { get => roomConfig; }
        public ScrollRect curFloorScroll { get; private set; }
        public Transform curGround { get; private set; }
        public bool IsLoadCompleted { get; private set; }
        public UIManager UiManager { get => uiManager; }
        public BackItemWorld CurrentDragBackItem { get; private set; }
        public bool BackItemIsDragging { get => CurrentDragBackItem != null; }
        public float ProcessTouchedAllItem => (float)countBackItemTouched / countId;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            uiManager = FindAnyObjectByType<UIManager>();
        }
        private void Start()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnTouchBackItem>(GetTouchBackItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(GetLoadDataCompleted);
            EventRoomBase.OnInitBackItem += GetInitItemWorld;
            EventRoomBase.OnClickUIButton += GetClickUIButton;
            EventRoomBase.OnBeginDragBackItem += OnBeginDragBackItem;
            EventRoomBase.OnEndDragBackItem += OnEndDragBackItem;

        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnLoadDataCompleted>(GetLoadDataCompleted);
            EventDispatcher.Instance.RemoveListener<EventKey.OnTouchBackItem>(GetTouchBackItem);
            EventRoomBase.OnInitBackItem -= GetInitItemWorld;
            EventRoomBase.OnClickUIButton -= GetClickUIButton;
            EventRoomBase.OnBeginDragBackItem -= OnBeginDragBackItem;
            EventRoomBase.OnEndDragBackItem -= OnEndDragBackItem;
        }

        private void TestTutorial()
        {
            //var tutorial = TutorialController.Instance.CreateTutorial();
            //var step1 = TutorialController.Instance.CreateStep<TutorialWithBG>();
            //var step2 = TutorialController.Instance.CreateStep<TutorialWithBG>();
            //var step3 = TutorialController.Instance.CreateStep<TutorialWithBG>();
            //var step4 = TutorialController.Instance.CreateStep<TutorialWithBG>();
            //tutorial.Register(step1);
            //tutorial.Register(step2);
            //tutorial.Register(step3);
            //tutorial.Register(step4);

            //step1.Setup(resetBtnTest.transform);
            //step1.OnClickTutorial += () =>
            //{
            //    step1.Stop();
            //};
            //step1.OnTutorialComplete += () =>
            //{
            //    step2.Setup(resetBtnTest2.transform);
            //    step2.Play();
            //};
            //step2.OnClickTutorial += () =>
            //{
            //    step3.Setup(resetBtnTest3.transform);
            //    step2.Stop();
            //};
            //step2.OnTutorialComplete += () =>
            //{
            //    step3.Play();
            //};
            //step3.OnClickTutorial += () =>
            //{
            //    step4.Setup(resetBtnTest4.transform);
            //    step3.Stop();
            //};
            //step3.OnTutorialComplete += () =>
            //{
            //    step4.Play();
            //};
            //step4.OnClickTutorial += () =>
            //{
            //    step4.Stop();
            //};

            //resetBtnTest.onClick.AddListener(() => { if (step1.IsPlaying) step1.OnClickTutorial?.Invoke(); });
            //resetBtnTest2.onClick.AddListener(() => { if (step2.IsPlaying) step2.OnClickTutorial?.Invoke(); });
            //resetBtnTest3.onClick.AddListener(() => { if (step3.IsPlaying) step3.OnClickTutorial?.Invoke(); });
            //resetBtnTest4.onClick.AddListener(() => { if (step4.IsPlaying) step4.OnClickTutorial?.Invoke(); });
            //DOVirtual.DelayedCall(3, () =>
            //{
            //    step1.Play();
            //});
        }

        private void GetTouchBackItem(EventKey.OnTouchBackItem obj)
        {
            countBackItemTouched++;
        }

        public Food CreateBread(Transform parent, Vector3 startPos, Quaternion rotation)
        {
            var bread = Instantiate(breadPb, startPos, rotation, parent);
            bread.AssignDrag();
            return bread;
        }

        private void OnEndDragBackItem(BackItemWorld obj)
        {
            if (obj == CurrentDragBackItem) CurrentDragBackItem = null;
        }

        private void OnBeginDragBackItem(BackItemWorld obj)
        {
            CurrentDragBackItem = obj;
        }

        public void AssignBackFloor(PanelType parentPanelType, PanelType curPanel)
        {
            this.parentPanelType = parentPanelType;
            this.curPanel = curPanel;
        }

        private void GetClickUIButton(string obj)
        {
            if (obj.Equals(roomConfig.BACK_BUTTON))
            {
                EventDispatcher.Instance.Dispatch(new EventKey.OnBackClick { parentPanelType = parentPanelType, myPanelType = curPanel });
            }
        }

        private void GetInitItemWorld(BackItemWorld obj)
        {
        }
        public int GenerateBackItemId(BackItemWorld obj)
        {
            obj.Setup(uiManager);
            countId++;
            return countId;
        }

        private void GetLoadDataCompleted(EventKey.OnLoadDataCompleted obj)
        {
            IsLoadCompleted = true;
        }

        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.scrollRect != null)
            {
                curFloorScroll = obj.scrollRect;
                curGround = obj.ground;
            }
        }

        public void GetCurrentPosition(Transform _transform)
        {
            curPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            curPosition.z = 0;
            _transform.position = curPosition;
        }

        public static void GetScreenRatio(System.Action OnNormal = null, System.Action OnLong = null, System.Action OnWide = null)
        {
            if (Camera.main.aspect >= 1.7)
            {
                //   Debug.Log("16:9");
                OnLong?.Invoke();
            }
            else if (Camera.main.aspect >= 1.5)
            {
                //     Debug.Log("3:2");
                OnNormal?.Invoke();
            }
            else
            {
                //    Debug.Log("4:3");
                OnWide?.Invoke();
            }
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
        public Edge[] GetEdges(Transform[] limitZones)
        {
        //    if (edges == null || limitZones.Length > edges.Length)
                edges = new Edge[limitZones.Length];


            for (int i = 0; i < edges.Length; i++)
            {
                var nexIdx = i + 1;
                if (nexIdx > limitZones.Length)
                {
                    edges[i] = null;
                    continue;
                }
                if (nexIdx == limitZones.Length) nexIdx = 0;
                var edge = new Edge(limitZones[i], limitZones[nexIdx]);
                edges[i] = edge;
            }

            return edges;
        }

        public bool Is_inside(Vector2 itemPos, Edge[] edges)
        {
            var cnt = 0;
            foreach (var edge in edges)
            {
                var x1 = edge.pointArea1.position.x;
                var y1 = edge.pointArea1.position.y;
                var x2 = edge.pointArea2.position.x;
                var y2 = edge.pointArea2.position.y;

                if ((itemPos.y < y1) != (itemPos.y < y2) && itemPos.x < x1 + ((itemPos.y - y1) / (y2 - y1)) * (x2 - x1))
                {
                    cnt += 1;
                }
            }
            Debug.Log("IS_Inside: " + (cnt % 2 == 1));
            return cnt % 2 == 1;
        }
        public bool Is_inside(Vector2 itemPos, Transform[] limitZones)
        {
            var edges = GetEdges(limitZones);

            var cnt = 0;
            foreach (var edge in edges)
            {
                if (edge == null) break;
                var x1 = edge.point1.x;
                var y1 = edge.point1.y;
                var x2 = edge.point2.x;
                var y2 = edge.point2.y;

                if ((itemPos.y < y1) != (itemPos.y < y2) && itemPos.x < x1 + ((itemPos.y - y1) / (y2 - y1)) * (x2 - x1))
                {
                    cnt += 1;
                }
            }
            Debug.Log("IS_Inside: " + (cnt % 2 == 1));
            return cnt % 2 == 1;
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
        GameDataSO,
        Mode,
        BackItems,
    }


}