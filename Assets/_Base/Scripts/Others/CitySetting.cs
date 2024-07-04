using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _Base.Helper
{
    [CreateAssetMenu(fileName = "CitySetting", menuName = "Scriptable Objects/" + "CitySetting")]
    public class CitySetting : ScriptableObject
    {
        public string WFCITY_SCENE;
        public string WFSHOPPINGMALL_SCENE;
        public string WFSCHOOL_SCENE;
        public string WFPLAYGROUND_SCENE;
        public string WFHOUSE_SCENE;
        public string WFOPERA_SCENE;
        public string WFHOSPITAL_SCENE;
        public string WFBEACHVILLA_SCENE;


    }
}