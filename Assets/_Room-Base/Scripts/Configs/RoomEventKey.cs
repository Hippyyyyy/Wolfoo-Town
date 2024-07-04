using SCN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class RoomEventKey: IEventParams
    {
        public struct Chair: IEventParams
        {
            public CharacterWolfooWorld wolfooWorld;
            public CharacterWorld newCharacter;
            public ChairWorld chair;
            public SeatWorld seat;
        }
        public struct BedWithCharacter: IEventParams
        {
            public CharacterWolfooWorld wolfooWorld;
            public CharacterWorld newCharacter;
            public BedWorld bed;
        }
    }
}
