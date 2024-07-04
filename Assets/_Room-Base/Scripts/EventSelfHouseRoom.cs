using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class EventSelfHouseRoom : EventRoomBase
    {
        public static Action<OptionScrollItem> OnInitScrollItem;
        public static Action<CustomRoomItem> OnBeginDragCustomItem;
        public static Action<CustomRoomItem> OnEndDragCustomItem;
        public static Action<CustomRoomItem> OnDragCustomItem;
        public static Action<OptionScrollItem> OnDragScrollItem;
        public static Action<OptionScrollItem> OnBeginDragScrollItem;
        public static Action<OptionScrollItem> OnEndDragScrollItem;
        /// <summary>
        /// Bool params name is "IsOpen"
        /// </summary>
        public static Action<bool> OnClickDecorOption;
        public static Action<FloorPaintingWorld> OnEndDragFloorPainting;
        public static Action<WallPaintingWorld> OnEndDragWallPainting;
    }
}
