using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class Camping : Table
    {
        [SerializeField] Transform itemZone;
        protected override void OnDistanceVerified(BackItem backitem)
        {
            var _character = backitem.GetComponent<Character>();
            if (_character != null)
            {
                _character.OnSitToChair(itemZone.position, itemZone);
            }

            var _newCharacter = backitem.GetComponent<NewCharacterUI>();
            if (_character != null)
            {
                _newCharacter.OnSitToChair(itemZone.position, itemZone);
            }
        }
    }
}