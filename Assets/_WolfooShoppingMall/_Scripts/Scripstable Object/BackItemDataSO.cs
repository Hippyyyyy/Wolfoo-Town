using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using static _WolfooShoppingMall.CreamHandmade;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = Const.BACK_ITEM_DATA_SO, menuName = "Scriptable Objects/" + Const.BACK_ITEM_DATA_SO)]
    public class BackItemDataSO : ScriptableObject
    {
        public PlasticCupData plasticCupData;
        public FilmTicketData filmTicketData;
        public FilmData filmData;
        public DoorData doorData;
        public FlowerData flowerData;
        public CakeData CakeData;
        public PizzaData PizzaData;
        public ParticleSystem smokeFallingFxPb;
    }

    [System.Serializable]
    public struct PlasticCupData
    {
        public Sprite[] beverageColorSprites;
        public Sprite sideDownSprite;
    }
    [System.Serializable]
    public struct PizzaData
    {
        public Sprite[] toppingSprites;
        public Sprite[] pizzaSprites;
        public Sprite boxSprite;
    }
    [System.Serializable]
    public struct CakeData
    {
        public Sprite[] cakeSprites;
        public Sprite[] creamCakeSprites;
        public Sprite[] creamHandmadeSprites;
        public CreamColor[] colors;
    }
    [System.Serializable]
    public struct DoorData
    {
        public Sprite[] blueDoorSprites;
        public Sprite[] stoveDoorSprites;
        public Sprite[] elevatorSprites;
        public Sprite[] curtainsSprites;
    }
    [System.Serializable]
    public struct FlowerData
    {
        public Sprite[] idleSprites;
        public Sprite[] tiedSprites;
        public Sprite[] peanutSprites;
    }

    [System.Serializable]
    public struct FilmTicketData
    {
        public Sprite[] ticketColorSprites;
        public Sprite[] moneySprites;
    }
    [System.Serializable]
    public struct FilmData
    {
        public FilmClipData[] clipsData;
        public Sprite[] thumbnailSprites;
        public VideoClip[] allClips;
    }
    [System.Serializable]
    public struct FilmClipData
    {
        public VideoClip[] episodeClips;
    }
}