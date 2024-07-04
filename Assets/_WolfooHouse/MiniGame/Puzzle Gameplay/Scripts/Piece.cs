using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using SCN;
using Coffee.UISoftMask;

namespace _WolfooCity.Minigames.Puzzle
{
    public class Piece : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerUpHandler
    {
        [SerializeField] private Image imgFill;
        [SerializeField] private SlotPiece correctSlot;
        public bool IsInstalled;
        private Vector3 screenPoint;
        private Vector3 offset;
        private bool isMatch;
        private bool isReturnScroll;
        private bool isDragUpdate;
        private bool hasDrag = false;
        private bool isDragNor = false;
        private Vector3 startPos;
        private int idPiece;
        private Vector2 widthHeigh;
        private void Start()
        {
            widthHeigh = new Vector2(Screen.width, Screen.height);
        }
        public void OnValidateLevel(SlotPiece correctSlot_)
        {
            imgFill = transform.GetChild(0).GetComponent<Image>();
            correctSlot = correctSlot_;
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (!isDragNor && isDragUpdate) return;
            if (IsInstalled) return;
            //var mousePos = Input.mousePosition;
            //Vector3 curScreenPoint = new Vector3(mousePos.x, mousePos.y, screenPoint.z);
            //Vector3 curPosition = curScreenPoint + offset;
            //transform.position = curPosition;

            _WolfooShoppingMall.GameManager.instance.GetCurrentPosition(transform);
            CheckDistanceCorrect();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsInstalled) return;
         //   SoundManager.Instance.PlaySound(SoundType.PieceBeginDrag);
            transform.SetAsLastSibling();
            isDragUpdate = false;
            var mousePos = eventData.position;
            offset = transform.position - new Vector3(mousePos.x, mousePos.y, screenPoint.z);
            eventData.pointerPress = eventData.pointerEnter;
            transform.DOScale(Vector3.one * 1.1f, 0.1f);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsInstalled) return;
            transform.DOScale(Vector3.one, 0.1f);
            if (isMatch)
            {
                IsInstalled = true;
                correctSlot.IsFull = true;
                transform.DOMove(correctSlot.transform.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOPunchScale(Vector3.one * 0.15f, 0.1f, 2, 2).OnComplete(() =>
                    {
                        transform.DOScale(Vector3.one * 0.98f, 0.1f);
                        transform.SetParent(correctSlot.transform);
                        EventDispatcher.Instance.Dispatch(new EventKey.CheckProgress());
                        EventDispatcher.Instance.Dispatch(new EventKey.OnStarEffect { trans = transform });
                    });
                });
            }
            else if (isReturnScroll)
            {
                EventDispatcher.Instance.Dispatch(new EventKey.ReturnItemPiece { idPiece = idPiece });
                Destroy(gameObject);
            }
            else
            {
           //     SoundManager.Instance.PlaySound(SoundType.PieceEndDrag);
            }
        }
        private void Update()
        {
            if (isDragUpdate)
            {
                var mousePos = Input.mousePosition;
                Vector3 curScreenPoint = new Vector3(mousePos.x, mousePos.y, mousePos.z);
                Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint); /*curScreenPoint /*+ offset*/;
                curPosition.z = transform.position.z;
                transform.position = curPosition;
                CheckDistanceCorrect();
            }
        }
        public void OnHintPiece()
        {
            IsInstalled = true;
            hasDrag = true;
            isDragUpdate = false;
            isDragNor = false;
            transform.DOMove(correctSlot.transform.position, 1f).SetEase(Ease.InBack).OnComplete(() =>
            {
                transform.DOPunchScale(Vector3.one * 0.15f, 0.1f, 2, 2).OnComplete(() =>
                {
                    correctSlot.IsFull = true;
                    transform.DOScale(Vector3.one * 0.98f, 0.1f);
                    transform.SetParent(correctSlot.transform);
                    EventDispatcher.Instance.Dispatch(new EventKey.CheckProgress());
                    EventDispatcher.Instance.Dispatch(new EventKey.OnStarEffect { trans = transform });
                });
            });
        }
        public void InitPiece(Transform slotPiece, Image imageFill_, int idPiece_, Transform scrollTrans)
        {
            idPiece = idPiece_;
            correctSlot = slotPiece.GetComponent<SlotPiece>();
            // imageFill.gameObject.SetActive(false);
            //transform.GetComponent<RectTransform>().anchorMax = slotPiece.GetComponent<RectTransform>().anchorMax;
            //transform.GetComponent<RectTransform>().anchorMin = slotPiece.GetComponent<RectTransform>().anchorMin;
            transform.localScale = Vector3.one;
            transform.GetComponent<Image>().sprite = slotPiece.transform.GetComponent<Image>().sprite;
            transform.GetComponent<Image>().SetNativeSize();
            transform.position = slotPiece.position;
            imgFill = Instantiate(imageFill_, imageFill_.transform);
            imgFill.transform.position = imageFill_.transform.position;
            imgFill.transform.SetParent(transform);
            imgFill.color = Color.white;
            imgFill.raycastTarget = false;
            imgFill.gameObject.AddComponent<SoftMaskable>();
            imgFill.gameObject.SetActive(true);
            startPos = scrollTrans.transform.position;
            isDragUpdate = true;
        }
        private void CheckDistanceCorrect()
        {
            var distance = Vector2.Distance(transform.position, correctSlot.transform.position);
            isMatch = distance < 1.75f ? true : false;
            var distance2 = Vector2.Distance(new Vector3(transform.position.x, startPos.y, 0), startPos);
            isReturnScroll = distance2 < 2f ? true : false;
        }
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (hasDrag) return;
            hasDrag = true;
            var mousepos = eventData.position;
            //   offset = transform.position - new Vector3(mousepos.x, mousepos.y, 0);
            isDragUpdate = true;
            eventData.pointerPress = eventData.pointerEnter;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            isDragUpdate = false;
            if (IsInstalled || isDragNor) return;
            transform.DOScale(Vector3.one, 0.1f);
            isDragNor = true;
            EventDispatcher.Instance.Dispatch(new EventKey.DoneDragItemPiece { idPiece = idPiece });
            if (isMatch)
            {
                SoundManager.Instance.PlaySound(SoundType.PieceCorrect);
                IsInstalled = true;
                correctSlot.IsFull = true;
                transform.DOMove(correctSlot.transform.position, 0.1f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOPunchScale(Vector3.one * 0.15f, 0.1f, 2, 2).OnComplete(() =>
                    {
                        transform.DOScale(Vector3.one * 0.98f, 0.1f);
                        transform.SetParent(correctSlot.transform);
                        EventDispatcher.Instance.Dispatch(new EventKey.CheckProgress());
                        EventDispatcher.Instance.Dispatch(new EventKey.OnStarEffect { trans = transform });
                    });
                });
                return;

            }
            else if (isReturnScroll)
            {
                EventDispatcher.Instance.Dispatch(new EventKey.ReturnItemPiece { idPiece = idPiece });
                Destroy(gameObject);
            }
            else
            {
                SoundManager.Instance.PlaySound(SoundType.PieceEndDrag);
            }
        }
    }
}