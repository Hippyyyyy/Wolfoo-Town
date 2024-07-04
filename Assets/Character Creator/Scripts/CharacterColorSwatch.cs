using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
	[CreateAssetMenu]
    public class CharacterColorSwatch : ScriptableObject
    {
		
		public int ID;

		public CharacterColorCategory Category;

		public Color Color1;

		public Color Color2;

        public bool IsFree;
#if UNITY_EDITOR
        private void OnValidate()
        {
            Color1.a = 1;
            Color2.a = 1;
        }
#endif
    }

}
