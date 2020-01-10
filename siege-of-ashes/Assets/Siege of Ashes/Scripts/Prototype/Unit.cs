using System.Collections.Generic;
using UnityEngine;
using SiegeOfAshes.Tiles;

namespace SiegeOfAshes.Movement
{
    public class Unit : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField]
        private new string name;
        [Space]

        [Header("Movement Settings")]
        [SerializeField]
        private int movementPoints;
        public Tile[] currentTilesAvailable;

        private void Start()
        {
            BoardManager.Instance.onBoardUpdate += DetermineAvailableTiles;
        }

        #region Tile Prediction
        /// <summary>
        /// A function that returns all of the tiles that the unit
        /// can move to given the amount of movement points available.
        /// </summary>
        /// <param name="allTiles">
        /// An array including all of the tiles in the scene.
        /// </param>
        /// <remarks>
        /// **NOTE** This current does not incorporate any 
        /// A* pathfinding yet, but will do during the stages after 
        /// the prototype.
        /// </remarks>
        /// <returns></returns>
        public void DetermineAvailableTiles(Tile[] allTiles)
        {
            Vector3 unitPosition = this.transform.position;
            List<Tile> accessibleTiles = new List<Tile>();

            foreach(Tile tile in allTiles)
            {
                if (tile.IsPassable)
                {
                    float tileDistance = Vector3.Distance(tile.Position, unitPosition);
                    tileDistance = Mathf.RoundToInt(tileDistance);
                    if (tileDistance <= movementPoints)
                    {
                        accessibleTiles.Add(tile);
                        tile.GameObject.GetComponent<Renderer>().material.color = Color.red;
                    }
                }
            }

            currentTilesAvailable = accessibleTiles.ToArray();
        }
        #endregion
    }
}

