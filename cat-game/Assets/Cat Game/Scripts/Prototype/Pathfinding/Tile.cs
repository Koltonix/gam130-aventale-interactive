using System;
using UnityEngine;

namespace CatGame.Pathfinding
{
    [Serializable]
    public class Tile
    {
        public int boardX;
        public int boardY;

        public GameObject worldReference;
        public Color defaultColour;
        public Vector2 position;

        public bool isPassable;
        public bool isOccupied;

        public int hCost;
        public int gCost;
        public int FCost { get { return hCost + gCost; } }

        public Tile parent;

        public Tile(int x, int y, GameObject worldReference, Vector2 position, bool isPassable)
        {
            this.worldReference = worldReference;
            this.position = position;
            this.isPassable = isPassable;
            isOccupied = false;

            boardX = x;
            boardY = y;

            defaultColour = worldReference.GetComponent<Renderer>().material.color;
        }
    }
}

