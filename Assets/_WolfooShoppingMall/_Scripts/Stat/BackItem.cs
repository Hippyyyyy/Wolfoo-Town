using _Base;
using DG.Tweening;
using SCN;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public abstract class BackItem : MonoBehaviour,
        IBeginDragHandler,
        IEndDragHandler,
        IDragHandler,
        IPointerClickHandler,
        IPointerDownHandler,
        IPointerUpHandler
    {
        [SerializeField] protected bool isDance;
        [SerializeField] protected int assignPriorityy = 1;
        [SerializeField] Ease ease = Ease.OutBounce;
        [SerializeField] protected Image image;
        [SerializeField] Transform standZone;
        [SerializeField] protected AudioClip myClip;
        private bool isInited;

        /// <summary>
        /// Not use this id for Save to Local Storage
        /// </summary>
        [HideInInspector] public int id { get; private set; }
        protected int comparePriority;

        protected bool canClick;
        protected bool canDrag;
        protected bool isComparePos;
        protected bool canMoveToGround;
        protected bool isStandTable;
        protected bool isScaleDown;
        protected bool isJumpingTable;
        protected bool isMoveToTray;
        private bool isAssigned;
        protected bool isClick;

        public bool IsCarry { get; protected set; }
        public bool IsInBag { get; protected set; }
        public bool IsFood { get; protected set; }
        public bool IsBeverage { get; protected set; }
        public bool IsScratch { get; protected set; }
        public bool IsAssigned
        {
            get => isAssigned;
            protected set
            {
                isAssigned = value;
                if (isAssigned)
                {
                    if (smokeFallingFx != null) smokeFallingFx.Stop();
                    GameManager.instance.CanMoveFloor = true;
                }
            }
        }

        protected GameObject Content { get; set; }
        protected GameObject Ground { get; set; }
        public Transform StandZone { get => standZone; }

        private Transform startParent;
        protected Vector3 startScale;
        protected Vector3 startPos;
        protected Vector3 startLocalPos;
        protected Vector3 localRotation;
        private Vector3 offset;

        protected Tweener tweenScale;
        protected Tween tweenJump;
        private Tweener rotateTween_;
        protected Tween tweenMove;
        protected Tween tweenPunch;
        protected Tween delayTween;
        private bool firstTouch = true;
        private int rdTimeLoop;
        private float _distance;
        private Vector3 beginTouch;
        private int countSlideMove;
        private ParticleSystem smokeFallingFx;
        private bool isRegisterEventLoadData;

        protected BackItemDataSO data;
        protected CharacterDataSO characterData;
        private Tween _tweenDragDelay;
        private Tween _tweenMoveToGroundDelay;
        private Tween tweenSound;
        private GameObject maskRotate;
        private bool isTouched;

        /// <summary>
        /// Default = 0.3f
        /// </summary>
        protected float scaleIndex { private get; set; } = 0.3f;

        protected virtual void InitItem() { }

        private void Awake()
        {
            startParent = transform.parent;
            startScale = transform.localScale;
            startPos = transform.position;
            startLocalPos = transform.localPosition;
            startLocalPos.z = 0;
            startPos.z = 0;

            image = image == null ? GetComponent<Image>() : image;
            standZone = standZone == null ? transform : standZone;
        }

        protected virtual void Start()
        {
            if(GameManager.instance.IsLoadCompleted) { InitData(); }
            else
            {
                isRegisterEventLoadData = true;
                EventDispatcher.Instance.RegisterListener<EventKey.OnLoadDataCompleted>(InitData);
            }
        }

        protected virtual void InitData()
        {
            if (isInited) return;
            isInited = true;

            id = GameManager.instance.countId++;
            if (isDance)
            {
                rdTimeLoop = Random.Range(2, 4);
                Dancing();
            }
            InitItem();

            if (GameManager.instance.curFloorScroll != null) Content = GameManager.instance.curFloorScroll.content.gameObject;
            if (GameManager.instance.curGround != null) Ground = GameManager.instance.curGround.gameObject;

            data = DataSceneManager.Instance.BackItemDataSO;
            characterData = DataSceneManager.Instance.MainCharacterData;

            if (canDrag)
            {
                if (data != null)
                    smokeFallingFx = Instantiate(data.smokeFallingFxPb, transform);
                if(smokeFallingFx == null) Instantiate(characterData.smokeFxPb, transform);
                if (smokeFallingFx != null)
                    smokeFallingFx.transform.position = standZone.position;

            }
        }
        protected virtual void OnDestroy()
        {
            if (tweenMove != null)
                tweenMove?.Kill();
            if (tweenPunch != null)
                tweenPunch?.Kill();
            if (tweenJump != null)
                tweenJump?.Kill();
            if (delayTween != null)
                delayTween?.Kill();
            if (_tweenDragDelay != null)
                _tweenDragDelay?.Kill();
            if (tweenScale != null)
                tweenScale?.Kill();
            if (_tweenMoveToGroundDelay != null)
                _tweenMoveToGroundDelay?.Kill();

            if (isRegisterEventLoadData) EventDispatcher.Instance.RemoveListener<EventKey.OnLoadDataCompleted>(InitData);
        }

        protected virtual void OnEnable()
        {
            EventDispatcher.Instance.RegisterListener<EventKey.OnInitItem>(GetInitItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnBeginDragBackItem>(GetBeginDragItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnDragBackItem>(GetDragItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnEndDragBackItem>(GetEndDragItem);
            EventDispatcher.Instance.RegisterListener<EventKey.OnClickBackItem>(GetClickBackItem);
        }
        protected virtual void OnDisable()
        {
            EventDispatcher.Instance.RemoveListener<EventKey.OnInitItem>(GetInitItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnBeginDragBackItem>(GetBeginDragItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnDragBackItem>(GetDragItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnEndDragBackItem>(GetEndDragItem);
            EventDispatcher.Instance.RemoveListener<EventKey.OnClickBackItem>(GetClickBackItem);
        }
        void CreateMaskRotate()
        {
            if(maskRotate == null)
            {
                // Create Mask
                maskRotate = Instantiate(GameManager.instance.RotateMaskItempb, transform);
                maskRotate.transform.SetAsFirstSibling();
            }
        }

        public void InitOrder(int order)
        {
            comparePriority = order;
        }
        public virtual void AssignDrag()
        {
            canDrag = true;
            isComparePos = true;
            Ground = GameManager.instance.curGround.gameObject;
            Content = GameManager.instance.curFloorScroll.content.gameObject;

        }

        public void PlaySound()
        {
            if (myClip != null)
            {
                if (SoundManager.instance != null)
                {
                    SoundManager.instance.PlayOtherSfx(myClip);
                }
                else if(SoundBaseRoomManager.Instance != null)
                {
                    SoundBaseRoomManager.Instance.Play(myClip);
                }
            }
        }
        public void AssignClick()
        {
            canClick = true;
        }
        /// <summary>
        /// Tìm ra b? m?t ???c ?u tiên ?? item bay ??n
        /// </summary>
        /// <param name="OnSuccess"></param>
        /// <param name="OnFail"></param>
        public void CheckPriority(System.Action OnSuccess = null, System.Action OnFail = null)
        {
            if (comparePriority > GameManager.instance.CurPriority)
            {
                GameManager.instance.CurPriority = comparePriority;
                OnSuccess?.Invoke();
            }
            else
            {
                OnFail?.Invoke();
            }
        }

        private void TouchedThisObject()
        {
            if (!isTouched)
            {
                isTouched = true;
                EventDispatcher.Instance.Dispatch(new EventKey.OnTouchBackItem { backItem = this });
            }
        }
        private void GetInitItem(EventKey.OnInitItem obj)
        {
            if (obj.scrollRect != null)
            {
                Content = obj.scrollRect.content.gameObject;
            }
            if (obj.ground != null)
            {
                Ground = obj.ground.gameObject;
            }
        }
        void Dancing()
        {
            tweenPunch = transform.DOPunchScale(new Vector3(0.05f, -0.05f, 0), 1.5f, 2).OnComplete(() =>
            {
                delayTween = DOVirtual.DelayedCall(rdTimeLoop, () =>
                {
                    Dancing();
                });
            });
        }
        public void DisableDance()
        {
            if (delayTween != null)
            {
                delayTween?.Kill();
            }
            if (tweenPunch != null)
            {
                tweenPunch?.Kill();
            }
            transform.localScale = startScale;
        }
        public Image GetImage()
        {
            return image;
        }
        public void OnPacking(Transform _endParent)
        {
            isAssigned = true;
            KillDragging();
            transform.SetParent(_endParent);
            transform.localPosition = Vector3.zero;
            //transform.localScale = Vector3.zero;
            image.enabled = false;
        }

        protected virtual void GetClickBackItem(EventKey.OnClickBackItem item)
        {
        }

        protected virtual void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
        }

        protected virtual void GetDragItem(EventKey.OnDragBackItem item)
        {
        }

        protected virtual void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            // Track Sibling Inddex
            if (!gameObject.activeSelf) return;
            if (Vector2.Distance(item.backitem.transform.position, transform.position) > 2) return;
            if (item.backitem == null) return;
            if (item.backitem == this) return;
            if (IsAssigned || item.backitem.IsAssigned) return;
            if (!item.backitem.isComparePos) return;
            if (!isComparePos) return;
            if (!item.backitem.canMoveToGround || !canMoveToGround) return;

            GetSiblingIndex(item.backitem);
        }

        public virtual void GetSiblingIndex(BackItem backItem_)
        {
            _distance = Vector2.Distance(backItem_.transform.position, transform.position);
            if (_distance > 2) return;
            if (backItem_.transform.position.y > transform.position.y)
            {
                if (backItem_.canMoveToGround)
                {
                  //  backItem_.KillDragging();
                    backItem_.transform.SetParent(transform.parent);
                    backItem_.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);

                    backItem_.MoveToGround();
                }
            }
        }
        public void AssignToCharacter(Transform _endParent, bool isWolfoo = true)
        {
            if (IsAssigned) return;
            IsAssigned = true;

            KillDragging();
            transform.SetParent(_endParent);
            transform.localPosition = Vector3.zero;
            KillScalling();
            if (!isWolfoo) transform.localScale *= 3;
        }

        public void OnFeed(Transform _endParent)
        {
            IsAssigned = true;

            KillDragging();
            KillScalling();
            transform.SetParent(_endParent);

            tweenMove = transform.DOLocalMove(Vector3.zero, 0.25f).SetEase(Ease.Linear);
            tweenScale = transform.DOScale(transform.localScale / 3, 0.35f).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    Destroy(gameObject);
                });
        }

        public void MoveToGround(bool isForce = false)
        {
            if ((!canMoveToGround || IsAssigned) && !isForce) return;

            transform.SetParent(Content.transform);

            if(SoundManager.instance == null)
            {
                tweenSound?.Kill();
                tweenSound = DOVirtual.DelayedCall(0.25f, () =>
                {
                    SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.FallingDown);
                });
            }
            else
            {
                if (SoundManager.instance.Sfx != null && !SoundManager.instance.Sfx.isPlaying)
                    SoundManager.instance.PlayOtherSfx(SfxOtherType.FallingDown, 0.2f);
            }

            if(isForce)
            {
                KillScalling();
                tweenScale = transform.DOScale(startScale, 0.5f)
                    .SetEase(Ease.Flash);
            }

            if (smokeFallingFx != null)
                smokeFallingFx.Play();
            if (transform.position.y > Ground.transform.position.y)
            {
             //   transform.localRotation = Quaternion.Euler(localRotation);
                if (tweenMove != null && tweenMove.IsActive()) return;
                tweenMove = transform.DOLocalMoveY(Ground.transform.localPosition.y, 1)
                .SetEase(ease, 0.25f, 0.25f).OnComplete(() =>
                {
                });
            }
            else
            {
             //   transform.localRotation = Quaternion.Euler(localRotation);
                if (tweenJump != null && tweenJump.IsActive()) return;
                tweenJump = transform.DOLocalJump(transform.localPosition, 10, 1, 0.5f).SetEase(Ease.OutBounce).OnComplete(() =>
                {
                    //if (smokeFallingFx != null)
                    //    smokeFallingFx.Play();
                });
            }
        }
        public void MoveToStartPos()
        {
            canMoveToGround = false;
            KillDragging();
            KillScalling();
            transform.SetParent(startParent);
            transform.localScale = startScale;
            tweenMove = transform.DOMove(startPos, 0.5f).OnComplete(() =>
            {
                tweenMove = transform.DOPunchPosition(Vector3.up * 3, 0.5f, 1).OnComplete(() =>
                {
                });
            });
        }


        void CheckPriority(int priority, System.Action OnVerified)
        {
            if (priority < GameManager.instance.CurPriority) return;
            if (IsAssigned)
            {
                if (smokeFallingFx != null) smokeFallingFx.Stop();
                return;
            }

            GameManager.instance.CurPriority = priority;
            OnVerified?.Invoke();
        }

        public void JumpToEndPos(Vector3 _endPos, Transform _endParent, System.Action OnComplete = null, int assignPriority = 1)
        {
            CheckPriority(assignPriority, () =>
            {
                canMoveToGround = false;

                if(_tweenDragDelay != null) _tweenDragDelay?.Kill();
                _tweenDragDelay = DOVirtual.DelayedCall(0.1f, () =>
                {
                    CheckPriority(assignPriority, () =>
                    {
                        GameManager.instance.CanMoveFloor = false;
                        if (smokeFallingFx != null) smokeFallingFx.Play();

                        KillDragging();
                        if (_endParent != null) transform.SetParent(_endParent);
                        tweenJump = transform.DOJump(_endPos, 0.25f, 1, 0.5f)
                        .SetEase(Ease.OutBounce)
                        .OnComplete(() =>
                        {
                            GameManager.instance.CanMoveFloor = true;
                            OnComplete?.Invoke();
                        //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
                        });
                    });
                });
            });
        }
        public void JumpToCart(
            Transform _endParent,
            bool isSmoking = false)
        {
            canMoveToGround = false;
            isAssigned = true;

            if (isSmoking)
            {
                if (smokeFallingFx != null)
                    smokeFallingFx.Play();
            }

            KillDragging();
            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(transform.localPosition, 150f, 1, 0.5f)
            .SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
            //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
            });
        }
        public void JumpToEndLocalPos(
            Vector3 _endPos,
            System.Action OnComplete = null,
            Ease jumpEase = Ease.OutBounce,
            float power = 50f,
            bool isSmoking = false,
            int assignPriority = 1)
        {
            if (assignPriority < GameManager.instance.CurPriority) return;
            GameManager.instance.CurPriority = assignPriority;

            canMoveToGround = false;
            if (IsAssigned) return;

                if(_tweenDragDelay != null) _tweenDragDelay?.Kill();
            _tweenDragDelay = DOVirtual.DelayedCall(0.1f, () =>
            {
                if (IsAssigned) return;
                if (assignPriority < GameManager.instance.CurPriority) return;
                if (isSmoking)
                {
                    if (smokeFallingFx != null)
                        smokeFallingFx.Play();
                }

                KillDragging();

             //   if (_endParent != null) transform.SetParent(_endParent);
                tweenJump = transform.DOLocalJump(_endPos, power, 1, 0.5f)
                .SetEase(jumpEase)
                .OnComplete(() =>
                {
                    GameManager.instance.CanMoveFloor = true;
                    OnComplete?.Invoke();
                //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
                });
            });
        }
        public virtual void MoveToEndLocalPos(
            Vector3 _endPos,
            System.Action OnComplete = null,
            Ease jumpEase = Ease.Flash,
            bool isOffset = false,
            float time = 0.5f)
        {
            canMoveToGround = false;
            if (IsAssigned) return;

            KillDragging();
            offset = isOffset ? standZone.localPosition : new Vector3(0, 0, 0);

            if (IsAssigned) return;
            tweenJump = transform.DOLocalMove(_endPos + offset, time)
            .SetEase(jumpEase)
            .OnComplete(() =>
            {
                OnComplete?.Invoke();
                //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
            });
        }
        public virtual void JumpIntoBasket(
            Transform _endParent,
            System.Action OnComplete = null)
        {
            isComparePos = false;
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(_endParent);
            tweenJump = transform.DOLocalJump(Vector3.zero, 200f, 1, 0.5f)
            .OnComplete(() =>
            {
                OnComplete?.Invoke();
                //     SoundManager.instance.PlayOtherSfx(SfxOtherType.DownToGround);
            });
        }
        public virtual void OnSlide(Transform slideZone, Transform _endParent)
        {
            canMoveToGround = false;
            canDrag = false;
            IsAssigned = true;
            KillDragging();

            SoundManager.instance.PlayOtherSfx(SfxOtherType.SlideWhistle);
            if (countSlideMove == 0)
            {
                SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Hooray);
                transform.SetParent(_endParent);
                transform.localPosition = slideZone.GetChild(0).localPosition;
                transform.rotation = slideZone.GetChild(0).rotation;
            }

            rotateTween_ = transform.DORotate(slideZone.GetChild(countSlideMove).rotation.eulerAngles, 0.5f).SetEase(Ease.Linear);
            tweenMove = transform.DOLocalMove(slideZone.GetChild(countSlideMove).localPosition, 800)
                .SetEase(Ease.Linear)
                .SetSpeedBased(true)
                .OnComplete(() =>
                {
                    countSlideMove++;
                    if (countSlideMove == slideZone.childCount - 1)
                    {
                        rotateTween_ = transform.DORotate(slideZone.GetChild(countSlideMove).rotation.eulerAngles, 0.5f).SetEase(Ease.Linear);
                        tweenMove = transform.DOLocalMove(slideZone.GetChild(countSlideMove).localPosition, 1000)
                           .SetEase(Ease.Linear)
                           .SetSpeedBased(true).OnComplete(() =>
                           {
                               countSlideMove = 0;
                               canMoveToGround = true;
                               canDrag = true;
                           });
                    }
                    else
                    {
                        OnSlide(slideZone, _endParent);
                    }
                });
        }

        public void KillDragging()
        {
            if (tweenJump != null)
                tweenJump?.Kill();
            if (tweenMove != null)
                tweenMove?.Kill();
            if (_tweenDragDelay != null) _tweenDragDelay?.Kill();
        }
        public void KillScalling()
        {
            if (tweenScale != null)
                tweenScale?.Kill();
            if (tweenPunch != null)
                tweenPunch?.Kill();
            transform.localScale = startScale;

        }
        public void OnPunchScale()
        {
            KillScalling();
            tweenPunch = transform.DOPunchScale(new Vector3(-0.1f, 0.1f, 0), 0.5f, 5).OnComplete(() =>
            {
            });
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (!canClick) return;
            TouchedThisObject();
            if (isDance)
            {
                if (!firstTouch) return;
                KillScalling();
                firstTouch = false;
                rdTimeLoop *= 2;
            }
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!canDrag) return;
            TouchedThisObject();

            GameManager.instance.CurPriority = 0;
            canMoveToGround = true;
            isJumpingTable = false;
            IsAssigned = false;
            KillDragging();

            CreateMaskRotate();
            if (canDrag) isComparePos = true;

            if (smokeFallingFx != null) smokeFallingFx.Stop();

            transform.SetParent(Content.transform);
            transform.SetAsLastSibling();
            transform.localRotation = Quaternion.Euler(Vector3.zero);
            GameManager.instance.GetCurrentPosition(transform);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!canDrag) return;

            GameManager.instance.GetCurrentPosition(transform);
            GameManager.instance.CheckPos(transform);
            EventDispatcher.Instance.Dispatch(new EventKey.OnDragBackItem { backItem = this });
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            if (!canDrag) return;

            if (_tweenMoveToGroundDelay != null)
                _tweenMoveToGroundDelay?.Kill();
            _tweenMoveToGroundDelay = DOVirtual.DelayedCall(0.1f, () =>
            {
                MoveToGround();
            });
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            isClick = true;
            beginTouch = transform.position;

            if (!isScaleDown) return;

            KillScalling();
            tweenScale = transform.DOScale(startScale + Vector3.one * scaleIndex, 0.5f)
                .SetEase(Ease.Flash);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            if (beginTouch != transform.position) isClick = false;
            if (!isScaleDown) return;

            KillScalling();
            tweenScale = transform.DOScale(startScale, 0.5f)
                .SetEase(Ease.Flash);
        }
    }
}