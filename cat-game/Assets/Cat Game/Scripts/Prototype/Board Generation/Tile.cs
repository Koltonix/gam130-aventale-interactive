using System;
using UnityEngine;
using CatGame.Units;

namespace CatGame.Tiles
{
    [Serializable]
    public class Tile
    {
        public Vector3 Position;
        public GameObject GameObject;
        public Unit OccupiedUnit;

        public bool IsPassable;
        public Color32 Colour;
        

        public Tile(Vector3 Position, GameObject GameObject)
        {
            this.Position = Position;
            this.GameObject = GameObject;
            this.OccupiedUnit = null;

            IsPassable = GameObject.layer == 9 ? true : false;
            Colour = GameObject.GetComponent<Renderer>().material.color;
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
