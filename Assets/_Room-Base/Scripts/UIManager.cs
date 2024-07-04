using _Base;
using DG.Tweening;
using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public abstract class UIManager : MonoBehaviour
    {
        [System.Serializable]
        public struct LimitArea
        {
            public Transform leftLimit;
            public Transform rightLimit;
            public Transform upLimit;
            public Transform downLimit;
        }

        [SerializeField] RoomFloorConfig roomConfig;
        [SerializeField] RoomBase myRoom; 
        [SerializeField] Button backBtn;
        [SerializeField] Button addWolfooBtn;
        [SerializeField] Button addCharacterBtn;
        [SerializeField] Button resetBtn;
        [SerializeField] NewPlayerPanel playerPanel;
        [SerializeField] Canvas uiCanvas;
        [SerializeField] LimitArea scrollLimit;


        public LimitArea limitArea;
        public bool IsPlayerPanelOpend { get; set; }

        private Tweener _tweenMove;

        public Transform ItemContent { get => myRoom.Content; }
        public RoomBase MyRoom
        {
            get => myRoom;
            set => myRoom = value;
        }

        public Action OnClickCharacterBtn;
        public Action OnClickWolfooBtn;
        public Action<bool> OnChangeStatePanel;

        protected virtual void Start()
        {
            backBtn.onClick.AddListener(OnBack);
            addCharacterBtn.onClick.AddListener(ClickCharacterPanel);
            addWolfooBtn.onClick.AddListener(ClickWolfooPanel);
            resetBtn.onClick.AddListener(OnResetRoom);

            playerPanel.Assign(scrollLimit, this);
        }

        protected virtual void OnDestroy()
        {
            EventDispatcher.Instance.Dispatch(new _WolfooCity.EventKey.OnDestroyScene());
            if (_tweenMove != null) _tweenMove?.Kill();
        }

        private void OnResetRoom()
        {
            LoadSceneManager.Instance.ReloadScene();
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }

        protected virtual void ClickCharacterPanel()
        {
            IsPlayerPanelOpend = !IsPlayerPanelOpend;
            OnClickCharacterBtn?.Invoke();

        //    addWolfooBtn.gameObject.SetActive(!IsPlayerPanelOpend);
            if (IsPlayerPanelOpend)
            {
                Open();
            }
            else
            {
                Close();
            }
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }

        protected virtual void ClickWolfooPanel()
        {
            IsPlayerPanelOpend = !IsPlayerPanelOpend;
            OnClickWolfooBtn?.Invoke();

            addCharacterBtn.gameObject.SetActive(!IsPlayerPanelOpend);
            if (IsPlayerPanelOpend)
            {
                Open();
                EventRoomBase.OnOpenCharacterPanel?.Invoke();
            }
            else
            {
                Close();
                EventRoomBase.OnCloseCharacterPanel?.Invoke();
            }
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }


        private void OnBack()
        {
            EventRoomBase.OnClickUIButton?.Invoke(roomConfig.BACK_BUTTON);
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }

        void Open()
        {
            OnChangeStatePanel?.Invoke(true);

            _tweenMove?.Kill();
            uiCanvas.renderMode = RenderMode.WorldSpace;
            _tweenMove = transform.DOMoveY(1.95f, 0.5f).OnComplete(() =>
            {
            });
        }
        void Close()
        {
            _tweenMove?.Kill();
            _tweenMove = transform.DOMoveY(0, 0.5f).OnComplete(() =>
            {
                uiCanvas.renderMode = RenderMode.ScreenSpaceCamera;
                OnChangeStatePanel?.Invoke(false);
            });
        }
        protected void HideFooter()
        {
            addWolfooBtn.gameObject.SetActive(false);
            addCharacterBtn.gameObject.SetActive(false);
        }
        protected void ShowFooter()
        {
            addWolfooBtn.gameObject.SetActive(true);
            addCharacterBtn.gameObject.SetActive(true);
        }
    }
}
