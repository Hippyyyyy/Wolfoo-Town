using _Base;
using _WolfooShoppingMall;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class TicketPrinter : BackItem
    {
        [SerializeField] FilmTicket ticketPb;
        [SerializeField] Transform ticketZone;
        [SerializeField] Animator animator;
        [SerializeField] FilmTicket[] multiTicketPbs;
        [SerializeField] string[] triggerNames;
        [SerializeField] Transform groundArea;

        private FilmTicket ticket;
        private int countShake;
        private int curTicketIdx;
        private bool isPrinting;
        private FilmTicket ticketClicked;

        protected override void InitItem()
        {
            canClick = true;
            isDance = true;
        }

        public void OnPrinted()
        {
            ticket.OnPrinted();
            canClick = true;
            isPrinting = false;
            
            if (GUIManager.instance.CurrentMapController == CityType.Playground)
            {
                ticket.transform.SetParent(groundArea);
                ticket.transform.localPosition = Vector3.zero;
                ticket.transform.localRotation = Quaternion.Euler(Vector3.forward * 90);
            }
        }
        protected override void GetClickBackItem(EventKey.OnClickBackItem item)
        {
            base.GetClickBackItem(item);
            if(item.ticket != null)
            {
                if (GUIManager.instance.CurrentMapController == CityType.Playground)
                {
                    ticketClicked = item.ticket;
                }
            }
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            if (SoundManager.instance != null) SoundManager.instance.PlayOtherSfx(SfxOtherType.Printing, 0, 2);
            else SoundBaseRoomManager.Instance.Play(SoundBaseRoomManager.SfxType.Printing);

            countShake = 0;

            if(animator != null)
            {
                if(GUIManager.instance.CurrentMapController == CityType.Playground)
                {
                    OnPlaygroundTicketMachinePrint();
                }
                else
                {
                    isPrinting = true;
                    animator.SetTrigger("Run");
                    ticket = Instantiate(ticketPb, ticketZone);
                    ticket.transform.localPosition = Vector3.zero;
                }
            }
            else
            {
                isPrinting = true;
                OnPrinting(() =>
                {
                    ticket = Instantiate(ticketPb, ticketZone);
                    ticket.transform.localPosition = ticketZone.GetChild(0).localPosition;
                    ticket.OnPrinted(data.filmTicketData.ticketColorSprites[curTicketIdx],
                        ticketZone.GetChild(1).localPosition,
                        ticketZone.GetChild(2).localPosition);

                    curTicketIdx++;
                    if (curTicketIdx >= data.filmTicketData.ticketColorSprites.Length) curTicketIdx = 0;

                    canClick = true;
                    isPrinting = false;
                });
            }
        }

        private void OnPlaygroundTicketMachinePrint()
        {
            if (isPrinting) return;
            isPrinting = true;
            var triggerIdx = -1;
            if (ticketClicked == null) triggerIdx = UnityEngine.Random.Range(0, triggerNames.Length);
            else triggerIdx = ticketClicked.AssignIdx;

            animator.SetTrigger(triggerNames[triggerIdx]);
            ticket = Instantiate(multiTicketPbs[triggerIdx], ticketZone);
            ticket.Init();
            ticket.gameObject.SetActive(true);
        }

        void OnPrinting(System.Action OnComplete)
        {
            countShake++;
            if (countShake >= 3)
            {
                OnComplete?.Invoke();
                return;
            }

            tweenMove = transform.DOLocalMoveX(startLocalPos.x - 10, 0.15f)
            .OnComplete(() =>
            {
                tweenMove = transform.DOLocalMoveX(startLocalPos.x + 10, 0.15f)
                .OnComplete(() =>
                {
                    tweenMove = transform.DOLocalMoveX(startLocalPos.x, 0.15f)
                    .OnComplete(() =>
                    {
                        OnPrinting(OnComplete);
                    });
                });
            });
        }
    }
}