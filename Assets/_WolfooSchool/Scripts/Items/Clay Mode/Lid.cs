using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooSchool
{
    public class Lid : ItemMove
    {
        public void AssignItem(Sprite _sprite)
        {
            itemImg.sprite = _sprite;
            itemImg.SetNativeSize();
        }
    }
}