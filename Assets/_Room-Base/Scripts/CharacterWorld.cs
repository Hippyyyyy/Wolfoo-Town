using SCN;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterWorld : BackItemWorld
    {
        [SerializeField] Transform leftFingerArea;
        [SerializeField] Transform rightFingerArea;
        [SerializeField] Transform mouthArea;
        [SerializeField] Transform headArea;
        [SerializeField] Transform eyesArea;
        [SerializeField] Transform hatArea;
        [SerializeField] Transform buttArea;
        [SerializeField] Transform footArea;
        [SerializeField] CharacterAnimationManager animator;

        private float distance;

        public Action<CharacterWorld> GetDrag;
        public Action<CharacterWorld> GetBeginDrag;
        public Action<CharacterWorld> GetEndDrag;
        private int maxLayerElementOrder;
        private BackItemWorld carryLeftItem;
        private BackItemWorld carryRightItem;
        private float distanceFoot;

        public (BackItemWorld element, int maxLayer) OrderElement
        {
            get
            {
                if (carryLeftItem != null) return (carryLeftItem, maxLayerElementOrder);
                if (carryRightItem != null) return (carryRightItem, maxLayerElementOrder);

                return (null, maxLayerElementOrder);
            }
            private set { OrderElement = value; }
        }

        public Vector3 ComparedPosition
        {
            get
            {
                return transform.position - Vector3.up * distanceFoot;
            }
        }

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

            distanceFoot = transform.position.y - footArea.position.y;

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
        protected override void OrderLayerIndex(bool isMax = false)
        {
            if (isMax)
            {
                LayerOrder = 1000;
            }
            else
            {
                if (transform.parent == GameManager.instance.UiManager.ItemContent)
                {
                    LayerOrder = ((int)(transform.localPosition.y - distanceFoot) * -100);
                }
                else
                {
                    LayerOrder = ((int)(transform.localPosition.y - distanceFoot) * -100);
                }
            }
        }
        protected override void OnBeginDrag()
        {
            base.OnBeginDrag();
            animator.InitDrag();
        }
        protected override void OnDrag()
        {
            base.OnDrag();

            transform.position = new Vector3(transform.position.x, transform.position.y + distanceFoot, 0);
            animator.PlayDrag();

            GetDrag?.Invoke(this);
        }

        protected override void OnEndDrag()
        {
            base.OnEndDrag();
            animator.StopDrag();
            GetEndDrag?.Invoke(this);
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

        private void GetVerifiedBed(RoomEventKey.BedWithCharacter eventkey)
        {
            if (eventkey.newCharacter != this) return;

            var obj = eventkey.bed;
            var distanceButt = buttArea.position.y - transform.position.y;
            var endPos = new Vector3(obj.SleepArea.position.x, obj.SleepArea.position.y + distanceButt + distanceFoot);

            JumpTo(endPos, () =>
            {
                transform.SetParent(obj.transform);
                AttachTo(obj.transform);
                LayerOrder = 0;
            });
        }

        private void GetVerifiedChair(RoomEventKey.Chair eventKey)
        {
            if (eventKey.newCharacter != this) return;

            var obj = eventKey.seat;
            var distanceButt = buttArea.position.y - transform.position.y;
            var endPos = new Vector3(obj.transform.position.x, obj.transform.position.y + distanceButt + distanceFoot);
            JumpTo(endPos, () =>
            {
                transform.SetParent(obj.transform);
                AttachTo(obj.transform);
                LayerOrder = 0;
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
                obj.JumpTo(hand.position, () =>
                {
                    obj.transform.SetParent(hand);
                    obj.LayerOrder = maxLayerElementOrder;
                    obj.PlaySound();
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
                if(distance > 1) return;

                var beverage = obj.GetComponent<BeverageWorld>();
                if(beverage != null)
                {
                    beverage.Release(mouthArea.position, () =>
                    {
                        // Play Happy Animation
                        SoundCharacterManager.Instance.PlayWolfooInteresting();
                        animator.PlayEating();
                    });
                }

                var food = obj.GetComponent<FoodWorld>();
                if(food != null)
                {
                    food.JumpTo(mouthArea.position, () =>
                    {
                        food.Feed();
                        SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Eating1);
                        animator.PlayEating();
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
            OnDrag();
            OrderLayerIndex();
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -0.01f);
        }

    }
}
