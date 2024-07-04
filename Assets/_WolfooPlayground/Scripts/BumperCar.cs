using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class BumperCar : BackItem
    {
        [SerializeField] Transform sitArea;
        [SerializeField] float power = 10;
        private Rigidbody2D rgbd;
        private float distance_;
        private BackItem myItem;

        protected override void InitData()
        {
            base.InitData();
            canClick = true;

            rgbd = GetComponent<Rigidbody2D>();
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

            transform.SetAsFirstSibling();
            rgbd.AddForce(new Vector2(Random.Range(-power, power), Random.Range(0, power)));
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.character != null)
            {
                if (item.character == myItem)
                {
                    myItem.transform.SetParent(Content.transform);
                    myItem.transform.localScale = Vector3.one * 0.8f;
                    myItem = null;
                }
            }
            if (item.newCharacter != null)
            {
                if (item.newCharacter == myItem)
                {
                    myItem.transform.SetParent(Content.transform);
                    myItem.transform.localScale = Vector3.one * 0.8f;
                    myItem = null;
                }
            }
        }
        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (myItem != null) return;
            if (item.character != null)
            {
                distance_ = Vector2.Distance(item.character.transform.position, sitArea.position);
                if (distance_ <= 1)
                {
                    myItem = item.character;
                    item.character.OnSitToChair(sitArea.position, sitArea);
                    myItem.transform.localScale = Vector3.one;
                }
            }
            if (item.newCharacter != null)
            {
                distance_ = Vector2.Distance(item.newCharacter.transform.position, sitArea.position);
                if (distance_ <= 1)
                {
                    myItem = item.newCharacter;
                    item.newCharacter.OnSitToChair(sitArea.position, sitArea);
                    myItem.transform.localScale = Vector3.one;
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            SoundPlaygroundManager.Instance.PlayOtherSfx(SoundTown<SoundPlaygroundManager>.SFXType.CarImpact);
        }
    }
}