using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System;
using SCN;
using SCN.IAP;
using _Base;

namespace _WolfooShoppingMall
{
    public class PlayerPanel : MonoBehaviour
    {
        [SerializeField] Button addCharacterBtn;
        [SerializeField] ScrollRect characterScrollView;
        [SerializeField] Text coinText;
        [SerializeField] ParticleSystem rainbowFx;
        [SerializeField] List<Transform> moveScrollTrans;
        [SerializeField] Button backBtn;
        [SerializeField] Transform signalBoard;
        [SerializeField] Button iapBtn;
        [SerializeField] GameObject coverBackImg;
        [SerializeField] Transform newCharacterScrollItemPb;

        private Tweener moveScrollTween;
        private Transform curFloor;
        private Tweener moveBackTween;
        private bool isOpening;
        private Tween delayTween;
        private int curCoinPlus;
        private CharacterData data;
        private List<bool> localData;
        /// <summary>
        /// 0: is Off - 1: is On
        /// </summary>
        private int statePanel = 0;
        private PanelType parentPanelType;
        private PanelType curPanel;
        private Tweener rotateTween;
        private bool isStart = true;
        private List<Character> characters = new List<Character>();
        private Tweener _tweenScale;
        private Tween _tweenDelay;
        private CityType cityType;
        private bool isScroll;

        public bool IsOpen { get => statePanel == 1; }

        enum StateChooseCharacter
        {
            Close,
            Open,
        }

        public void AssignCity(CityType cityType)
        {
            this.cityType = cityType;
        }

        #region INPUT METHOD
        public void ClickResetMap()
        {
            LoadSceneManager.Instance.ReloadScene();
        }
        #endregion

        private void Awake()
        {
            signalBoard.rotation = Quaternion.Euler(Vector3.forward * -90);
        }

        void Start()
        {
            addCharacterBtn.onClick.AddListener(OnOpenCharacterPanel);
            backBtn.onClick.AddListener(OnBack);
            iapBtn.onClick.AddListener(OnOpenIapPanel);

            EventDispatcher.Instance.RegisterListener<EventKey.OnRemoveAds>(GetBuyRemoveAds);

            if (GameManager.instance.IsLoadCompleted) { InitData(); }
            else { EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(InitData); }

            if (Camera.main.aspect >= 1.5)
            {
                addCharacterBtn.transform.localScale = Vector3.one * 0.8f;
            }

            PunchScale();
            IAPManager.Instance.OnBuyDone = GetBuyDone;
        }

        private void InitData()
        {
            if (DataSceneManager.Instance.ItemDataSO != null) data = DataSceneManager.Instance.ItemDataSO.CharacterData;
            if (data.characterPbs == null) data = DataSceneManager.Instance.MainCharacterData.CharacterData; 

            localData = DataSceneManager.Instance.LocalDataStorage.unlockCharacters;

            coverBackImg.SetActive(false);

            curFloor = transform.parent;
            //
            StartCoroutine(InitNewCharacterScroll());
            StartCoroutine(InitWolfooScroll());
        }

        private void GetBuyDone()
        {
            for (int i = 0; i < characters.Count; i++)
            {
                if (AdsManager.Instance.IsRemovedAds)
                {
                    characters[i].CharacterScrollView.Unlock();
                }
            }
        }

        private void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragItem>(GetEndDragItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnBeginDragItem>(GetBeginDragItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnWatchAds>(GetWatchAds);
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);

        //    iapBtn.gameObject.SetActive(!AdsManager.Instance.IsRemovedAds);

            if (!isStart)
            {
                localData = DataSceneManager.Instance.LocalDataStorage.unlockCharacters;
                for (int i = 0; i < characters.Count; i++)
                {
                    if (localData[i]) characters[i].CharacterScrollView.Unlock();
                }
            }
            isStart = false;
        }
        private void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragItem>(GetEndDragItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragBackItem>(GetEndDragBackItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnWatchAds>(GetWatchAds);
            EventDispatcher.Instance.RemoveListener<EventKey.OnBeginDragItem>(GetBeginDragItem);
        }

        private void OnDestroy()
        {
            if (delayTween != null) delayTween?.Kill();
            if (_tweenDelay != null) _tweenDelay?.Kill();
            if (_tweenScale != null) _tweenScale?.Kill();
            EventDispatcher.Instance.RemoveListener<EventKey.OnRemoveAds>(GetBuyRemoveAds);
            EventDispatcher.Instance.RemoveListener<EventKey.OnLoadDataCompleted>(InitData);

            if (parentPanelType == PanelType.Intro)
            {
                EventDispatcher.Instance.Dispatch(new _WolfooCity.EventKey.OnDestroyScene());
            }
        }

        private void GetBeginDragItem(EventKey.OnBeginDragItem obj)
        {
            if(obj.characterScroll != null)
            {
                StopScrolling();
            }
        }
        void GetBuyRemoveAds()
        {
            //  FirebaseManager.instance.LogBuyIAP("");
            iapBtn.gameObject.SetActive(false);
        }
        private void OnOpenIapPanel()
        {
            _Base.FirebaseManager.instance.LogClick("iap", cityType.ToString());
            StartCoroutine(IAPManager.Instance.IEStart());
            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);
        }

        public void OnShowBoard()
        {
            rotateTween = signalBoard.DORotate(Vector3.zero, 0.5f).SetEase(Ease.OutBack);
        }

        private void OnBack()
        {
            _Base.FirebaseManager.instance.LogClick("back", cityType.ToString());
            EventDispatcher.Instance.Dispatch(new EventKey.OnBackClick { parentPanelType = parentPanelType, myPanelType = curPanel });
        }
        public void AssignBackFloor(PanelType parentPanelType, PanelType curPanel)
        {
            this.parentPanelType = parentPanelType;
            this.curPanel = curPanel;
        }

        private void GetWatchAds(EventKey.OnWatchAds obj)
        {
        }

        void PunchScale()
        {
            _tweenDelay = DOVirtual.DelayedCall(5, () =>
            {
                _tweenScale = addCharacterBtn.transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 2)
                .OnComplete(() =>
                {
                    PunchScale();
                });
            });
        }
        private void GetEndDragBackItem(EventKey.OnEndDragBackItem obj)
        {
            if (obj.character != null)
            {
                if (statePanel == 0) return;
                if (obj.character.transform.position.y - moveScrollTrans[1].position.y <= 1)
                {
                    obj.character.AssignToScrollItem();
                    ScrollTo(obj.character.transform);
                }
            }
            if(obj.newCharacter != null)
            {
                if (statePanel == 0) return;
                if (obj.newCharacter.transform.position.y - moveScrollTrans[1].position.y <= 1)
                {
                    obj.newCharacter.AssignToScrollItem();
                    ScrollTo(obj.newCharacter.transform);
                }
                else
                {
                    obj.newCharacter.AssginToFloor();
                }
            }
        }
        private void GetEndDragItem(EventKey.OnEndDragItem obj)
        {
            if (obj.characterScroll != null)
            {
                if (statePanel == 0) return;
                if (obj.characterScroll.transform.position.y - moveScrollTrans[1].position.y <= 1)
                {
                    obj.characterScroll.SetToScrollView();
                    ScrollTo(obj.characterScroll.transform);
                }
                else
                {
                    obj.characterScroll.SetToFloor(curFloor.GetComponent<Floor>().ScrollRect.content);
                }
            }
        }
        private void Scrolling(Vector3 endPos, bool isScrollToRight)
        {
            if (characterScrollView.content.localPosition.x < -characterScrollView.content.sizeDelta.x || characterScrollView.content.localPosition.x > 0)
            {
                isScroll = false;
            }

            if (characterScrollView.content.anchoredPosition.x > endPos.x && isScrollToRight)
            {
                characterScrollView.content.anchoredPosition += Vector2.left * 100;
                return;
            }

            if (characterScrollView.content.anchoredPosition.x < endPos.x && !isScrollToRight)
            {
                characterScrollView.content.anchoredPosition += Vector2.right * 100;
                return;
            }

            isScroll = false;
        }
        public void StopScrolling()
        {
            characterScrollView.velocity = Vector2.zero;
        }
        public void ScrollTo(Transform endTarget)
        {
            Canvas.ForceUpdateCanvases();

            var endPos = (Vector2)characterScrollView.transform.InverseTransformPoint(characterScrollView.content.position)
                    - (Vector2)characterScrollView.transform.InverseTransformPoint(endTarget.position)
                    + Vector2.right * 1000;
            endPos.y = 0;
            var isScrollToRight = characterScrollView.content.anchoredPosition.x > endPos.x;

            isScroll = true;
            while (isScroll)
            {
                Scrolling(endPos, isScrollToRight);
            }
        }

        IEnumerator InitNewCharacterScroll()
        {
            yield return new WaitForSeconds(0.5f);

            var totalCharacter = CharacterTransporter.TotalCharacterHolder;
            var characterUiPb = CharacterTransporter.UIInRoomCharacter;
            Debug.Log("Data Character: " + totalCharacter);

            for (int i = 0; i < totalCharacter; i++)
            {
                var itemView = Instantiate(newCharacterScrollItemPb, characterScrollView.content);
                var itemUi = Instantiate(characterUiPb, itemView);

                var characterDataSet = CharacterTransporter.GetDataSet(i);
                itemUi.CharacterPartHelper.Assign(characterDataSet);
                var item = itemUi.transform.GetComponentInChildren<NewCharacterUI>();
                Debug.Log("Scroll Item: " + item);

                item.Assign(characterDataSet.assetAge);
                item.AssignToScrollView();
                item.transform.SetAsFirstSibling();
          //      itemUi.transform.GetComponentInChildren<NewCharacterScrollItem>(true).Assign(characterScrollView, itemWorld.GetComponent<CharacterWorld>());
          //
            }
        }

        private IEnumerator InitWolfooScroll()
        {
            yield return new WaitForSeconds(0.6f);

            for (int i = 0; i < data.characterPbs.Length; i++)
            {
                var character = Instantiate(data.characterPbs[i], characterScrollView.content);
                var rainbow = Instantiate(data.rainbowFxPb, transform);
                if (Camera.main.aspect >= 1.5)
                {
                    character.transform.localScale = Vector3.one * 0.8f;
                }
                character.CharacterScrollView.AssignItem(i, data.sprites[i], character, !localData[i], rainbow);
                characters.Add(character);

                if (AdsManager.Instance.IsRemovedAds)
                {
                    character.CharacterScrollView.Unlock();
                }
            }
        }

        private void OnOpenCharacterPanel()
        {
            if (curFloor == null) return;

            if (isOpening) return;
            isOpening = true;

            moveScrollTween?.Kill();
            moveBackTween?.Kill();

            statePanel = 1 - statePanel;

            if (statePanel == 1) // On
            {
                coverBackImg.SetActive(true);
                moveBackTween = curFloor.transform.DOMoveY(UISetupManager.Instance.moveFloorTrans[statePanel].position.y, 0.5f)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        moveScrollTween = characterScrollView.transform
                            .DOMoveY(moveScrollTrans[statePanel].position.y, 0.5f)
                            .SetEase(Ease.OutBack)
                            .OnComplete(() =>
                            {
                                isOpening = false;
                            });
                    });
            }
            else // Off
            {
                moveBackTween = curFloor.transform.DOMoveY(UISetupManager.Instance.moveFloorTrans[statePanel].position.y, 0.5f)
                    .SetEase(Ease.Linear);
                moveScrollTween = characterScrollView.transform
                    .DOMoveY(moveScrollTrans[statePanel].position.y, 0.5f)
                    .SetEase(Ease.InBack)
                    .OnComplete(() =>
                    {
                        coverBackImg.SetActive(false);
                        isOpening = false;
                    });
            }
        }
    }
}