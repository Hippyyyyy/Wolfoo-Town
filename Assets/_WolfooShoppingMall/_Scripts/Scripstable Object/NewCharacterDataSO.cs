using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "New CharacterData", menuName = "Scriptable Objects/" + "New CharacterData")]
    public class NewCharacterDataSO : ScriptableObject
    {
        public GameObject[] ObjPrefabs;
        public GameObject[] WolfooPrefabs;
        public Sprite[] AvatarSprites;
        public Sprite[] WolfooSprites;
    }
}