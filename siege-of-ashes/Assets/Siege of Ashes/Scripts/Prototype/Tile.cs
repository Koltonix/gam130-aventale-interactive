using System;

namespace SiegeOfAshes.Tiles
{
    [Serializable]
    public struct Tile
    {
        public int X;
        public int Y;
        public bool IsPassable;
    }
}
