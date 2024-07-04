using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class DataTownTransporter : MonoBehaviour
    {
        public static List<GameObject> roomDatas;

        public static void AddRoomData(GameObject obj)
        {
            if(roomDatas == null) roomDatas = new List<GameObject>();
            roomDatas.Add(obj);
        }
        public static void ReleaseRoomData()
        {
            if(roomDatas != null) roomDatas.Clear();
        }
    }
}
