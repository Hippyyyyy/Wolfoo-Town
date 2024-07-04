using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooShoppingMall
{
    public class AdsEventManager
    {
        public static Action<Action, Action> ShowIntersitialAds;
        public static Action<Action, Action> ShowRewardAds;
        public static Action ShowBannerAds;
        public static Action HideBannerAds;
    }
}
