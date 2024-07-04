using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace _Base
{
    [CreateAssetMenu(fileName = "DataSetting", menuName = "Scriptable Objects/" + "DataSetting")]
    public class DataSetting : ScriptableObject
    {
        public TownData[] TownDatas;
        public string[] AddressDatas;
        public AssetLabelReference DialogSchoolTag;
        public AssetLabelReference DialogMallTag;

        [System.Serializable]
        public struct TownData
        {
            public string title;
            public CityType cityType;
            public List<string> dataAssetPath;
            public string scenePath;
        }
    }
}