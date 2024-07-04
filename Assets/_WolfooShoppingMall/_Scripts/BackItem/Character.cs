using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using SCN;

namespace _WolfooShoppingMall
{
    public class Character : BackItem
    {
        [SerializeField] Image handRightItemImg;
        [SerializeField] Image handLeftItemImg;
        [SerializeField] ParticleSystem starFx;
        [SerializeField] CharacterAnimation wolfooAnim;
        [SerializeField] Transform mouthZone;
        [SerializeField] Transform glassZone;
        [SerializeField] Transform faceMaskZone;
        [SerializeField] Transform operaMaskArea;
        [SerializeField] Transform hatArea;

        bool isSitToChair;
        private CharacterScrollItem characterScrollView;
        private float distance;

        BackItem[] handItems = new BackItem[2] { null, null };
        private Glasses curGlasses;
        private FaceMask myFaceMask;
        private OperaHat myOperaMask;
        private ParticleSystem myLightingFx;
        private Hat myHat;
        private float lastPosX;

        public CharacterScrollItem CharacterScrollView { get => characterScrollView; }

        protected override void InitItem()
        {
            canClick = true;
            canDrag = true;
            isComparePos = true;
        }

        private void Awake()
        {
            characterScrollView = GetComponent<CharacterScrollItem>();
            handRightItemImg.enabled = false;
            handLeftItemImg.enabled = false;
        }

        protected override void Start()
        {
            base.Start();

            wolfooAnim.PlayIdle();

            if (Content == null)
                Content = GameManager.instance.curFloorScroll.content.gameObject;
            if (Ground == null)
                Ground = GameManager.instance.curGround.gameObject;

            ParticleSystem starFxPb;
            if (DataSceneManager.Instance.ItemDataSO != null) starFxPb = DataSceneManager.Instance.ItemDataSO.CharacterData.starFxPb;
            else starFxPb = DataSceneManager.Instance.MainCharacterData.CharacterData.starFxPb;

            if (starFxPb != null)
                starFx = Instantiate(starFxPb, transform);
        }
        protected override void OnEnable()
        {
            wolfooAnim.gameObject.SetActive(true);
            SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Hello);

            base.OnEnable();
        }
        protected override void OnDisable()
        {
            wolfooAnim.gameObject.SetActive(false);
            base.OnDisable();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;
            isSitToChair = false;
            wolfooAnim.PlaySit();

            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { character = this, backitem = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            if (!isSitToChair)
                wolfooAnim.PlayIdle();
        //    transform.rotation = Quaternion.Euler(Vector3.zero);
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { character = this, backitem = this });

            lastPosX = transform.position.x;
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            if (transform.position.x > lastPosX) transform.rotation = Quaternion.Euler(Vector3.up * 180);
            if (transform.position.x < lastPosX) transform.rotation = Quaternion.Euler(Vector3.zero);
            lastPosX = transform.position.x;
        }

        protected override void GetDragItem(EventKey.OnDragBackItem item)
        {
            base.GetDragItem(item);
            if(item.perfume != null)
            {
                if (myLightingFx != null && myLightingFx.isPlaying) return;
                distance = Vector2.Distance(item.perfume.SmokePerfumeArea.position, faceMaskZone.position);
                if(distance < 1)
                {
                    if (myLightingFx == null) myLightingFx = Instantiate(characterData.lightingFxPb, transform);

                    myLightingFx.startDelay = 2;
                    myLightingFx.Play();
                }
            }
        }

        public void SetToDefaultScale()
        {
            transform.SetParent(Content.transform);
            transform.localScale = startScale;
        }

        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == null) return;
            if (item.backitem == this) return;

            BackItem _curItem = null;
            if (item.toy != null) _curItem = item.toy;
            if (item.carToy != null) _curItem = item.carToy;
            if (item.giftDecal != null) _curItem = item.giftDecal;
            if (item.plasticup != null) _curItem = item.plasticup;
            if (item.popcorn != null) _curItem = item.popcorn;
            if (item.popcornBox != null) _curItem = item.popcornBox;
            if (item.glasses != null) _curItem = item.glasses;
            if (item.faceMask != null) _curItem = item.faceMask;
            if (item.backitem.IsCarry) _curItem = item.backitem;
            if (item.backitem.IsFood) _curItem = item.backitem;
            if (item.backitem.IsBeverage) _curItem = item.backitem;
            if (item.toy != null && !item.backitem.IsCarry) _curItem = null;

            if (item.plasticup != null)
            {
                if (item.plasticup.IsHasWater)
                {
                    distance = Vector2.Distance(item.plasticup.transform.position, mouthZone.position);
                    if (distance < 1)
                    {
                        wolfooAnim.PlayEat();
                        item.plasticup.OnPour(mouthZone);
                        return;
                    }
                }
            }

            if (item.dentisTool != null)
            {
                distance = Vector2.Distance(item.dentisTool.transform.position, mouthZone.position);
                if (distance < 1)
                {
                    item.dentisTool.Action(mouthZone.position);
                }
            }

            if (item.faceMask != null && myFaceMask == null)
            {
                distance = Vector2.Distance(item.faceMask.transform.position, faceMaskZone.position);
                if (distance < 1)
                {
                    myFaceMask = item.faceMask;
                    item.faceMask.Swear(faceMaskZone);
                    return;
                }
            }

            if (item.operaHat != null)
            {
                distance = Vector2.Distance(item.operaHat.transform.position, operaMaskArea.position);
                if (distance < 1)
                {
                    if (myOperaMask != null) myOperaMask.MoveToGround(true);

                    myOperaMask = item.operaHat;
                    item.operaHat.Swear2(operaMaskArea, transform.localRotation.y == 180);
                    return;
                }
                else
                {
                    if (myOperaMask != null) myOperaMask = null;
                }
            }
            if (item.hat != null && item.hat.IsNormalHat)
            {
                distance = Vector2.Distance(item.hat.transform.position, hatArea.position);
                if (distance < 1)
                {
                    if (myHat != null) myHat.MoveToGround(true);

                    myHat = item.hat;
                    item.hat.Swear(hatArea, transform.localRotation.y == 180);
                    return;
                }
                else
                {
                    if (myHat != null) myHat = null;
                }
            }

            if (item.clothing != null)
            {
                distance = Vector2.Distance(item.clothing.transform.position, transform.position);
                if (distance < 2)
                {
                    // Sound Character WOw Here
                    SoundCharacterManager.Instance.PlayWolfooInteresting();

                    item.clothing.OnCharacterSwear();
                    wolfooAnim.ChangeSkin(item.clothing.IdAssign);
                    wolfooAnim.PLayTakeAPhoto();

                    if (starFx != null)
                    {
                        starFx.transform.position = transform.position;
                        starFx.time = 0;
                        starFx.Play();
                    }
                    return;
                }
            }
            if (item.glasses != null)
            {
                if (Vector2.Distance(item.glasses.StandZone.position, glassZone.transform.position) < 1)
                {
                    if (curGlasses != null)
                        curGlasses.MoveToGround(true);

                    item.glasses.OnHang(glassZone);
                    curGlasses = item.glasses;
                    return;
                }
                else
                {
                    if (item.glasses == curGlasses)
                    {
                        curGlasses.MoveToGround(true);
                        curGlasses = null;
                    }
                }
            }

            if (_curItem == null) return;

            if (item.backitem.IsFood || item.backitem.IsBeverage)
            {
                distance = Vector2.Distance(_curItem.transform.position, mouthZone.position);
                if (distance < 1)
                {
                    _curItem.OnFeed(mouthZone);
                    if (item.backitem.IsScratch)
                    {
                        wolfooAnim.PlayEat(true);
                        return;
                    }
                    wolfooAnim.PlayEat(false);
                    return;
                }
            }

            distance = Vector2.Distance(_curItem.transform.position, handLeftItemImg.transform.position);
            if (distance < 1)
            {
                if (handItems[0] != null)
                {
                    handItems[0].transform.localRotation = Quaternion.Euler(Vector3.zero);
                    handItems[0].MoveToGround(true);
                }

                _curItem.AssignToCharacter(handLeftItemImg.transform);
                _curItem.PlaySound();
                handItems[0] = _curItem;
                return;
            }
            else
            {
                if (item.backitem == handItems[0])
                {
                    handItems[0] = null;
                }
            }

            distance = Vector2.Distance(_curItem.transform.position, handRightItemImg.transform.position);
            if (distance < 1)
            {
                if (handItems[1] != null)
                {
                    handItems[1].transform.localRotation = Quaternion.Euler(Vector3.zero);
                    handItems[1].MoveToGround(true);
                }

                _curItem.AssignToCharacter(handRightItemImg.transform);
                _curItem.PlaySound();
                handItems[1] = _curItem;
                return;
            }
            else
            {
                if (item.backitem == handItems[1])
                {
                    handItems[1] = null;
                }
            }

        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);
            if (item.faceMask != null)
            {
                if (item.faceMask == myFaceMask) myFaceMask = null;
            }
        }

        public override void OnSlide(Transform slideZone, Transform _endParent)
        {
            wolfooAnim.PlaySit();
            base.OnSlide(slideZone, _endParent);
        }

        public void Jumping(Vector3 _endPos, Transform _endParent)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();

            transform.position = _endPos;
            transform.SetParent(_endParent);
            wolfooAnim.PlayJump(true);
        }

        public void OnGoToBed(Vector3 _endPos, Transform _endParent)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();

            transform.position = _endPos;
            transform.SetParent(_endParent);
            wolfooAnim.PlaySleep();
        }

        public void AssginToFloor()
        {
            faceMaskZone.gameObject.SetActive(true);
            handRightItemImg.gameObject.SetActive(true);
            handLeftItemImg.gameObject.SetActive(true);
            glassZone.gameObject.SetActive(true);
            MoveToGround();
        }

        public void AssignToScrollItem()
        {
            canDrag = false;
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            faceMaskZone.gameObject.SetActive(false);
            handRightItemImg.gameObject.SetActive(false);
            handLeftItemImg.gameObject.SetActive(false);
            glassZone.gameObject.SetActive(false);

            CharacterScrollView.SetToScrollView();
            DOVirtual.DelayedCall(0.1f, () =>
            {
                enabled = false;
                canDrag = true;
            });

        }
        public void OnSitToSeesaw(Transform parent, Direction direction = Direction.Right)
        {
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(parent);
            transform.localPosition = Vector2.zero;
            transform.rotation = Quaternion.Euler(direction == Direction.Right ? Vector3.up * 180 : Vector3.zero);
            wolfooAnim.PlaySit();
        }

        public void OnSitToChair(Vector3 endPos, Transform _endParent, bool isCinema = false)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();
            wolfooAnim.PlaySit();
            transform.SetParent(_endParent);
            transform.localPosition = Vector3.zero;

            if (isCinema) transform.rotation = Quaternion.Euler(Vector3.up * 180);
        }
        public void OnSitDownHorse(Transform _endTrans)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();
            //DisableDrag():

            wolfooAnim.PlaySit();
            transform.SetParent(_endTrans);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localPosition = new Vector3(-70f, -80f, 0);
        }
    }
}