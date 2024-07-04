using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class UIMediterraneanManager : UIManager
    {
        [SerializeField] Button decorPopupBtn;
        [SerializeField] Animator _anim;
        [SerializeField] string openName;
        [SerializeField] string closeName;
        [SerializeField] bool isOpenOption;
        [SerializeField] Transform decorArea;
        private Tweener _tweenTut;

        protected override void Start()
        {
            base.Start();

            CheckState();
            decorPopupBtn.onClick.AddListener(OnOpenDecorOption);
            GetTutRoom();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (_tweenTut != null) _tweenTut?.Kill();
        }
        void GetTutRoom()
        {
            _tweenTut = decorPopupBtn.transform.DOPunchScale(Vector3.one * 0.2f, 1, 4).SetLoops(-1, LoopType.Restart).SetDelay(2);
        }
        protected override void ClickCharacterPanel()
        {
            base.ClickCharacterPanel();
            decorArea.gameObject.SetActive(!IsPlayerPanelOpend);
        }
        protected override void ClickWolfooPanel()
        {
            base.ClickWolfooPanel();
            decorArea.gameObject.SetActive(!IsPlayerPanelOpend);
        }

        private void OnOpenDecorOption()
        {
            _tweenTut?.Kill();
            decorPopupBtn.transform.localScale = Vector3.one;

            isOpenOption = !isOpenOption;
            CheckState();
            if (isOpenOption)
            {
                HideFooter();
                GetTutRoom();
            }
            else
            {
                ShowFooter();
            }
            EventSelfHouseRoom.OnClickDecorOption?.Invoke(isOpenOption);
            SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Click);
        }

        private void CheckState()
        {
            //     BlockInput.gameObject.SetActive(isOpenOption);
            //     floor.Assign(!isOpenOption);
            if (isOpenOption)
            {
                _anim.Play(openName, 0, 0);
            }
            else
            {
                _anim.Play(closeName, 0, 0);
            }
        }
    }
}
