using System.Collections.Generic;
using UnityEngine;
using CatGame.Tiles;
using CatGame.Data;

namespace CatGame.Units
{
    public class Mover : MonoBehaviour
    {
        [Header("Movement Settings")]
        public int actionPoints;
        private int defaultActionPoints;
        public Tile[] currentTilesAvailable;

        private Unit unit;

        private void Start()
        {
            unit = this.GetComponent<Unit>();
            unit.owner.onActive += SetIsActive;

            defaultActionPoints = actionPoints;
            BoardManager.Instance.onBoardUpdate += DetermineAvailableTiles;
            TurnManager.Instance.onPlayerCycle += OnPlayerCycle;
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
            unitPosition.y = 0;

            List<Tile> accessibleTiles = new List<Tile>();

            foreach (Tile tile in allTiles)
            {
                if (tile.IsPassable)
                {
                    float tileDistance = Vector3.Distance(new Vector3(tile.Position.x, 0, tile.Position.z), unitPosition);

                    tileDistance = Mathf.RoundToInt(tileDistance);
                    if (tileDistance <= actionPoints & tileDistance > 0)
                    {
                        accessibleTiles.Add(tile);
                    }
                }
            }

            currentTilesAvailable = accessibleTiles.ToArray();
        }

        /// <summary>
        /// A check to see if the tile that has been provided is a 
        /// tile that is currently available to move to.
        /// </summary>
        /// <param name="tileToMoveTo">
        /// The tile that is to be checked against all of the 
        /// available tiles.
        /// </param>
        /// <returns>
        /// Returns true if the tile provided is a tile that is
        /// available to move to.
        /// </returns>
        public bool CanMoveToTile(Tile tileToMoveTo)
        {
            foreach (Tile tile in currentTilesAvailable)
            {
                if (tile == tileToMoveTo)
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeAvailableTilesColour(Color32 color)
        {
            foreach (Tile tile in currentTilesAvailable)
            {
                tile.GameObject.GetComponent<Renderer>().material.color = color;
            }
        }

        public void ResetTileColours()
        {
            foreach (Tile tile in currentTilesAvailable)
            {
                tile.GameObject.GetComponent<Renderer>().material.color = tile.Colour;
            }
        }

        public void ResetActionPoints()
        {
            actionPoints = defaultActionPoints;
        }

        #endregion

        #region Event Listener
        /// <summary>
        /// Used as a listener to invoke other functions
        /// </summary>
        /// <param name="isSelected">
        /// Determines whether the unit has been selected or deselected
        /// </param>
        public void SelectionListener(bool isSelected)
        {
            DetermineAvailableTiles(BoardManager.Instance.tiles);
            if (!isSelected) ResetTileColours();
        }

        private void OnPlayerCycle(Player player)
        {

        }

        private void SetIsActive()
        {
            
        }


        #endregion
    }
}

