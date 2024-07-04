using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "Room Floor Config", menuName = "Config/" + "Room Floor Config")]
    public class RoomFloorConfig : ScriptableObject
    {
        public float speed;
        public float dragDetectMax;
        public float dragDetectMin;
        public float smoothTime;

        [Header("####################   CONSTANT LAYER   ####################")]
        public int FLOOR_LAYER  = 10;
        public int TABLE_LAYER  = 15;
        public int OTHER_LAYER  = 6;
        public int OBSTACLE_LAYER  = 7;
        public int TABLE_FOOT  = 16;

        [Header("####################   CONSTANT BUTTON   ####################")]
        public string BACK_BUTTON            = "back button";
        public string OPEN_CHARACTER_BUTTON  = "open character button";
        public string ADD_CHARACTER_BUTTON   = "add character button";
    }
}
