using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class SpeakerWorld : SoundMachine
    {
        public override void Setup()
        {
            IsDragable = true;
            base.Setup();
        }
    }
}
