using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [RequireComponent(typeof(BackItemWorld))]
    public class CustomRoomItem : MonoBehaviour
    {
        [SerializeField] CustomRoomItemUI myUi;
        [SerializeField] Transform[] rotateObjs;
        [SerializeField] BackItemWorld backItem;
        [SerializeField] SpriteRenderer borderSprite;
        [SerializeField] Color warningColor = Color.red;
        [SerializeField] Color verifiedColor = Color.white;

        [SerializeField] SpriteRenderer colorSpriteRender;
        [SerializeField] SpriteRenderer borderSpriteRender;

        private bool isAssigned;
        private bool isInDrangerZone;
        private RoomFloorConfig roomConfig;
        private PictureWorld picture;
        private bool scrollIsOpen = true;

        private Tweener _tweenMove;
        private Tweener _tweenScale;
        private Tweener _tween;
        /// <summary>
        /// Check object is Collisioning with Obstacle or Wall
        /// </summary>
        public bool IsInDrangerZone { get => isInDrangerZone; }

        private void Start()
        {
            Assign();
            EventSelfHouseRoom.OnClickDecorOption += GetClickDecorOption;
            backItem.OnChangeSorting += OnChangeLayerSorting;
        }
        private void OnDestroy()
        {
            EventSelfHouseRoom.OnClickDecorOption -= GetClickDecorOption;
            backItem.OnChangeSorting -= OnChangeLayerSorting;
            if (_tweenMove != null) _tweenMove?.Kill();
            if (_tweenScale != null) _tweenScale?.Kill();
        }

        private void OnChangeLayerSorting(int obj)
        {
            myUi.SortingOrder(backItem.LayerOrder);
        }


        private void GetClickDecorOption(bool isOpen)
        {
            this.scrollIsOpen = isOpen;
            if (isOpen)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }
        void PlayCurrentState()
        {
            if(picture)
            {
                if (!picture.IsTriggerEnterWithWall)
                {
                    PlayAnimInDanger();
                    backItem.StopTrigger();
                }
                return;
            }
            if (isInDrangerZone)
            {
                PlayAnimInDanger();
                backItem.StopTrigger();
            }
        }

        void PlayAnimInDanger()
        {
            var beginPos = transform.position;
            _tweenMove = transform.DOMoveY(beginPos.y + 0.3f, 1).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
        }

        public void Assign(Sprite colorSprite, Sprite borderSprite)
        {
            colorSpriteRender.sprite = colorSprite;
            borderSpriteRender.sprite = borderSprite;

            var myCollider = GetComponent<BoxCollider2D>();
            var anhObj = transform.Find("anh").gameObject;
            var imgCollider = anhObj.AddComponent<BoxCollider2D>();
            myCollider.size = imgCollider.size;
            myCollider.offset = new Vector2(0, imgCollider.offset.y);
            imgCollider.isTrigger = true;

            DestroyImmediate(imgCollider);
        }

        public void Assign()
        {
            if (isAssigned) return;
            isAssigned = true;

            verifiedColor = Color.white;
            if (backItem == null) backItem = GetComponent<BackItemWorld>();
            if (myUi != null)
            {
                myUi.OnClickRemove = OnRemove;
                myUi.OnClickRotate = OnRotate;
            }
            roomConfig = GameManager.instance.RoomConfig;
            picture = backItem.GetComponent<PictureWorld>();
        }

        private void OnRemove()
        {
            // Play ANim
            var chair = backItem.GetComponent<ChairWorld>();
            if (chair != null) chair.ReLeaseCharacter();

            _tweenScale = transform.DOScale(0.3f, 0.3f).SetEase(Ease.InBack)
            .OnComplete(() =>
            {
                Destroy(this.gameObject);
            });
        }

        private void OnRotate()
        {
            var rotate = rotateObjs[0].transform.localRotation.y == 0 ? 180 : 0;
            foreach (var item in rotateObjs)
            {
                item.localRotation = Quaternion.Euler(Vector3.up * rotate);
            }
        }

        public void Enable(bool isFirstActive = false)
        {
            backItem.enabled = false;
            backItem.AssignTempDrag(false, isFirstActive);

            myUi.Show();
            enabled = true;
            if (_tween != null) _tween?.Kill();
            _tween = borderSprite.DOFade(1, 0.5f);
        }
        public void Disable()
        {
            backItem.enabled = true;
            backItem.AssignTempDrag(true, false);

            myUi.Hide();
            enabled = false;
            if (_tween != null) _tween?.Kill();
            _tween = borderSprite.DOFade(0, 0.5f);
        }

        public void BeginDrag()
        {
            EventSelfHouseRoom.OnBeginDragCustomItem?.Invoke(this);
            backItem.BeginDrag();
        }
        public void Drag()
        {
            EventSelfHouseRoom.OnDragCustomItem?.Invoke(this);
            backItem.Drag();
        }
        public void EndDrag()
        {
            EventSelfHouseRoom.OnEndDragCustomItem?.Invoke(this);
            backItem.EndDrag();
        }

        private void OnMouseDown()
        {
            if (!enabled) return;
            if (_tweenMove != null) _tweenMove?.Kill();
            BeginDrag();
        }
        private void OnMouseDrag()
        {
            if (!enabled) return;
            Drag();
            SetBorderColor();
        }
        private void OnMouseUp()
        {
            if (!enabled) return;
            EndDrag();
            PlayCurrentState();
        }

        void SetBorderColor()
        {
            if (picture != null)
            {
                borderSprite.color = picture.IsTriggerEnterWithWall ? verifiedColor : warningColor;
                return;
            }

            if (isInDrangerZone)
            {
                borderSprite.color = warningColor;
            }
            else
            {
                borderSprite.color = verifiedColor;
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($" Custom Item {name} is Trigger Enter with: {collision.gameObject.name}");
            if (scrollIsOpen)
            {
                if (collision.gameObject.layer == roomConfig.OBSTACLE_LAYER)
                {
                    isInDrangerZone = true;
                }

                if (collision.GetComponent<WallWorld>())
                {
                    isInDrangerZone = true;
                }
            }

            if (collision.gameObject.CompareTag("DeathZone"))
            {
                Destroy(gameObject);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Debug.Log($" Custom Item {name} is Trigger Exit with: {collision.gameObject.name}");
            if (scrollIsOpen)
            {
                if (collision.gameObject.layer == roomConfig.OBSTACLE_LAYER)
                {
                    if (picture == null)
                    {
                        isInDrangerZone = false;
                    }
                }
            }
        }

        [NaughtyAttributes.Button]
        private void AssignSprite()
        {
            borderSprite.sortingOrder = -2;

        }
    }
}
