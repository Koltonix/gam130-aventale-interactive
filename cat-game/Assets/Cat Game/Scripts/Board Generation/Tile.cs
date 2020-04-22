using System;
using UnityEngine;
using CatGame.Units;
using CatGame.Data;

namespace CatGame.Tiles
{
    /// <summary>
    /// Stores the Tile Data of the Board. It stores the position both in the 
    /// board and the world position. It also stores values to be used in the
    /// Pathfinding too.
    /// </summary>
    [Serializable]
    public class Tile
    {
        public Vector3 Position;
        public GameObject WorldReference;
        public Color32 DefaultColour;

        public Entity OccupiedEntity;

        public bool IsPassable;

        public int boardX;
        public int boardY;

        public int hCost;
        public int gCost;
        public int FCost { get { return hCost + gCost; } }

        public Tile parent;

        public bool isUsedInPathfinding;

        /// <summary>
        /// Constructor for the Tile.
        /// </summary>
        /// <param name="Position">World Position</param>
        /// <param name="GameObject">World GameObject Reference.</param>
        /// <param name="x">X Board Position.</param>
        /// <param name="y">Y Board Position.</param>
        public Tile(Vector3 Position, GameObject GameObject, int x, int y)
        {
            this.Position = Position;
            this.WorldReference = GameObject;
            this.OccupiedEntity = null;

            boardX = x;
            boardY = y;

            IsPassable = GameObject.layer == 9 ? true : false;
            DefaultColour = GameObject.GetComponent<Renderer>().material.color;

            isUsedInPathfinding = false;
        }

        /// <summary>Checks above the tile to see if there is a Unit currently on top of it.</summary>
        /// <returns>A Unit Class of what is standing above.</returns>
        /// <remarks>This is a nullable return type.</remarks>
        public Entity CheckForEntity()
        {
            Collider[] cols = Physics.OverlapSphere(Position, 1.5f);
            foreach (Collider nearbyObject in cols)
            {
                Entity unit = nearbyObject.GetComponent<Entity>();
                if (unit != null)
                {
                    OccupiedEntity = unit;
                    return unit;
                }
            }

            OccupiedEntity = null;
            return null;
        }
    }
}
