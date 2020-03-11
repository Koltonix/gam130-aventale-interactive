using System;
using UnityEngine;
using CatGame.Units;

namespace CatGame.Tiles
{
    [Serializable]
    public class Tile
    {
        public Vector3 Position;
        public GameObject WorldReference;
        public Color32 DefaultColour;

        public Unit OccupiedUnit;

        public bool IsPassable;

        public int boardX;
        public int boardY;

        public int hCost;
        public int gCost;
        public int FCost { get { return hCost + gCost; } }

        public Tile parent;

        public Tile(Vector3 Position, GameObject GameObject, int x, int y)
        {
            this.Position = Position;
            this.WorldReference = GameObject;
            this.OccupiedUnit = null;

            boardX = x;
            boardY = y;

            IsPassable = GameObject.layer == 9 ? true : false;
            DefaultColour = GameObject.GetComponent<Renderer>().material.color;
        }

        public Unit CheckForUnit()
        {
            RaycastHit hit;
            if (Physics.Raycast(Position, Vector3.up, out hit))
            { 
                Unit unit = hit.collider.GetComponent<Unit>();         
                if (unit != null)
                {
                    OccupiedUnit = unit;
                    return unit;
                }
            }

            OccupiedUnit = null;
            return null;
        }
    }
}
