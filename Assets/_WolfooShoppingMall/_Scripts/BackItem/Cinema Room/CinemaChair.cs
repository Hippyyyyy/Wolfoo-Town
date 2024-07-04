using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class CinemaChair : BackItem
    {
        [SerializeField] bool isClose;
        [SerializeField] Sprite[] statusSprites;
        [SerializeField] Transform sitZone;
        [SerializeField] Transform[] cupZones;
        private float distance;
        private bool isEmpty = true;
        private BackItem curCharacter;
        private int idxVerified;
        private float curVerifyDistance;

        private List<bool> listIdxCupEmpty = new List<bool>();
        private List<int> listIdBottle = new List<int>();
        private bool IsBeverageVerified;

        protected override void InitItem()
        {
            canClick = true;
        }
        protected override void Start()
        {
            base.Start();

            image.sprite = statusSprites[isClose ? 0 : 1];
            image.SetNativeSize();

            for (int i = 0; i < cupZones.Length; i++)
            {
                listIdxCupEmpty.Add(true);
                listIdBottle.Add(-1);
            }
        }
        protected override void GetBeginDragItem(EventKey.OnBeginDragBackItem item)
        {
            base.GetBeginDragItem(item);

            if (item.backitem != null)
            {
                if (item.backitem.IsBeverage)
                {
                    IsBeverageVerified = false;
                    if (!listIdBottle.Contains(item.backitem.GetInstanceID())) return;

                    int idx = listIdBottle.IndexOf(item.backitem.GetInstanceID());
                    listIdBottle[idx] = -1;
                    listIdxCupEmpty[idx] = true;
                }

                if (item.character != null)
                {
                    if (item.character != curCharacter) return;
                    isEmpty = true;
                }
                if (item.newCharacter != null)
                {
                    if (item.newCharacter != curCharacter) return;
                    isEmpty = true;
                }
            }
        }


        protected override void GetEndDragItem(EventKey.OnEndDragBackItem item)
        {
            base.GetEndDragItem(item);
            if (item.backitem == null) return;

            if (item.character != null)
            {
                if (isClose || !isEmpty) return;

                distance = Vector2.Distance(item.backitem.transform.position, sitZone.position);
                if (distance < 2)
                {
                    isEmpty = false;
                    curCharacter = item.character;

                    // character use method to sit here
                    item.character.OnSitToChair(sitZone.position, sitZone, true);
                }
            }
            if (item.newCharacter != null)
            {
                if (isClose || !isEmpty) return;

                distance = Vector2.Distance(item.backitem.transform.position, sitZone.position);
                if (distance < 2)
                {
                    isEmpty = false;
                    curCharacter = item.newCharacter;

                    // character use method to sit here
                    item.newCharacter.OnSitToChair(sitZone.position, sitZone, true);
                }
            }

            if (item.backitem.IsBeverage)
            {
                if (IsBeverageVerified) return;

                idxVerified = -1;
                curVerifyDistance = 1000f;

                for (int i = 0; i < cupZones.Length; i++)
                {
                    if (!listIdxCupEmpty[i]) continue;

                    distance = Vector2.Distance(item.backitem.transform.position, cupZones[i].position);
                    if (distance < 1)
                    {
                        IsBeverageVerified = true;
                        if (curVerifyDistance > distance)
                        {
                            idxVerified = i;
                            curVerifyDistance = distance;
                        }
                    }
                }

                if (idxVerified != -1) // OnComplete
                {
                    listIdxCupEmpty[idxVerified] = false;
                    listIdBottle[idxVerified] = item.backitem.GetInstanceID();

                    // Cup use method to sit here
                    item.backitem.transform.SetParent(cupZones[idxVerified]);
                    item.backitem.JumpToEndLocalPos(Vector3.up * item.backitem.transform.position.y, null, Ease.OutBounce);
                }
                else // OnFail
                {
                }

            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick || !isEmpty) return;
            isClose = !isClose;

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Click);

            image.sprite = statusSprites[isClose ? 0 : 1];
            image.SetNativeSize();

        }
    }
}