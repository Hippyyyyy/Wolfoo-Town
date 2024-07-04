using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Sticker : ItemDrag
    {
        [SerializeField] ParticleSystem starFx;
        public void AssignDrag()
        {
            canDrag = true;
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);

            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = transform.DOScale(startScale + Vector3.one * 0.2f, 0.3f);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);

            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = transform.DOScale(startScale, 0.3f).OnComplete(() =>
            {
            });

            starFx.transform.SetParent(transform.parent);
            starFx.transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
            starFx.transform.position = transform.position;
            starFx.time = 0;
            starFx.Play();

            if (!canDrag) return;

            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragItem { sticker = this });
        }

        public void OnDefuse()
        {
            canDrag = false;
            if (scaleTween != null) scaleTween?.Kill();
            scaleTween = transform.DOScale(Vector3.zero, 0.3f).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}