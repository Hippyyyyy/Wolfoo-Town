using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class Flour : Food
    {
        [SerializeField] ParticleSystem smokeFx;
        private bool isTransformToBread;

        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { backitem = this, food = this, flour = this });
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { backitem = this, food = this });
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);

            if (!isTransformToBread && item.egg != null)
            {
                var distance = Vector2.Distance(item.egg.transform.position, transform.position);
                if (distance < 1)
                {
                    isTransformToBread = true;
                    var bread = GameManager.instance.CreateBread(transform.parent, transform.position, transform.rotation);
                    if (smokeFx != null)
                    {
                        smokeFx.transform.SetParent(bread.transform);
                        smokeFx.Play();
                    }

                    Destroy(item.egg.gameObject);
                    Destroy(gameObject);
                }
            }
        }
    }
}
