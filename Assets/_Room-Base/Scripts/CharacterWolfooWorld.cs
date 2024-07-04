using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterWolfooWorld : BackItemWorld
    {
        [SerializeField] CharacterWolfooWorldAnimation myAnim;
        [SerializeField] Transform leftFingerArea;
        [SerializeField] Transform rightFingerArea;
        [SerializeField] Transform mouthArea;
        [SerializeField] Transform headArea;
        [SerializeField] Transform eyesArea;
        [SerializeField] Transform hatArea;
        [SerializeField] Transform body;

        private float distance;

        public Action<CharacterWolfooWorld> GetDrag;
        public Action<CharacterWolfooWorld> GetBeginDrag;
        public Action<CharacterWolfooWorld> GetEndDrag;
        private int maxLayerElementOrder;
        private BackItemWorld carryLeftItem;
        private BackItemWorld carryRightItem;
        private float lastPosX;

        protected override void RegisterEvent()
        {
            base.RegisterEvent();
            EventDispatcher.Instance.RegisterListener<RoomEventKey.Chair>(GetVerifiedChair);
            EventDispatcher.Instance.RegisterListener<RoomEventKey.BedWithCharacter>(GetVerifiedBed);
        }
        protected override void RemoveEvent()
        {
            base.RemoveEvent();
            EventDispatcher.Instance.RemoveListener<RoomEventKey.Chair>(GetVerifiedChair);
            EventDispatcher.Instance.RemoveListener<RoomEventKey.BedWithCharacter>(GetVerifiedBed);
        }

        public override void Setup()
        {
            if (IsAssigned) return;
            IsCharacter = true;
            IsClick = true;
            IsDragable = true;

            base.Setup();

            name = "Character - " + Id;

            maxLayerElementOrder = 0;
            foreach (var item in GetComponentsInChildren<SpriteRenderer>())
            {
                maxLayerElementOrder = item.sortingOrder > maxLayerElementOrder ? item.sortingOrder : maxLayerElementOrder;
            }

        }

        public void AssignToScroll()
        {
            transform.localPosition = Vector3.zero;
            Setup();
            transform.localScale *= 100;
        }

        protected override void OnEndDrag()
        {
            base.OnEndDrag();
            GetEndDrag?.Invoke(this);
        }
        protected override void OnDrag()
        {
            base.OnDrag();
            GetDrag?.Invoke(this);

            if (transform.position.x - lastPosX > 0.1f) transform.rotation = Quaternion.Euler(Vector3.up * 180);
            if (transform.position.x - lastPosX < -0.1f) transform.rotation = Quaternion.Euler(Vector3.zero);
            lastPosX = transform.position.x;
        }
        protected override void OnBeginDrag()
        {
            base.OnBeginDrag();
            myAnim.PlayIdle();
        }
        protected override void GetEndDragBackItem(BackItemWorld obj)
        {
            base.GetEndDragBackItem(obj);
            CompareWithHand(obj, true);
            CompareWithHand(obj, false);
            CompareWithHead(obj);
            CompareWithEyes(obj);
            CompareWithMouth(obj);
        }

        private void GetVerifiedBed(RoomEventKey.BedWithCharacter eventKey)
        {
            if (eventKey.wolfooWorld != this) return;

            var obj = eventKey.bed;
            JumpTo(obj.SleepArea.position, () =>
            {
                transform.SetParent(obj.transform);
                AttachTo(obj.transform);
                LayerOrder = 0;
                myAnim.PlaySleep();
            });
        }

        private void GetVerifiedChair(RoomEventKey.Chair eventKey)
        {
            if (eventKey.wolfooWorld != this) return;

            var obj = eventKey.seat;
            JumpTo(obj.transform.position, () =>
            {
                transform.SetParent(obj.transform);
                AttachTo(obj.transform);
                LayerOrder = 0;
                myAnim.PlaySit();
            });
        }

        private void CompareWithHand(BackItemWorld obj, bool isLeftHand)
        {
            if (obj.IsCarryItem)
            {
                var hand = isLeftHand ? leftFingerArea : rightFingerArea;
                distance = Vector2.Distance(obj.transform.position, hand.position);
                if (distance > 1)
                {
                    if (carryLeftItem != null && carryLeftItem == obj)
                    {
                        carryLeftItem = null;
                    }
                    if (carryRightItem != null && carryRightItem == obj)
                    {
                        carryRightItem = null;
                    }

                    return;
                }

                if (isLeftHand)
                {
                    if (carryLeftItem != null)
                    {
                        carryLeftItem.PlayMovingToGround();
                        carryLeftItem = obj;
                    }
                }
                else if (!isLeftHand)
                {
                    if (carryRightItem != null)
                    {
                        carryRightItem.PlayMovingToGround();
                        carryRightItem = obj;
                    }
                }

                maxLayerElementOrder++;
                //  LayerOrder = maxLayerElementOrder;
                obj.JumpTo(hand.position, () =>
                {
                    obj.transform.SetParent(hand);
                    obj.LayerOrder = maxLayerElementOrder;
                    myAnim.PlayHappy();
                });
            }
        }
        private void CompareWithHead(BackItemWorld obj)
        {
        }
        private void CompareWithEyes(BackItemWorld obj)
        {
        }
        private void CompareWithMouth(BackItemWorld obj)
        {
            if (obj.IsFood)
            {
                distance = Vector2.Distance(obj.transform.position, mouthArea.position);
                if (distance > 1) return;

                var beverage = obj.GetComponent<BeverageWorld>();
                if (beverage != null)
                {
                    beverage.Release(mouthArea.position, () =>
                    {
                        // Play Happy Animation
                        myAnim.PlayEat();
                        SoundCharacterManager.Instance.PlayWolfooInteresting();
                    });
                }

                var food = obj.GetComponent<FoodWorld>();
                if (food != null)
                {
                    food.JumpTo(mouthArea.position, () =>
                    {
                        food.Feed();
                        myAnim.PlayEat();
                        SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Eating1);
                    });
                }
            }
        }

        public void AttachTo(Transform _endParent)
        {
            StopMovingToGround();
            transform.SetParent(_endParent);
        }
        public void Hide(Transform endParent)
        {
            gameObject.SetActive(false);
            transform.SetParent(endParent);
            StopMovingToGround();
        }
        public void Show()
        {
            gameObject.SetActive(true);
            transform.SetParent(_myMap.ItemContent);
            //       OnDrag();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.01f);
            //       transform.rotation = Quaternion.Euler(Vector3.zero);
            OrderLayerIndex();
        }
    }
}
