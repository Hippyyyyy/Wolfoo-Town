using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterTransporter
    {
        public static List<CharacterController> properitseHolders;

        public static void Init()
        {
        }
        public static int TotalCharacterHolder
        {
            get
            {
                try
                {
                    var length = CharacterDataHolder.Instance.AssetLength;
                    return length;
                }
                catch (System.Exception)
                {
                    return 3;
                }
            }
        }
        public static CharacterController WorldCharacter
        {
            get
            {
                var item = CharacterDataHolder.Instance.WorldCharacter;
                return item;
            }
        }
        public static CharacterController UICharacter
        {
            get
            {
                var item = CharacterDataHolder.Instance.UICharacter;
                return item;
            }
        }
        public static CharacterController UIInRoomCharacter
        {
            get
            {
                var item = CharacterDataHolder.Instance.UIInRoomCharacter;
                return item;
            }
        }
        public static CharacterFeatureLibrary.Data GetNextDataSet()
        {
            return CharacterDataHolder.Instance.GetNextDataSet();
        }
        public static CharacterFeatureLibrary.Data GetDataSet(int id)
        {
            return CharacterDataHolder.Instance.GetDataSet(id);
        }
    }
}
