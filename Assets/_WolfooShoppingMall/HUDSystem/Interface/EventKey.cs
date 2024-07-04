using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SCN;
using UnityEngine.UI;
using _Base;

namespace _WolfooShoppingMall
{
    public class EventKey : IEventParams
    {
        public struct OnSelect : IEventParams
        {
            public int idx;
            public ClipItem clipItem;
            public Floor floor;
            public Direction direction;
            public Elevator elevator;
            public ElevatorPanel elevatorPanel;
            public FilmMachine filmMachine;
            public int subIdx;
            public _Base.CityType mapControllerType;
            public MapController mapController;
            public MapLivingController mapLivingController;
            public MapHospitalController mapHospitalController;
            public MapPlaygroundController mapPlaygroundController;
            public MapOperaController mapOperaController;
            public MapBeachVillaController mapBeachVillaController;
            public MapCampingParkController mapCampingParkController;
            internal MapMediterraneanController mediterraneanController;
        }
        public struct OnCollision : IEventParams
        {
            public int idx;
            public ClawMachineToy toy;
            public string tag;
        }
        public struct OnRemoveAds : IEventParams
        {
        }
        public struct OnLoadDataCompleted : IEventParams
        {
        }
        public struct OnClickItem : IEventParams
        {
            public int idx;
            public VideoBannerPanel videoPanel;
            public ButtonWorldSpace button;
            public DollClothingScrollItem dollClothingItem;
            public CaptureComponentScrollItem captureItem;
            public IceScreamScrollItem iceScreamItem;
            public OperaClothesScrollItem operaClothesScrollItem;
            public BackClick background;
        }
        public struct OnBeginDragBackItem : IEventParams
        {
            public BackItem backitem;
            public Clothing clothing;
            public Character character;
            public NewCharacterUI newCharacter;
            public PizzaTopping pizzaTopping;
            public Pizza pizza;
            public BeverageLid beverageLid;
            public Food food;
            public FaceMask faceMask;
            public ToiletPaper paper;
            public Hat hat;
            public Pan pan;
        }
        public struct OnDragBackItem : IEventParams
        {
            public BackItem backItem;
            public MopItem mop;
            public Perfume perfume;
            public WaterProvider waterProvider;
        }
        public struct OnEndDragBackItem : IEventParams
        {
            public PlassticCup plasticup;
            public BeverageLid beverageLid;
            public PopcornBox popcornBox;
            public Popcorn popcorn;
            public BackItem backitem;
            public WaterBottle waterBottle;
            public Ball ball;
            public Toy toy;
            public CarToy carToy;
            public Character character;
            public GiftDecal giftDecal;
            public MoneyPolime money;
            public Flower flower;
            public Shovel shovel;
            public Peanut peanut;
            public WaterProvider waterProvider;
            public ShoppingBasket shoppingBasket;
            public Cake cake;
            public Plate plate;
            public CreamHandmade cream;
            public Clothing clothing;
            public PizzaTopping pizzaTopping;
            public Pizza pizza;
            public Food food;
            public Glasses glasses;
            public Straw straw;
            public MopItem mop;
            public BagItem bag;
            public News news;
            public FaceMask faceMask;
            public DentistTool dentisTool;
            public TempueratureMachine tempueratureMachine;
            public ToiletPaper paper;
            public StickerBackItem sticker;
            public OperaHat operaHat;
            public Hat hat;
            public NewCharacterUI newCharacter;
            public Pan pan;
            public Flour flour;
            public Egg egg;
            public Fertilizer fertilizer;
        }
        public struct OnBeginDragItem : IEventParams
        {
            public CharacterScrollItem characterScroll;
        }
        public struct OnDragItem : IEventParams
        {
            public ClawControl clawControl;
            public Direction direction;
            public MakingCreamScrollItem makingCream;
        }
        public struct OnEndDragItem : IEventParams
        {
            public Character character;
            public CharacterScrollItem characterScroll;
            public DollClothingScrollItem dollClothingItem;
            public MakingCreamScrollItem makingCream;
            public CaptureComponentScrollItem captureItem;
            public Sticker sticker;
            public OperaClothesScrollItem operaClothesItem;
        }
        public struct OnClickBackItem : IEventParams
        {
            public Flower flower;
            public GiftDecal decal;
            public FridgeDoor fridgeDoor;
            public Lamp lamp;
            public bool state;
            public FilmTicket ticket;
        }
        public struct OnSuccess : IEventParams
        {
            public ClawMachineToy toy;
            public int id;
        }
        public struct OnBackClick : IEventParams
        {
            public PanelType myPanelType;
            public PanelType parentPanelType;
        }
        public struct OnTouchBackItem: IEventParams
        {
            public BackItem backItem;
            public BackItemWorld backItemWorld;
        }
        public struct OnInitItem : IEventParams
        {
            public ScrollRect scrollRect;
            public Transform ground;
            public DollClothingMode dollClothing;
            public HandmadeCakeMode handmadeCake;
            public CaptureMode captureMode;
            public List<Sprite> sprites;
            public IceScreamMode iceScream;
            public Food food;
            public ClawMachineMode clawMachineMode;
            public OperaClothesScrollItem operaClothesScrollItem;
            public OperaHatScrollItem operaHatScrollItem;
            public bool isEndClawMachineMode;
        }
        public struct InitAdsPanel : IEventParams
        {
            public int instanceID;
            public int idxItem;
            public int idxSubItem;
            public Sprite spriteItem;
            public FilmMachine filmMachine;
            public ClipItem clipItem;
            public string nameObj;
            public string curPanel;
            public CharacterItem characterItem;
            public CharacterPaletteColorItem characterPaletteColorItem;
            public Color color;
            public string adsType;
            public string title;
        }
        public struct OnWatchAds : IEventParams
        {
            public int instanceID;
            public int idxItem;
            public int idxSubItem;
            public CharacterItem characterItem;
            public CharacterPaletteColorItem characterPaletteColorItem;
        }
        public struct OnWatchInterAds : IEventParams
        {
        }
    }

}