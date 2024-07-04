using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace _WolfooCity.Minigames
{
    public class TileMap : MonoBehaviour
    {
        [SerializeField] Tile[] tiles;
        [SerializeField] Chessman chessman;
        [SerializeField] Chessman chesswoman;

        public Tile[] Tiles { get => tiles; }
        public Chessman Chesswoman { get => chesswoman; }
        public Chessman Chessman { get => chessman; }
    }
}