using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    [CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/" + "CharacterData")]
    public class CharacterDataSO : ScriptableObject
    {
        public CharacterData CharacterData;
        public ParticleSystem smokeFxPb;
        public ParticleSystem lightingFxPb;
    }
}