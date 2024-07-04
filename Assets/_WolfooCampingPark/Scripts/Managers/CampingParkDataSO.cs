using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "CampingPark Data", menuName = "Scriptable Objects/" + "CampingPark Data")]
    public class CampingParkDataSO : ScriptableObject
    {
        public MilkTeaData[] MilkTeas;
        [System.Serializable]
        public struct MilkTeaData
        {
            public Sprite sprite;
            public Sprite waterSprite;
        }
    }
}
