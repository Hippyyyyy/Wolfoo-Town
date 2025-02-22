using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace _WolfooSchool
{
    public class EventManager : MonoBehaviour
    {
        public static Action OnReplay;
        public static Action<Vector3> OnUpdateStartPosAlpha;
        public static Action<Panel, PanelType, bool> OnBackPanel;
        public static Action OnShowPanel;
        public static Action OnBuyRemoveAds;
        public static Action OnBeginDragDrake;
        public static Action<PanelType> OpenPanel;
        public static Action OnCorrect;
        public static Action OnEndDragDrake;
        public static Action OnEndDragMop;
        public static Action OnLose;
        public static Action OnUpdateGift;
        public static Action<int, Action> OnUpdateShapeBoard;
        public static Action<int, Action> OnUpdateShapeGlocies;
        public static Action<Transform> OnDragBackItem;
        public static Action<bool> OnOpenDoor;
        public static Action<Panel, PanelType> OnPlaygame;
        public static Action<int, BackItem> OnCarryItem;
        public static Action<int, BackItem> OnStandTable;
        public static Action<int, BackItem, Vector3> OnWolfooSitToChair;
        public static Action<int, PriceItem, Sprite> InitAdsPanel;
        public static Action<int, Sprite> InitAdsPanelWithNoCoin;
        public static Action GetNextShapeBoard;
        public static Action<GameObject, GameObject> GetMainPanel;
        public static Action<Vector3> OnDragMop;
        public static Action<Vector3, BackItem> OnJumpToGarbage;
        public static Action<int, BackItem> OnClickBackItem;
        public static Action<BackItem, int> OnEndDragBackItem;
        public static Action<List<BallAnimation>, BallAnimation> OnMoveBall;
        public static Action<int> OnResultMoveOut;
        public static Action<int, int, ResultItem> OnEndDragResult;
        public static Action<int, int> GetCompleteShapeCut;
        public static Action<int, bool> OnClickAlpha;
        public static Action<CrayonItem> ChangeCrayon;
        public static Action<int, PaperItem> OnClickPaper;
        public static Action<int, ShapeItem> OnClickEmptyShape;
        public static Action<int, DecorDragItem> OnBeginDragDecor;
        public static Action<int, DecorDragItem, float> OnEndDrag;
        public static Action<bool> OnCollisionObstacle;
        public static Action OnCompletedRace;
        public static Action<PanelType> OnClickPanel;
        public static Action OnEndSession;
        public static Action<GameObject> OnCompleteMove;
        public static Action OnCompletedHalfGame;
        public static Action OnCompleteCutBox;
        public static Action OnCloseReadyPanel;
        public static Action<bool> OnScratch;
        public static Action OnClickTutorial;
        public static Action<bool> OnCompleteOpenGift;
        public static Action<int> OnPlayRandomGame;
        public static Action<int> OnInitCoin;
        public static Action<int> OnGetNewSticker;
        public static Action<int, PriceItem> OnWatchAds;
        public static Action<int, ObstacleGalaxy> OnCollisionObstacleGalaxy;
        public static Action<GameObject, PanelType, bool, GameObject> OnEndgame;
    }

}