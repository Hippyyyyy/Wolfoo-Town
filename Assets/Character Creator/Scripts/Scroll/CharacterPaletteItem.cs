using _WolfooShoppingMall;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public abstract class CharacterPaletteItem : MonoBehaviour
    {
        public abstract bool IsSelected();

        public abstract CharacterFeatureSet GetFeatureSet();

    }
