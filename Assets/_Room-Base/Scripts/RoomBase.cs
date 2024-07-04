using _Base;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class RoomBase : MonoBehaviour
    {
        [SerializeField] private PanelType myPanel;
        [SerializeField] private PanelType parentPanel = PanelType.Intro;
        [SerializeField] private FloorWorld floor;
        [SerializeField] BoxCollider2D blockMask;

        public Transform Content;
        public PanelType Panel { get => myPanel; }
        public BoxCollider2D BlockMask { get => blockMask; }

        private void Start()
        {
            floor.Assign(true);
            floor.Assign();
            floor.transform.localPosition = Vector3.zero;

            if (!BaseDataManager.Instance.playerMe.IsCityShowed(GameManager.instance.City))
            {
                floor.PreviewMap(() =>
                {
                    blockMask.gameObject.SetActive(false);
                });
                blockMask.gameObject.SetActive(true);
            }
            GameManager.instance.AssignBackFloor(parentPanel, myPanel);

            EventRoomBase.OnCreatedMinigame += OnCreateMinigame;
            EventRoomBase.OnCompletedMinigame += OnCompleteMinigame;
            EventRoomBase.OnCloseCharacterPanel += OnCompleteMinigame;
         //   ResizeBG();
        }
        private void OnDestroy()
        {
            EventRoomBase.OnCreatedMinigame -= OnCreateMinigame;
            EventRoomBase.OnCompletedMinigame -= OnCompleteMinigame;
            EventRoomBase.OnCloseCharacterPanel -= OnCompleteMinigame;
        }

        private void OnCreateMinigame()
        {
            blockMask.gameObject.SetActive(true);
            floor.Assign(false);
        }

        private void OnCompleteMinigame()
        {
            blockMask.gameObject.SetActive(false);
            floor.Assign(true);
        }

        //public void OnSwipe()
        //{
        //    xMoved = initTouch.x - touchTrans.position.x;
        //    yMoved = initTouch.y - touchTrans.position.y;
        //    distance = Vector2.Distance(initTouch, touchTrans.position);
        //    swipedLeft = Mathf.Abs(xMoved) > Mathf.Abs(yMoved);

        //    // Swipe Left
        //    if (swipedLeft && xMoved > 0)
        //    {
        //    }
        //    // Swipe Right
        //    else if (swipedLeft && xMoved < 0)
        //    {
        //    }
        //    // Swipe Up
        //    else if (!swipedLeft && yMoved < 0)
        //    {
        //    }
        //    // Swipe Down
        //    else if (!swipedLeft && yMoved > 0)
        //    {
        //    }
        //}

        private void ResizeBG()
        {
            var normalSize = 1f;
            var wideSize = 1f;
            var longSize = 1f;
            switch (myPanel)
            {
                case PanelType.BeachVillaRoom1:
                    break;
                case PanelType.CampingParkRoom1:
                    break;
                default:
                    break;
            }

            if (Camera.main.aspect >= 1.7)
            {
                //   Debug.Log("16:9");
                transform.localScale = Vector3.one * longSize;
            }
            else if (Camera.main.aspect >= 1.5)
            {
                //     Debug.Log("3:2");
                transform.localScale = Vector3.one * normalSize;
            }
            else
            {
                //    Debug.Log("4:3");
                transform.localScale = Vector3.one * wideSize;
            }
        }
    }
}
