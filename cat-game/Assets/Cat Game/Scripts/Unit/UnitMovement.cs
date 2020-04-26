using System.Collections.Generic;
using UnityEngine;
using CatGame.Tiles;
using CatGame.Data;
using CatGame.Pathfinding;
using CatGame.Combat;

namespace CatGame.Units
{
    /// <summary>
    /// Handles the calculation of tiles available by using the Pathfinding.
    /// It also handles the colouring of the Tiles depending on their state.
    /// </summary>
    public class UnitMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float apCostModifier = 1.0f;

        [HideInInspector]
        public Tile currentTile;

        [HideInInspector]
        public List<Tile> availableTiles;
        [HideInInspector]
        public List<Tile> nearbyEnemyUnits;
        [HideInInspector]
        public Tile[] nearbyFriendlyUnits;
        [HideInInspector]
        public Dictionary<Tile, List<Tile>> tilePaths;
        public Dictionary<Tile, List<Tile>> pathsToEnemy;

        [Header("Required Data")]
        public Player owner;
        public Unit currentUnit;

        [Header("Movement Colour")]
        public Color32 availableTileColour;
        public Color32 selectedTileColour;
        public Color32 enemyTileColour;
        public Color32 friendlyTileColour;
        public Color32 pathColour;

        private void Start()
        {
            currentUnit = this.GetComponent<Unit>();
            owner = currentUnit.GetOwner();

            owner.GetPlayerReference().onActive += SetIsActive;

            BoardManager.Instance.onBoardUpdate += DetermineTilesInSphere;
            TurnManager.Instance.onPlayerCycle += OnPlayerCycle;
        }

        #region Tile Prediction

        /// <summary>Determines all of the total tiles within a radius of the maximum AP.</summary>
        /// <param name="allTiles">All of the Tiles in the Scene.</param>
        public void DetermineTilesInSphere(Tile[] allTiles)
        {
            List<Tile> accessibleTiles = new List<Tile>();

            List<Tile> friendlyUnits = new List<Tile>();
            List<Tile> enemyUnits = new List<Tile>();

            currentTile = BoardManager.Instance.GetTileFromWorldPosition(this.transform.position);

            foreach (Tile tile in allTiles)
            {
                float xBoardDistance = Mathf.Abs(tile.boardX - currentTile.boardX);
                float yBoardDistance = Mathf.Abs(tile.boardY - currentTile.boardY);

                //Making each Tile do a check to see if there is a Unit above it.
                tile.CheckForEntity();
                tile.isUsedInPathfinding = false;

                //No Unit is occupying it and it is passable
                if (tile.IsPassable && tile.OccupiedEntity == null)
                {
                    //Within the AP Distance
                    if (xBoardDistance <= owner.GetCurrentActionPoints() / apCostModifier)
                    {
                        if (yBoardDistance <= owner.GetCurrentActionPoints() / apCostModifier)
                        {
                            accessibleTiles.Add(tile);
                        }
                    }                 
                }

                //There is a Unit occupying it that is not itself
                else if (tile.OccupiedEntity != null && tile.OccupiedEntity != currentUnit)
                {
                    Attacker unitAttack = this.GetComponent<Attacker>();
                    if (tile.OccupiedEntity.owner == owner) friendlyUnits.Add(tile);
                    else
                    {
                        //Will only add the Unit if it is within the move distance and can also attack.
                        if (xBoardDistance <= unitAttack.AttackRange + owner.GetPlayerReference().ActionPoints - unitAttack.AttackAP)
                        {
                            if (yBoardDistance <= unitAttack.AttackRange + owner.GetPlayerReference().ActionPoints - unitAttack.AttackAP) enemyUnits.Add(tile);
                        }
                    }
                }
            }

            availableTiles = accessibleTiles;
            nearbyFriendlyUnits = friendlyUnits.ToArray();
            nearbyEnemyUnits = enemyUnits;

            tilePaths = PathfindAvailableTiles(accessibleTiles.ToArray());

            RemoveUnusedTiles();
        }

        /// <summary>Gets the A* path to the End Tile.</summary>
        /// <param name="endTile">The Tile which you wish to get the path for.</param>
        /// <returns>All of the Tiles in the path in order.</returns>
        public Tile[] GetAvailableTilesFromPathfinding(Tile endTile)
        {
            //Sanity Checks
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

        /// <summary>
        /// Stores all of the possible paths that can be taken and is accessed using
        /// the final Tile.
        /// </summary>
        /// <param name="nearbyTiles">All of the Tiles within the AP distance.</param>
        /// <returns></returns>
        public Dictionary<Tile, List<Tile>> PathfindAvailableTiles(Tile[] nearbyTiles)
        {
            //Stores the pathfinding for every tile available using the final Tile as the unique identifier
            Dictionary<Tile, List<Tile>> allPaths = new Dictionary<Tile, List<Tile>>();

            foreach (Tile endTile in nearbyTiles)
            {
                List<Tile> finalPath = PathfindingManager.Instance.GetPath(currentTile.Position, endTile.Position, true, null);
                if (finalPath == null) continue;
                //If there are only x tiles or less in the path
                if (finalPath.Count - 1 <= owner.GetCurrentActionPoints() / apCostModifier)
                {
                    allPaths.Add(endTile, finalPath);
                    SetTilesUsingPathfinding(finalPath.ToArray(), true);
                }
            }

            return allPaths;
        }
        
        /// <summary>Sets the Tile to state that the Pathfinding is being used by it.</summary>
        /// <param name="tiles"></param>
        /// <param name="areUsed"></param>
        /// <remarks>
        /// The reason for this is that it is more efficient to check if it is possible
        /// to reach this Tile within the actual AP Distance rather than a sphere cast.
        /// </remarks>
        private void SetTilesUsingPathfinding(Tile[] tiles, bool areUsed)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                tiles[i].isUsedInPathfinding = areUsed;
            }
        }

        /// <summary>Removes the Tiles not used in the pathfinding and there aren't accessible</summary>
        private void RemoveUnusedTiles()
        {
            pathsToEnemy = new Dictionary<Tile, List<Tile>>();
            Attacker unitAttack = this.GetComponent<Attacker>();

            for (int i = availableTiles.Count - 1; i >= 0; i--)
            {
                if (!availableTiles[i].isUsedInPathfinding) availableTiles.RemoveAt(i);
            }

            for (int i = nearbyEnemyUnits.Count - 1; i >= 0; i--)
            {
                List<Tile> enemyPath = new List<Tile>(PathfindingManager.Instance.GetPath(currentTile.Position, nearbyEnemyUnits[i].Position, true, nearbyEnemyUnits[i]));
                //Size reduced by two to negate the end tile and also the diagonal factor
                int enemyPathDistance = enemyPath.Count - 2;

                if (enemyPathDistance >= unitAttack.AttackRange + owner.GetPlayerReference().ActionPoints - unitAttack.AttackAP)
                {
                    nearbyEnemyUnits.RemoveAt(i);
                }
                
                //Will be used in pathfinding
                else
                {
                    GetShortestAdjacentEnemyPaths(nearbyEnemyUnits[i]);
                }
            }
        }

        public void GetShortestAdjacentEnemyPaths(Tile enemyTile)
        {
           Tile[] currentPath;
           Tile[] adjacentTiles = BoardManager.Instance.GetAllAdjacentTiles(enemyTile);

            int shortestLength = 2147483647;

            foreach (Tile tile in adjacentTiles)
            {
                currentPath = PathfindingManager.Instance.GetPath(currentTile.Position, tile.Position, true, null).ToArray();

                if (currentPath.Length < shortestLength)
                {
                    shortestLength = currentPath.Length;
                    if (pathsToEnemy.ContainsKey(enemyTile)) pathsToEnemy.Remove(enemyTile);
                    pathsToEnemy.Add(enemyTile, new List<Tile>(currentPath));  
                }
            }
        }

        /// <summary>
        /// A check to see if the tile that has been provided is a 
        /// tile that is currently available to move to.
        /// </summary>
        /// <param name="tileToMoveTo">The tile that is to be checked against all of the available tiles.</param>
        /// <returns>Returns true if the tile provided is a tile that is available to move to.</returns>
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

        /// <summary>Changes the Tile Rendering Colour.</summary>
        /// <param name="tiles">Tiles to change.</param>
        /// <param name="colour">Colour to change the Tiles.</param>
        public void ChangeTileColours(Tile[] tiles, Color32 colour)
        {
            foreach (Tile tile in tiles)
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = colour;
            }
        }

        /// <summary>Resets the Tiles Renderer to their default colour</summary>
        /// <param name="tiles">Tiles to change.</param>
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
        /// <summary>Used as a listener to invoke other functions.</summary>
        /// <param name="isSelected">Determines whether the unit has been selected or deselected.</param>
        public void SelectionListener(bool isSelected)
        {
            if (!isSelected)
            {
                ResetTileColours(availableTiles.ToArray());
                ResetTileColours(nearbyEnemyUnits.ToArray());
                ResetTileColours(nearbyFriendlyUnits);
            }

            else
            {
                ChangeTileColours(availableTiles.ToArray(), availableTileColour);
                ChangeTileColours(nearbyEnemyUnits.ToArray(), enemyTileColour);
                ChangeTileColours(nearbyFriendlyUnits, friendlyTileColour);
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

