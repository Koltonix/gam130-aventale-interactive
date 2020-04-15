﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Tiles;
using CatGame.Data;
using CatGame.Pathfinding;

namespace CatGame.Units
{
    public class UnitMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public Tile currentTile;

        public List<Tile> availableTiles;
        public Tile[] nearbyUnits;
        public Dictionary<Tile, List<Tile>> tilePaths;

        [Header("Required Data")]
        public Player owner;
        public Unit currentUnit;

        [Header("Movement Colour")]
        public Color32 availableTileColour;
        public Color32 selectedTileColour;
        public Color32 enemyTileColour;
        public Color32 pathColour;

        private void Start()
        {
            currentUnit = this.GetComponent<Unit>();
            owner = currentUnit.owner;

            owner.GetPlayerReference().onActive += SetIsActive;

            BoardManager.Instance.onBoardUpdate += DetermineTilesInSphere;
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
        public void DetermineTilesInSphere(Tile[] allTiles)
        {
            List<Tile> accessibleTiles = new List<Tile>();
            List<Tile> availableUnits = new List<Tile>();

            currentTile = BoardManager.Instance.GetTileFromWorldPosition(this.transform.position);

            foreach (Tile tile in allTiles)
            {
                tile.CheckForUnit();
                tile.isUsedInPathfinding = false;

                if (tile.IsPassable && tile.OccupiedUnit == null)
                {
                    if (Mathf.Abs(tile.boardX - currentTile.boardX) <= owner.GetCurrentActionPoints())
                    {
                        if (Mathf.Abs(tile.boardY - currentTile.boardY) <= owner.GetCurrentActionPoints())
                        {
                            accessibleTiles.Add(tile);

                            if (tile.OccupiedUnit != null && tile.OccupiedUnit != currentUnit)
                            {
                                Debug.Log("UNIT IS HERE");
                                availableUnits.Add(tile);
                            }
                        }
                    }                 
                }
            }

            availableTiles = accessibleTiles;
            this.nearbyUnits = availableUnits.ToArray();
            tilePaths = PathfindAvailableTiles(accessibleTiles.ToArray());

            RemoveUnusedTiles();
        }

        //Returns the path using an end tile
        public Tile[] GetAvailableTilesFromPathfinding(Tile endTile)
        {
            if (tilePaths != null && endTile != null)
            {
                foreach (var tile in tilePaths)
                {
                    if (tile.Key.Position == endTile.Position)
                    {
                        return tile.Value.ToArray();
                    }
                }
            }

            return null;
        }

        public Dictionary<Tile, List<Tile>> PathfindAvailableTiles(Tile[] nearbyTiles)
        {
            //Stores the pathfinding for every tile available using the final Tile as the unique identifier
            Dictionary<Tile, List<Tile>> allPaths = new Dictionary<Tile, List<Tile>>();

            foreach (Tile endTile in nearbyTiles)
            {
                List<Tile> finalPath = PathfindingManager.Instance.GetPath(currentTile.Position, endTile.Position);
                if (finalPath == null) continue;
                //If there are only x tiles or less in the path
                if (finalPath.Count - 1 <= owner.GetCurrentActionPoints())
                {
                    allPaths.Add(endTile, finalPath);
                    SetTilesUsingPathfinding(finalPath.ToArray(), true);
                }
            }

            return allPaths;
        }
        
        private void SetTilesUsingPathfinding(Tile[] tiles, bool areUsed)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].isUsedInPathfinding = areUsed;
            }
        }

        private void RemoveUnusedTiles()
        {
            for (int i = availableTiles.Count - 1; i >= 0; i--)
            {
                if (!availableTiles[i].isUsedInPathfinding) availableTiles.RemoveAt(i);
            }
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
            foreach (Tile tile in availableTiles)
            {
                if (tile == tileToMoveTo)
                {
                    return true;
                }
            }

            return false;
        }

        public void ChangeAvailableTilesColour(Color32 colour)
        {
            foreach (Tile tile in availableTiles)
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = colour;
            }
        }

        public void ChangeEnemyTilesColour(Color32 colour)
        {
            foreach (Tile tile in nearbyUnits)
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = colour;
            }
        }

        public void ChangeTileColours(Tile[] tiles, Color32 colour)
        {
            foreach (Tile tile in tiles)
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = colour;
            }
        }

        public void ResetTileColours(Tile[] tiles)
        {
            foreach (Tile tile in tiles)
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = tile.DefaultColour;
            }
        }

        private void OnDestroy()
        {
            BoardManager.Instance.onBoardUpdate -= DetermineTilesInSphere;
            TurnManager.Instance.onPlayerCycle -= OnPlayerCycle;
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
            if (!isSelected)
            {
                ResetTileColours(availableTiles.ToArray());
                ResetTileColours(nearbyUnits);
            }

            else
            {
                ChangeTileColours(availableTiles.ToArray(), availableTileColour);
                ChangeTileColours(nearbyUnits, enemyTileColour);
            }
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

