using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCN;
using UnityEngine.UI;
using _WolfooCity.Minigames.Puzzle;

namespace _WolfooCity
{
    public class EventKey : IEventParams
    {
        public struct OnDestroyScene : IEventParams
        {
        }
        public struct OnInitItem : IEventParams
        {
            public ScrollRect scrollRect;
            public Transform ground;
            //public DollClothingMode dollClothing;
            //public HandmadeCakeMode handmadeCake;
            //public CaptureMode captureMode;
            public List<Sprite> sprites;
            //public IceScreamMode iceScream;
            //public Food food;
            //public ClawMachineMode clawMachineMode;
            public bool isEndClawMachineMode;
        }
        public struct OnLoadCompleted : IEventParams
        {
            public List<object> data;
        }
        public struct InitAdsPanel : IEventParams
        {
            public int instanceID;
            public int idxItem;
            public int idxSubItem;
            public Sprite spriteItem;
            //public FilmMachine filmMachine;
            //public ClipItem clipItem;
            public string nameObj;
        }
        public struct OnWatchAds : IEventParams
        {
            public int instanceID;
            public int idxItem;
            public int idxSubItem;
        }
        public struct CheckProgress : IEventParams
        {

        }
        public struct OnStarEffect : IEventParams
        {
            public Transform trans;
        }
        public struct OnDragPiece : IEventParams
        {
            public int idPiece;
            public Quaternion quaternion;
            public ItemPiece itemPiece;
        }
        public struct OnHintPiece : IEventParams
        {
            public int idPiece;
            public Quaternion quaternion;
        }
        public struct DoneDragItemPiece : IEventParams
        {
            public int idPiece;
        }
        public struct ReturnItemPiece : IEventParams
        {
            public int idPiece;
        }
        public struct OnChangeNumberPieces : IEventParams
        {
            public int numPiece;
            public int index;
        }
        public struct OnSelectPicture : IEventParams
        {
            public int idCol;
            public int idItem;
        }
        public struct OnHint : IEventParams
        {
        }
        public struct OnHintBottom : IEventParams
        {
            public bool isHint;
        }
        public struct OnHintImage : IEventParams
        {
            public bool isHint;
        }
        public struct UpdateUIIngame : IEventParams
        {
        }
        public struct OpenPanel : IEventParams
        {
            public PanelType panelType;
        }
    }
}