using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames.Puzzle
{
    public class PlayerData
    {
        public static int indexNumPieces = 0;
        public static int idCol = 0;
        public static int idItem = 0;
        public static int indexImg = 0;
        private static string countHint = "countHint";
        private static string isFirstTime = "isFirstTime";
        private static string rank = "rank";
        public static int Rank
        {
            get => PlayerPrefs.GetInt(rank);
            set => PlayerPrefs.SetInt(rank, value);
        }
        public static int CountHint
        {
            get => PlayerPrefs.GetInt(countHint);
            set => PlayerPrefs.SetInt(countHint, value);
        }
        public static bool IsFirstTime
        {
            get => PlayerPrefs.GetInt(isFirstTime) == 0 ? true : false;
            set => PlayerPrefs.SetInt(isFirstTime, value == false ? 1 : 0);
        }
    }
}