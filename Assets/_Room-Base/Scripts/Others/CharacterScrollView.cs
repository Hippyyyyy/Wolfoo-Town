using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace _WolfooShoppingMall
{
    public class CharacterScrollView : PlayerScrollView
    {
        public override void Setup(UIManager.LimitArea limitArea)
        {
            myLimit = new Transform[4] { limitArea.downLimit, limitArea.rightLimit, limitArea.upLimit, limitArea.leftLimit };
        }
    }
}
