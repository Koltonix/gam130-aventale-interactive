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

        public Tile[] availableTiles;
        public Unit[] nearbyUnits;
        public Dictionary<Tile, List<Tile>> tilePaths;

        [Header("Required Data")]
        public IPlayerData owner;
        private IUnitData unitData;
        public Unit currentUnit;

        private ITurn turnData;
        private IPlayerManager globalPlayerData;

        private Player debugPlayer;

        private void Start()
        {
            owner = PlayerManager.Instance.GetCurrentPlayer();
            turnData = TurnManager.Instance;
            globalPlayerData = PlayerManager.Instance;

            unitData = this.GetComponent<IUnitData>();
            currentUnit = this.GetComponent<Unit>();

            debugPlayer = owner.GetPlayerReference();
            
            owner.GetPlayerReference().onActive += SetIsActive;

            BoardManager.Instance.onBoardUpdate += DetermineTilesInSphere;
            turnData.AddToListener += OnPlayerCycle;
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
            Vector3 unitPosition = this.transform.position;
            unitPosition.y = 0;

            List<Tile> accessibleTiles = new List<Tile>();
            List<Unit> accessibleUnits = new List<Unit>();

            foreach (Tile tile in allTiles)
            {
                tile.CheckForUnit();

                if (unitPosition == tile.Position) currentTile = tile;

                if (tile.IsPassable && tile.OccupiedUnit == null)
                {
                    float tileDistance = Vector3.Distance(new Vector3(tile.Position.x, 0, tile.Position.z), unitPosition);

                    tileDistance = Mathf.RoundToInt(tileDistance);
                    if (tileDistance <= owner.GetCurrentActionPoints() & tileDistance > 0)
                    {
                        accessibleTiles.Add(tile);
                    }                    
                }

                //Change this later to be integrated with the pathfinding
                if (tile.OccupiedUnit != null && tile.OccupiedUnit != currentUnit)
                {
                    accessibleUnits.Add(tile.OccupiedUnit);
                }
            }

            availableTiles = accessibleTiles.ToArray();
            nearbyUnits = accessibleUnits.ToArray();

            tilePaths = PathfindAvailableTiles(availableTiles);
        }

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
                allPaths.Add(endTile, finalPath);
            }

            return allPaths;
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

        public void ChangeAvailableTilesColour(Color32 color)
        {
            foreach (Tile tile in availableTiles)
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = color;
            }
        }

        public void ResetTileColours()
        {
            foreach (Tile tile in availableTiles)
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = tile.DefaultColour;
            }
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
            DetermineTilesInSphere(BoardManager.Instance.tiles);
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

