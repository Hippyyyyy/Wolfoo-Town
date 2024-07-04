using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "WFHouseData", menuName = "Scriptable Objects/" + "WFHouseData")]
    public class HouseDataSO : ScriptableObject
    {
        public Sprite[] FlowerSprites;
        public Sprite[] FruitSprites;
    }
}
