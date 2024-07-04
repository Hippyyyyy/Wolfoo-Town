using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static _WolfooShoppingMall.UIManager;

namespace _WolfooShoppingMall
{
    public abstract class PlayerScrollView : MonoBehaviour
    {
        [SerializeField] public ScrollRect scrollview;
        public float moveVelocity = 1;
        public bool IsOpenPanel { get; private set; }

        public abstract void Setup(LimitArea limitArea);

        protected Transform[] myLimit;
        private bool isScroll;
        private bool isScrollToRight;
        private Vector2 endPos;

        public void Hide()
        {
            IsOpenPanel = false;
            gameObject.SetActive(false);
            transform.SetAsLastSibling();
        }
        public void Show()
        {
            IsOpenPanel = true;
            gameObject.SetActive(true);
            transform.SetAsLastSibling();
        }

        public bool CheckInSideScrollView(NewCharacterScrollItem scrollItem)
        {
            var isInsideScrollView = GameManager.instance.Is_inside(scrollItem.transform.position, myLimit);
            return isInsideScrollView;
        }
        public bool CheckInSideScrollView(NewCharacterWolfooScrollItem scrollItem)
        {
            var isInsideScrollView = GameManager.instance.Is_inside(scrollItem.transform.position, myLimit);
            return isInsideScrollView;
        }
        public bool CheckInSideScrollView(CharacterWorld scrollItem)
        {
            var isInsideScrollView = GameManager.instance.Is_inside(scrollItem.ComparedPosition, myLimit);
            return isInsideScrollView;
        }
        public bool CheckInSideScrollView(CharacterWolfooWorld wolfoo)
        {
            var isInsideScrollView = GameManager.instance.Is_inside(wolfoo.transform.position, myLimit);
            return isInsideScrollView;
        }
        private void Update()
        {
            if(isScroll)
            {
                if (scrollview.content.localPosition.x < -scrollview.content.sizeDelta.x || scrollview.content.localPosition.x > 0)
                {
                    isScroll = false;
                }

                if (scrollview.content.anchoredPosition.x > endPos.x && isScrollToRight)
                {
                    scrollview.content.anchoredPosition += Vector2.left * moveVelocity;
                    return;
                }

                if(scrollview.content.anchoredPosition.x < endPos.x && !isScrollToRight)
                {
                    scrollview.content.anchoredPosition += Vector2.right * moveVelocity;
                    return;
                }

                isScroll = false;
            }
        }
        public void ScrollTo(Transform endTarget)
        {
            Canvas.ForceUpdateCanvases();

            endPos = (Vector2)scrollview.transform.InverseTransformPoint(scrollview.content.position)
                    - (Vector2)scrollview.transform.InverseTransformPoint(endTarget.position)
                    + Vector2.right * 1000;
            endPos.y = 0;
         //   scrollview.content.anchoredPosition = endPos;
            isScroll = true;
            isScrollToRight = scrollview.content.anchoredPosition.x > endPos.x;
        }
    }
}
