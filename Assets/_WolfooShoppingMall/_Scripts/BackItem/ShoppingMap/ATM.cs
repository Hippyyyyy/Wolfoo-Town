using _Base;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _WolfooShoppingMall
{
    public class ATM : BackItem
    {
        [SerializeField] MoneyPolime ticketPb;
        [SerializeField] Transform ticketZone;

        private MoneyPolime ticket;
        private int countShake;
        private int curTicketIdx;

        protected override void InitItem()
        {
            canClick = true;
            isDance = true;
        }
        protected override void Start()
        {
            base.Start();
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            if (!canClick) return;
            canClick = false;

            countShake = 0;
            OnPrinting(() =>
            {
                ticket = Instantiate(ticketPb, ticketZone);
                ticket.transform.localPosition = ticketZone.GetChild(0).localPosition;
                ticket.OnPrinted(data.filmTicketData.moneySprites[curTicketIdx],
                    ticketZone.GetChild(1).localPosition,
                    ticketZone.GetChild(2).localPosition);

                curTicketIdx++;
                if (curTicketIdx >= data.filmTicketData.moneySprites.Length) curTicketIdx = 0;

                canClick = true;
            });
        }

        void OnPrinting(System.Action OnComplete)
        {
            countShake++;
            if (countShake >= 3)
            {
                OnComplete?.Invoke();
                return;
            }

            SoundManager.instance.PlayOtherSfx(SfxOtherType.Printing);
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