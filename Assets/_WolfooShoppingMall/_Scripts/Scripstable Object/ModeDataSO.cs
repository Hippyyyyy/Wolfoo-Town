using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = Const.MODE_DATA_SO, menuName = "Scriptable Objects/" + Const.MODE_DATA_SO)]
    public class ModeDataSO : ScriptableObject
    {
        public MapData[] MapDatas;
    }

    [System.Serializable]
    public struct MapData
    {
        public MapType mapType;
        public GameObject panel;
    }
}