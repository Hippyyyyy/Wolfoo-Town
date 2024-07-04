using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class Clothing : BackItem
    {
        [SerializeField] int idAssign = -1;
        [SerializeField] Image[] frontImg;
        [SerializeField] Image behindImg;
        [SerializeField] Transform hangerZone;
        [SerializeField] Transform wearZone;
        [SerializeField] float _awakeScale =  1;
        [SerializeField] State state = State.Hang;
        private Tweener rotateTWeen;
        private Transform awakeParent;
        bool isHanger;
        private OperaPulledPackage myPackage;

        public int IdAssign { get => idAssign; }
        public bool IsHanger { get => isHanger; }
        public Transform AwakeParent { get => awakeParent; }
        public OperaPulledPackage Package { get => myPackage; }
        public enum State
        {
            Fold,
            Hang,
            TestWear
        }
        protected override void InitData()
        {
            base.InitData();
            canDrag = true;
            isComparePos = true;
            isScaleDown = true;
            startScale = Vector3.one * _awakeScale;

            SetState(state);
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { clothing = this });
        }

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;

            SetState(State.Fold);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { clothing = this, backitem = this });
        }

        public void AssignPackage(OperaPulledPackage package)
        {
            myPackage = package;
        }
        public void AssignItem(int id, Sprite frontSprite, Sprite behindSprite, Sprite foldingSprite, State state)
        {
            idAssign = id;

            foreach (var img in frontImg)
            {
                img.sprite = frontSprite;
                img.SetNativeSize();
            }
            behindImg.sprite = behindSprite;
            behindImg.SetNativeSize();
            image.sprite = foldingSprite;
            image.SetNativeSize();

            SetState(state);
        }
        void SetState(State state)
        {
            this.state = state;
            switch (state)
            {
                case State.Fold:
                    image.rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    image.gameObject.SetActive(true);
                    hangerZone.gameObject.SetActive(false);
                    if(wearZone != null)
                    wearZone.gameObject.SetActive(false);
                    break;
                case State.Hang:
                    image.rectTransform.pivot = new Vector2(0.5f, 0.85f);
                    image.gameObject.SetActive(false);
                    hangerZone.gameObject.SetActive(true);
                    if(wearZone != null)
                    wearZone.gameObject.SetActive(false);
                    break;
                case State.TestWear:
                    image.rectTransform.pivot = new Vector2(0.5f, 0.85f);
                    image.gameObject.SetActive(false);
                    hangerZone.gameObject.SetActive(false);
                    if(wearZone != null)
                    wearZone.gameObject.SetActive(true);
                    break;
            }
        }

        public void AssignItem(int id, Sprite frontSprite, Sprite behindSprite, Sprite foldingSprite, bool _isHanger = false)
        {
            idAssign = id;

            foreach (var img in frontImg)
            {
                img.sprite = frontSprite;
                img.SetNativeSize();
            }
            behindImg.sprite = behindSprite;
            behindImg.SetNativeSize();
            image.sprite = foldingSprite;
            image.SetNativeSize();
            image.enabled = true;

            hangerZone.gameObject.SetActive(false);
            awakeParent = transform.parent;

            isHanger = _isHanger;
            SetState(_isHanger ? State.Hang : State.Fold);
        }
        public void OnHanging(Vector3 _endPos, Transform _endParent, bool isHang = true)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            if (isHang)
            {
                SetState(State.Hang);

                if (rotateTWeen != null) rotateTWeen?.Kill();
                transform.rotation = Quaternion.Euler(Vector3.zero);
                rotateTWeen = transform.DORotate(Vector3.forward * 10, 0.15f).OnComplete(() =>
                {
                    rotateTWeen = transform.DORotate(Vector3.forward * -10, 0.25f).OnComplete(() =>
                    {
                        rotateTWeen = transform.DORotate(Vector3.zero, 0.15f).OnComplete(() =>
                        {

                        }).SetEase(Ease.OutBounce);
                    });
                });
            }
            else
            {
                KillScalling();
                tweenScale = transform.DOPunchScale(Vector3.one * 0.1f, 0.5f, 1);

                SetState(State.TestWear);
            }

            transform.position = _endPos;
            transform.SetParent(_endParent);
        }

        public void SetFolding()
        {
            isHanger = false;
            SetState(State.Fold);
        }

        public void OnGeneration()
        {
            canDrag = false;

            KillScalling();
            transform.localScale = Vector3.zero;
            transform.localPosition = Vector3.zero;
            tweenScale = transform.DOScale(_awakeScale, 0.5f).SetEase(Ease.OutBack).OnComplete(() =>
            {
                canDrag = true;
            });
        }

        public void OnCharacterSwear()
        {
            KillDragging();
            KillScalling();
            transform.localScale = Vector3.zero;

            if (SoundManager.instance == null) SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Correct);
            else SoundManager.instance.PlayOtherSfx(SfxOtherType.Correct);

            DOVirtual.DelayedCall(2, () =>
            {
                Destroy(gameObject);
            });
        }
    }
}