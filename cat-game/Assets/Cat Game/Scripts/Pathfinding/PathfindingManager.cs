using System.Collections.Generic;
using UnityEngine;
using CatGame.Controls;
using CatGame.Tiles;

namespace CatGame.Pathfinding
{
    /// <summary>
    /// Pathfinds in a Grid Format using the A* Algorithm.
    /// Reference Video: https://youtu.be/AKKpPmxx07w
    /// </summary>
    public class PathfindingManager : MonoBehaviour
    {
        #region Singleton
        public static PathfindingManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(Instance);
        }
        #endregion

        [Header("Pathfinding Values")]
        private List<Tile> finalPath;
        [SerializeField]
        private Transform start;
        [SerializeField]
        private Transform end;

        [Header("Interface Initialisers")]
        [SerializeField]
        private GameObject boardGenerator;
        private IGetBoardData boardData;
        [SerializeField]
        private GameObject userInput;
        private IGetOnClick clickData;

        [Header("Debug Colour Settings")]
        [SerializeField]
        private Color32 passableColour;
        [SerializeField]
        private Color32 impassableColour;
        private float drawHeight = 0.25f;

        private void Start()
        {
            boardData = boardGenerator.GetComponent<IGetBoardData>();
            clickData = userInput.GetComponent<IGetOnClick>();
        }

        /// <summary>Gets the Tile Path using the Pathfinding Algorithm.</summary>
        /// <param name="startPosition">Start Tile World Positon.</param>
        /// <param name="endPosition">End Tile World Position.</param>
        /// <returns>A list of the Tiles of the Path.</returns>
        public List<Tile> GetPath(Vector3 startPosition, Vector3 endPosition, bool checkForUnit, Tile tileToIgnore)
        {
            FindPath(new Vector3(startPosition.x, boardData.GetBoardCentre().y, startPosition.z),
                         new Vector3(endPosition.x, boardData.GetBoardCentre().y, endPosition.z), checkForUnit, tileToIgnore);

            return finalPath;
        }

        /// <summary>Finds the closest path for the object to take from a start to and end point.</summary>
        /// <param name="startPosition">Start Tile World Position.</param>
        /// <param name="endPosition">End Tile World Position.</param>
        private void FindPath(Vector3 startPosition, Vector3 endPosition, bool checkForUnit, Tile tileToIgnore)
        {
            //Gets the Start Tile and End Tile.
            Tile startTile = boardData.GetTileFromWorldPosition(startPosition);
            Tile endTile = boardData.GetTileFromWorldPosition(endPosition);
            if (tileToIgnore != null) endTile = tileToIgnore;

            List<Tile> openList = new List<Tile>();
            HashSet<Tile> closedList = new HashSet<Tile>();

            openList.Add(startTile);

            while (openList.Count > 0)
            {
                Tile currentTile = openList[0];
                for (int i = 1; i < openList.Count; i++)
                {
                    if (openList[i].FCost < currentTile.FCost || openList[i].FCost == currentTile.FCost && openList[i].hCost < currentTile.hCost)
                    {
                        currentTile = openList[i];
                    }
                }

                openList.Remove(currentTile);
                closedList.Add(currentTile);

                if (currentTile == endTile)
                {
                    GetFinalPath(startTile, endTile, tileToIgnore);
                    break;
                }

                //Checks each Neightbouring tile to see if it is passable, or not
                foreach (Tile tile in boardData.GetNeighbouringTiles(currentTile))
                {
                    if (!tile.IsPassable || (tile.OccupiedEntity != null && checkForUnit) || closedList.Contains(tile))
                    {
                        if (tile != tileToIgnore) continue;
                    }

                    //Gets the cost of moving to the next tile
                    int moveCost = currentTile.gCost + GetManhattenDistance(currentTile, tile);

                    //Sets the new costs of the tile
                    if (!openList.Contains(tile) || moveCost < tile.FCost)
                    {
                        tile.gCost = moveCost;
                        tile.hCost = GetManhattenDistance(tile, endTile);
                        tile.parent = currentTile;

                        if (!openList.Contains(tile))
                        {
                            openList.Add(tile);
                        }
                    }
                }
            }
        }

        /// <summary>Gets the final path using the start Tile and end Tile.</summary>
        /// <param name="startTile">Starting Tile in the Path.</param>
        /// <param name="endTile">End Tile in the Path.</param>
        private void GetFinalPath(Tile startTile, Tile endTile, Tile tileToIgnore)
        {
            List<Tile> finalPath = new List<Tile>();
            Tile currentTile = endTile;

            while (currentTile != startTile)
            {
                finalPath.Add(currentTile);
                currentTile = currentTile.parent;
            }

            finalPath.Reverse();
            if (tileToIgnore == null) finalPath.Add(endTile);

            this.finalPath = finalPath;
        }

        /// <summary>
        /// Returns the Distance of the Tiles in terms of the boat, not the
        /// World Position distance.
        /// </summary>
        /// <param name="firstTile">First Tile in the Path.</param>
        /// <param name="secondTile">Last Tile in the Path.</param>
        /// <returns></returns>
        private int GetManhattenDistance(Tile firstTile, Tile secondTile)
        {
            int iX = Mathf.Abs(firstTile.boardX - secondTile.boardX);
            int iY = Mathf.Abs(firstTile.boardY - secondTile.boardY);

            return iX + iY;
        }

        /// <summary>
        /// Renders the Tile Path for debugging. This is obsolete and is
        /// now done in the UnitMovement script.
        /// </summary>
        private void DrawPath()
        {
            if (finalPath != null)
            {
                ResetTileColours();
                foreach (Tile tile in finalPath)
                {
                    tile.WorldReference.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
        }

        /// <summary>
        /// Resets the Tile colours of all of the board. This is obsolete and
        /// is now done in the UnitMovement script.
        /// </summary>
        private void ResetTileColours()
        {
            foreach (Tile tile in boardData.GetTiles())
            {
                tile.WorldReference.GetComponent<Renderer>().material.color = tile.DefaultColour;
            }
        }

        private void OnDrawGizmos()
        {
            if (boardData != null && boardData.GetTiles() != null)
            {
                //Renders all of the cubes in the Pathfinding as wireframe of either
                //red or green depending on if they are impassable, or not.
                foreach (Tile tile in boardData.GetTiles())
                {
                    if (tile != null)
                    {
                        Vector3 spawnPosition = tile.WorldReference.transform.position;
                        spawnPosition.y += drawHeight * .5f;

                        Vector3 tileScale = tile.WorldReference.transform.localScale;

                        Gizmos.color = tile.IsPassable ? passableColour : impassableColour;
                        Gizmos.DrawWireCube(spawnPosition, new Vector3(tileScale.x, tileScale.y + drawHeight, tileScale.z));
                    }
                    
                }
            }
            
        }
    }
}
