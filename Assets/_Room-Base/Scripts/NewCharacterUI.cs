using DG.Tweening;
using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class NewCharacterUI : BackItem
    {
        [SerializeField] Image handRightItemImg;
        [SerializeField] Image handLeftItemImg;
        [SerializeField] ParticleSystem starFx;
        [SerializeField] Transform mouthZone;
        [SerializeField] Transform glassZone;
        [SerializeField] Transform faceMaskZone;
        [SerializeField] Transform operaMaskArea;
        [SerializeField] Transform hatArea;
        [SerializeField] Transform buttArea;
        [SerializeField] CharacterUIAnimationManager animator;

        private bool isSitToChair;
        private float distance;
        private float distanceButt;
        private ParticleSystem myLightingFx;
        private FaceMask myFaceMask;
        private OperaHat myOperaMask;
        private Hat myHat;
        private Glasses curGlasses;
        private BackItem[] handItems = new BackItem[2];
        private Transform scrollItemParent;
        private int scrollSiblingIndex;
        private AgeSetting myAge;

        protected override void InitItem()
        {
            canClick = true;
            canDrag = true;
            isComparePos = true;
        }

        private void Awake()
        {
            handRightItemImg.enabled = false;
            handLeftItemImg.enabled = false;
        }

        protected override void Start()
        {
            base.Start();

        //    wolfooAnim.PlayIdle();

            if (Content == null)
                Content = GameManager.instance.curFloorScroll.content.gameObject;
            if (Ground == null)
                Ground = GameManager.instance.curGround.gameObject;

            ParticleSystem starFxPb;
            if (DataSceneManager.Instance.ItemDataSO != null) starFxPb = DataSceneManager.Instance.ItemDataSO.CharacterData.starFxPb;
            else starFxPb = DataSceneManager.Instance.MainCharacterData.CharacterData.starFxPb;

            if (starFxPb != null)
                starFx = Instantiate(starFxPb, transform);

            distanceButt = Vector2.Distance(transform.position, buttArea.position);
        }
        protected override void OnEnable()
        {
         //   wolfooAnim.gameObject.SetActive(true);
            SoundCharacterManager.Instance.Play(SoundCharacterManager.SfxWolfooType.Hello);

            base.OnEnable();
        }
        protected override void OnDisable()
        {
        //    wolfooAnim.gameObject.SetActive(false);
            base.OnDisable();
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            if (!canDrag) return;
            isSitToChair = false;
            //    wolfooAnim.PlaySit();
            animator.InitDrag();
            EventDispatcher.Instance.Dispatch(new EventKey.OnBeginDragBackItem { newCharacter = this, backitem = this });
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            if (!canDrag) return;
            if (!isSitToChair)
        //        wolfooAnim.PlayIdle();
            EventDispatcher.Instance.Dispatch(new EventKey.OnEndDragBackItem { newCharacter = this, backitem = this });
            animator.StopDrag();
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);

            animator.PlayDrag();
        }

        protected override void GetDragItem(EventKey.OnDragBackItem item)
        {
            base.GetDragItem(item);
            if (item.perfume != null)
            {
                if (myLightingFx != null && myLightingFx.isPlaying) return;
                distance = Vector2.Distance(item.perfume.SmokePerfumeArea.position, faceMaskZone.position);
                if (distance < 1)
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
                //        wolfooAnim.PlayEat();
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
                    item.faceMask.Swear(faceMaskZone, false);
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
                    item.operaHat.Swear2(operaMaskArea, transform.localRotation.y == 180, false);
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
                    item.hat.Swear(hatArea, transform.localRotation.y == 180, false);
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
              //      wolfooAnim.ChangeSkin(item.clothing.IdAssign);
                //    wolfooAnim.PLayTakeAPhoto();

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

                    item.glasses.OnHang(glassZone, false);
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
                        animator.PlayEating();
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
                //
                _curItem.AssignToCharacter(handLeftItemImg.transform, false);
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

                _curItem.AssignToCharacter(handRightItemImg.transform, false);
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
            //
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
          //  wolfooAnim.PlaySit();
            base.OnSlide(slideZone, _endParent);
        }

        public void Jumping(Vector3 _endPos, Transform _endParent)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();

            transform.position = _endPos;
            transform.SetParent(_endParent);
        //    wolfooAnim.PlayJump(true);
        }

        public void OnGoToBed(Vector3 _endPos, Transform _endParent)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();

            transform.position = _endPos;
            transform.SetParent(_endParent);
        //    wolfooAnim.PlaySleep();
        }
        public void Assign(AgeSetting age)
        {
            myAge = age;
        }

        public void AssginToFloor()
        {
            animator.Enable = true;
            faceMaskZone.gameObject.SetActive(true);
            handRightItemImg.gameObject.SetActive(true);
            handLeftItemImg.gameObject.SetActive(true);
            glassZone.gameObject.SetActive(true);
            MoveToGround();
            scrollItemParent.gameObject.SetActive(false);
        }
        public void AssignToScrollView()
        {
            animator.Enable = false;
            scrollItemParent = transform.parent;
            scrollSiblingIndex = transform.GetSiblingIndex();
        }

        public void AssignToScrollItem()
        {
            animator.Enable = false;
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();

            faceMaskZone.gameObject.SetActive(false);
            handRightItemImg.gameObject.SetActive(false);
            handLeftItemImg.gameObject.SetActive(false);
            glassZone.gameObject.SetActive(false);

            scrollItemParent.gameObject.SetActive(true);
            transform.SetParent(scrollItemParent);
            transform.SetSiblingIndex(scrollSiblingIndex);
            transform.localPosition = Vector3.zero;
        }
        public void OnSitToSeesaw(Transform parent, Direction direction = Direction.Right)
        {
            canMoveToGround = false;
            KillDragging();

            transform.SetParent(parent);
            transform.localPosition = Vector2.zero;
            transform.rotation = Quaternion.Euler(direction == Direction.Right ? Vector3.up * 180 : Vector3.zero);
      //      wolfooAnim.PlaySit();
        }

        public void OnSitToChair(Vector3 endPos, Transform _endParent, bool isCinema = false)
        {
            IsAssigned = true;
            canMoveToGround = false;
            KillDragging();
     //       wolfooAnim.PlaySit();
            transform.SetParent(_endParent);
         //   transform.position = endPos + (myAge == AgeSetting.Baby ? Vector3.down * 0.5f : Vector3.zero);
            transform.position = endPos + Vector3.down * distanceButt;

            if (isCinema) transform.rotation = Quaternion.Euler(Vector3.up * 180);
        }
        public void OnSitDownHorse(Transform _endTrans)
        {
            canMoveToGround = false;
            IsAssigned = true;
            KillDragging();
            //DisableDrag():

     //       wolfooAnim.PlaySit();
            transform.SetParent(_endTrans);
            transform.rotation = Quaternion.Euler(Vector3.zero);
            transform.localPosition = new Vector3(-70f, -80f, 0);
        }

    }
}
