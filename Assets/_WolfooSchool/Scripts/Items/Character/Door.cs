using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using SCN;

namespace _WolfooSchool
{
    public class Door : BackItem
    {
        [SerializeField] List<Sprite> sprites;
        [SerializeField] float scaleLess = 0f;
        [SerializeField] Vector3 posLess;
        [SerializeField] Button playgameBtn;
        [SerializeField] PanelType panelType;

        private Tween fadeTween;
        private Type curType = Type.Open;
        bool isFirstTouch = true;

        public Type CurType { get => curType; }

        public enum Type
        {
            Close,
            Open
        }

        protected override void InitData()
        {
            base.InitData();
            skinType = SkinBackItemType.Door;

            if (playgameBtn != null)
            {
                playgameBtn.onClick.AddListener(OnPLaygame);
                playgameBtn.gameObject.SetActive(false);
                playgameBtn.transform.DOPunchPosition(Vector3.up * 15, 1, 3).SetLoops(-1, LoopType.Restart);
            }

            SetStateDoor();
        }

        private void OnPLaygame()
        {
            OnClick();
            GUIManager.instance.GetCurFloor().Hide();
            EventManager.OpenPanel?.Invoke(panelType);
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            OnClick();
        }
        void OnClick()
        {
            if (isFirstTouch) { isFirstTouch = false; }
            else { DisableDance(); }

            fadeTween?.Kill();

            if (curType == Type.Open) curType = Type.Close;
            else curType = Type.Open;

            EventDispatcher.Instance.Dispatch(new EventKey.OnClickBackItem { door = this });
            SetStateDoor();
        }
        void SetStateDoor()
        {
            if (image == null) image = GetComponent<Image>();
            image.sprite = sprites[(int)curType];

            if (curType == Type.Open)
            {
                if (playgameBtn != null)
                    playgameBtn.gameObject.SetActive(true);
            }
            else
            {
                if (fadeTween != null) fadeTween?.Kill();

                if (playgameBtn != null)
                    playgameBtn.gameObject.SetActive(false);
            }
        }
    }
}