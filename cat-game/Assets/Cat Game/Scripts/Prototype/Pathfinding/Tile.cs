using System;
using UnityEngine;

namespace SiegeOfAshes.Pathfinding
{
    [Serializable]
    public class Tile : MonoBehaviour
    {
        public GameObject WorldReference;
        public Vector2 Position;
        public bool IsPassable;

        public Tile(GameObject worldReference, Vector2 position, bool isPassable)
        {
            WorldReference = worldReference;
            Position = position;
            IsPassable = isPassable;
        }
    }
}

