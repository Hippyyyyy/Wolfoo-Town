using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class CharacterFeatureLibraryManager : MonoBehaviour
    {
        private static CharacterFeatureLibraryManager _instance;
        public static CharacterFeatureLibraryManager Instance => _instance;

        public CharacterFeatureLibrary CharacterFeatureLibrary { get => characterFeatureLibrary; set => characterFeatureLibrary = value; }

        [SerializeField]
        CharacterFeatureLibrary characterFeatureLibrary;

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        public CharacterFeatureLibrary GetCharacterFeatureLibrary()
        {
            return CharacterFeatureLibrary;
        }
    }
}
