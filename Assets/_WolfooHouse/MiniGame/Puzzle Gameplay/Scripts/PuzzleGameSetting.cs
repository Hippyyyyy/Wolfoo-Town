using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames.Puzzle
{
    [CreateAssetMenu(fileName = "PuzzleSetting", menuName = "MiniGame/PuzzleGameplay Setting", order = 1)]
    public class PuzzleGameSetting : ScriptableObject
    {
        public int startIdxPicture;
        public Sprite[] Sprites;
    }
}