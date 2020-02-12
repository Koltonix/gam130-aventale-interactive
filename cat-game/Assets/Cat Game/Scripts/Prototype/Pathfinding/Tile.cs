using System;
using UnityEngine;

namespace SiegeOfAshes.Pathfinding
{
    [Serializable]
    public class Tile
    {
        public int boardX;
        public int boardY;

        public GameObject worldReference;
        public Vector2 position;

        public bool isPassable;
        public bool isOccupied;

        public int hCost;
        public int gCost;
        public int FCost { get { return hCost + gCost; } }

        public Tile parent;

        public Tile(GameObject worldReference, Vector2 position, bool isPassable)
        {
            this.worldReference = worldReference;
            this.position = position;
            this.isPassable = isPassable;
        }
    }
}

