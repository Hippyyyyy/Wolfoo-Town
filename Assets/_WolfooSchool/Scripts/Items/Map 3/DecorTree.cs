using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooSchool
{
    public class DecorTree : BackItem
    {
        [SerializeField] TreeAnimation treeAnimation;
        [SerializeField] ParticleSystem lightingFx;
        private float distance_;

        protected override void Start()
        {
            //    treeAnimation.gameObject.SetActive(false);
            treeAnimation.PlayIdle();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem obj)
        {
            base.GetEndDragItem(obj);

            if (obj.backItem == this) return;
            if (obj.waterBottle != null)
            {
                distance_ = Vector2.Distance(transform.position, obj.waterBottle.CompareZone.position);
                if (distance_ < 2)
                {
                    obj.waterBottle.OnPourWater(transform.position, () =>
                    {
                        treeAnimation.gameObject.SetActive(true);
                        treeAnimation.PlayAnim();
                        if (lightingFx != null) lightingFx.Play();
                    });
                }
                else
                {
                    //    obj.waterBottle.MoveToGround();
                }
            }
        }
    }
}