using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SelfHouseDataSO : ScriptableObject
    {
        public DecoratedData[] DecorDatas;
        public PaintingData[] WallPaintingDatas;
        public PaintingData[] FloorPaintingDatas;

        [System.Serializable]
        public struct DecoratedData
        {
            public OptionData[] OptionDatas;
        }
        [System.Serializable]
        public struct OptionData
        {
            public Sprite Icon;
            public Sprite[] ItemSprites;
            public GameObject[] ItemPrefabs;
            public OptionScrollItem OptionScrollItems;
        }
        [System.Serializable]
        public struct PaintingData
        {
            public PaintingColorName colorName;
            public Color color;
        }
        public enum PaintingColorName
        {
            Red,
            Blue,
            Purple,
            Yellow,
            Pink,
            LowGreen,
            HighGreen,
            Brown,
            Begie
        }
    }
}
