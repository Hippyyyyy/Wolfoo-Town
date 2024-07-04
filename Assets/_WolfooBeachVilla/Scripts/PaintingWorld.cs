using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static _WolfooShoppingMall.SelfHouseDataSO;

namespace _WolfooShoppingMall
{
    public class PaintingWorld : MonoBehaviour
    {
        [NaughtyAttributes.ShowIf("myType", Type.Color)]
        [SerializeField] PaintingColorName myColor;

        [SerializeField] private Type myType;

        public enum Type
        {
            Color,
            Sprite
        }
        private Tweener _tween;
        private SpriteRenderer spriteRender;

        protected PaintingObject TriggerPaintObj;

        public PaintingColorName Color { get => myColor; }
        public Sprite Sprite { get => spriteRender.sprite; }
        public Type PaintingType { get => myType; }

        private void Start()
        {
            spriteRender = GetComponent<SpriteRenderer>();
        }
        private void OnDestroy()
        {
            if (_tween != null) _tween?.Kill();
        }
        public void Drag()
        {
            OnDrag();
        }
        public virtual void EndDrag()
        {
            OnEndDrag();
        }
        void OnBeginDrag()
        {
        }
        void OnEndDrag()
        {
        }
        void OnDrag()
        {
            var mousePos = Input.mousePosition;
            var movepos = Camera.main.ScreenToWorldPoint(mousePos);
            movepos.z = 0;
            transform.position = movepos;
        }
        public void OnPainting(System.Action OnCompleted)
        {
            _tween = transform.DOScale(0.3f, 0.3f).SetEase(Ease.InBack).OnComplete(() =>
            {
                SoundBeachVillaManager.Instance.PlayOtherSfx(SoundTown<SoundBeachVillaManager>.SFXType.Scratch);
                OnCompleted?.Invoke();
            });
        }
        private void OnTriggerStay2D(Collider2D collision)
        {
            var paintingObj = collision.GetComponent<PaintingObject>();
            if (paintingObj)
            {
                TriggerPaintObj = paintingObj;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            var paintingObj = collision.GetComponent<PaintingObject>();
            if (TriggerPaintObj == paintingObj)
            {
                TriggerPaintObj = null;
            }
        }
    }
}
