using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Cabinet : MonoBehaviour
    {
        [SerializeField] Table[] tableZone;
        [SerializeField] Door[] doors;

        private void Start()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].OnTouched += GetDoorTouched;
                doors[i].AssignIndex(i);
            }
        }
        private void OnDestroy()
        {
            for (int i = 0; i < doors.Length; i++)
            {
                doors[i].OnTouched -= GetDoorTouched;
            }
        }

        private void GetDoorTouched(Door door)
        {
            var idx = door.Idx;
            tableZone[idx].IsEnable = door.IsOpen;
        }
    }
}