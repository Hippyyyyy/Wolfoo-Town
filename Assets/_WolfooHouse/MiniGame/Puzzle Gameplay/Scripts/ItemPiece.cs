using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SCN;
using Coffee;
using Coffee.UISoftMask;

namespace _WolfooCity.Minigames.Puzzle
{
    public class ItemPiece : MonoBehaviour, IPointerDownHandler
    {
        private SlotPiece slotCorrect;
        private int idPiece;
        private bool isSpawned = false;
        private Image imageFill;
        [SerializeField] private ButtonBase buttonPiece;
        private Vector3 baseScale;
        private Transform cacheParent;

        public bool IsSpawned { get => isSpawned; set => isSpawned = value; }
        public Transform CacheParent { get => cacheParent; set => cacheParent = value; }

        //public void OnDrag(PointerEventData eventData)
        //{
        //    if (isSpawned) return;
        //    isSpawned = true;
        //    transform.GetComponent<Image>().color = Color.gray;
        //    EventDispatcher.Instance.Dispatch(new EventKey.OnDragPiece { idPiece = idPiece });
        //}

        //public void OnEndDrag(PointerEventData eventData)
        //{
        //}
        private void Start()
        {
            buttonPiece.onClick.AddListener(OnButtonPieceClick);
            EventDispatcher.Instance.RegisterListener<EventKey.DoneDragItemPiece>(DoneDragItemPiece);
            EventDispatcher.Instance.RegisterListener<EventKey.ReturnItemPiece>(ReturnItemPiece);
        }
        private void OnDestroy()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.DoneDragItemPiece>(DoneDragItemPiece);
            EventDispatcher.Instance.RemoveListener<EventKey.ReturnItemPiece>(ReturnItemPiece);
        }
        private void DoneDragItemPiece(EventKey.DoneDragItemPiece data)
        {
            if (idPiece == data.idPiece)
            {
                IsSpawned = true;
                gameObject.SetActive(false);
            }
        }
        private void ReturnItemPiece(EventKey.ReturnItemPiece data)
        {
            if (idPiece == data.idPiece)
            {
                gameObject.SetActive(true);
                IsSpawned = false;
                imageFill.color = Color.white;
            }
        }
        private void OnButtonPieceClick()
        {
            if (IsSpawned) return;
            IsSpawned = true;
            imageFill.color = Color.grey;
            EventDispatcher.Instance.Dispatch(new EventKey.OnDragPiece { idPiece = idPiece, quaternion = transform.rotation });
        }
        public void InitPiece(Transform slotPiece, Image imageFill_, int idPiece_)
        {
            idPiece = idPiece_;
            slotCorrect = slotPiece.GetComponent<SlotPiece>();
            // imageFill.gameObject.SetActive(false);
            transform.GetComponent<RectTransform>().anchorMax = slotPiece.GetComponent<RectTransform>().anchorMax;
            transform.GetComponent<RectTransform>().anchorMin = slotPiece.GetComponent<RectTransform>().anchorMin;
            transform.localScale = Vector3.one;
            transform.GetComponent<Image>().sprite = slotPiece.transform.GetComponent<Image>().sprite;
            transform.GetComponent<Image>().SetNativeSize();
            transform.position = slotPiece.position;
            imageFill = Instantiate(imageFill_, imageFill_.transform);
            imageFill.transform.position = imageFill_.transform.position;
            imageFill.transform.SetParent(transform);
            imageFill.color = Color.white;
            imageFill.raycastTarget = false;
            imageFill.gameObject.SetActive(true);
        }
        public void RecyclePiece(Transform slotPiece, Image imageFill_, int idPiece_)
        {
            idPiece = idPiece_;
            slotCorrect = slotPiece.GetComponent<SlotPiece>();
            transform.SetParent(slotPiece);
            transform.localScale = Vector3.one;
            transform.GetComponent<RectTransform>().anchorMax = slotPiece.GetComponent<RectTransform>().anchorMax;
            transform.GetComponent<RectTransform>().anchorMin = slotPiece.GetComponent<RectTransform>().anchorMin;
            transform.GetComponent<Image>().sprite = slotPiece.transform.GetComponent<Image>().sprite;
            transform.GetComponent<Image>().SetNativeSize();
            transform.position = slotPiece.position;
            imageFill.color = Color.white;
            imageFill.transform.SetParent(imageFill_.transform);
            imageFill.transform.localPosition =Vector3.zero;
            imageFill.sprite = imageFill_.sprite;
            imageFill.transform.SetParent(transform);
            imageFill.SetNativeSize();
            transform.SetParent(CacheParent);
            IsSpawned = false;
            gameObject.SetActive(true);
        }
        public void OnHint()
        {
            IsSpawned = true;
            gameObject.SetActive(false);
            EventDispatcher.Instance.Dispatch(new EventKey.OnHintPiece { idPiece = idPiece, quaternion = transform.rotation });
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (IsSpawned) return;
            IsSpawned = true;
            imageFill.color = Color.grey;
            EventDispatcher.Instance.Dispatch(new EventKey.OnDragPiece { idPiece = idPiece, quaternion = transform.rotation, itemPiece = this });
        }
        //public void OnPointerDown(PointerEventData eventData)
        //{
        //    transform.GetComponent<Image>().color = Color.gray;
        //    EventDispatcher.Instance.Dispatch(new EventKey.OnDragPiece { idPiece = idPiece });
        //}
    }
}