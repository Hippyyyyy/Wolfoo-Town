using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames
{
    public class EventManager : MonoBehaviour
    {
        public static Action OnDiced;
        public static Action OnChessManJumped;
        public static Action OnJumpToEndTile;
        public static Action OnSlideComplete;
        public static Action OnFallDown;
        public static Action OnDicerInScreen;
        public static Action OnConfettiComplete;
        public void OnDiceComplete()
        {
            OnDiced?.Invoke();
        }
        public void OnChessJumped()
        {
            OnChessManJumped?.Invoke();
        }
        public void OnChessSlided()
        {
            OnSlideComplete?.Invoke();
        }
        public void OnFalled()
        {
            OnFallDown?.Invoke();
        }
        public void OnJumpIntoScreen()
        {
            OnDicerInScreen?.Invoke();
        }
        public void OnConffetEnded()
        {
            OnConfettiComplete?.Invoke();
        }
    }
}