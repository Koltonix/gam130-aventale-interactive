using System;
using UnityEngine;

namespace SiegeOfAshes.Tiles
{
    [Serializable]
    public class Tile
    {
        public Vector3 Position;
        public GameObject GameObject;
        public bool IsPassable;

        public Tile(Vector3 Position, GameObject GameObject)
        {
            this.Position = Position;
            this.GameObject = GameObject;
            IsPassable = GameObject.layer == 9 ? true : false;
        }
    }
}
