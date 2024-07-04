using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = Const.ITEM_DATA_SO, menuName = "Scriptable Objects/" + Const.ITEM_DATA_SO)]
    public class ItemsDataSO : ScriptableObject
    {
        public MachineToyData MachineToyData;
        public GiftDecalData GiftDecalData;
        public CharacterData CharacterData;
        public DollClothingData DollClothingData;
        public HandmadeCakeData HandmadeCakeData;
        public MashmallowData MashmallowData;
        public CaptureModeData CaptureModeData;
        public IceScreamModeData IceScreamModeData;
        public MeatData[] MeatData;
        public ParticleSystem smokeFxPb;
    }

    [System.Serializable]
    public struct MashmallowData
    {
        public Sprite[] sprites;
    }
    [System.Serializable]
    public struct LoadingData
    {
        public GameObject[] loadingAnims;
    }
    [System.Serializable]
    public struct CharacterData
    {
        public Sprite[] sprites;
        public Character[] characterPbs;
        public Sprite[] frontSkinSprite;
        public Sprite[] behindSkinSprite;
        public Sprite[] foldSkinSprite;
        public ParticleSystem rainbowFxPb;
        public ParticleSystem starFxPb;
    }
    [System.Serializable]
    public struct HandmadeCakeData
    {
        public HandMadeCake[] handMadeCakePbs;
        public HandmadeCakeComponent[] components;
        public Sprite[] creamSprites;
    }
    [System.Serializable]
    public struct HandmadeCakeComponent
    {
        public Item[] items;
        [System.Serializable]
        public struct Item
        {
            public Sprite[] sprites;
        }
    }
    [System.Serializable]
    public struct DollClothingDict
    {
        public int curTopicIdx;
        public int curItemIdx;
    }

    [System.Serializable]
    public struct DollClothingData
    {
        public Sprite[] topicBtnSprites;
        public Sprite[] topicBtnSprites2;
        public Sprite[] dressTopicData;
        public Sprite[] hairTopicData;
        public Sprite[] eyeHairTopicData;
        public Sprite[] accessoryTopicData;
        public Vector3[] accessoryPosData;
        public Vector3[] dressPosData;
        public Vector3[] hairPosData;
        public DollClothingDict[] dollClothingDicts;
    }
    [System.Serializable]
    public struct IceScreamModeData
    {
        public Sprite[] topicBtnSprites;
        public Sprite[] topicBtnSprites2;
        public Sprite[] chooseBtnSprites;
        public Sprite[] screamTopicSprites;
        public ToppingData[] toppingTopicData;
        public Sprite[] coneTopicData;
        public Sprite[] creamSprites;
        public CreamMachineAnimation.ColorType[] colorAnimTypes;

        [System.Serializable]
        public struct ToppingData
        {
            public Sprite topicSprite;
            public Sprite[] itemSprites;
        }
    }
    [System.Serializable]
    public struct CaptureModeData
    {
        public Sprite[] topicBtnSprites;
        public Sprite[] topicBtnSprites2;
        public CharacterTopicData[] characterTopicData;
        public Sprite[] characterAvaTopicData;
        public Sprite[] frameTopicData;
        public Sprite[] stickerTopicData;

        [System.Serializable]
        public struct CharacterTopicData
        {
            public Sprite[] toySprites;
        }
    }

    [System.Serializable]
    public struct MachineToyData
    {
        public Sprite[] toySprites;
    }
    [System.Serializable]
    public struct MeatData
    {
        public Sprite idleSprt;
        public Sprite grilledSprt;
    }

    [System.Serializable]
    public struct GiftDecalData
    {
        public Sprite[] decalSprites;
        public Sprite[] packageSprites;
        public TiedFlowerData[] tiedFlowerSprites;

        [System.Serializable]
        public struct TiedFlowerData
        {
            public Sprite[] tiedFlowerSprites;
        }
    }

}