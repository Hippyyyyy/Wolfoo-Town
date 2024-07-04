using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class EventRoomBase
    {
        public static Action<BackItemWorld> OnEndDragBackItem;
        public static Action<BackItemWorld> OnDragBackItem;
        public static Action<BackItemWorld> OnBeginDragBackItem;
        public static Action<BackItemWorld> OnInitBackItem;
        public static Action<BackItemWorld, bool> OnEndDragScrollCharacter;
        public static Action<BackItemWorld> OnBeginDragScrollCharacter;
        public static Action<string> OnClickUIButton;
        public static Action OnCreatedMinigame;
        public static Action OnCompletedMinigame;
        public static Action OnBeginDragFloor;
        public static Action OnEndDragFloor;
        public static Action OnOpenCharacterPanel;
        public static Action OnCloseCharacterPanel;
    }
}
