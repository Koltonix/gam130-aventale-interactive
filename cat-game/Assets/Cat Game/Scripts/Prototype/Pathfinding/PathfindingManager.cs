using System.Collections.Generic;
using UnityEngine;
using CatGame.Controls;

namespace CatGame.Pathfinding
{
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
        private Color32 unpassableColour;
        private float drawHeight = 0.25f;

        private void Start()
        {
            boardData = boardGenerator.GetComponent<IGetBoardData>();
            clickData = userInput.GetComponent<IGetOnClick>();
        }

        private void Update()
        {
            if (clickData.IsMovementSelected() && clickData.GetRaycastHit().collider != null)
            {
                FindPath(new Vector3(start.position.x, boardData.GetBoardHeight(), start.position.z), 
                         new Vector3(clickData.GetRaycastHit().point.x, boardData.GetBoardHeight(), clickData.GetRaycastHit().point.z));

                DrawPath();
            }
        }

        /// <summary>
        /// Finds the closest path for the object to take from a start to and end point
        /// </summary>
        /// <param name="startPosition"></param>
        /// <param name="endPosition"></param>
        /// <remarks>
        /// Sourced from with alternations: https://youtu.be/AKKpPmxx07w
        /// </remarks>
        private void FindPath(Vector3 startPosition, Vector3 endPosition)
        {
            Tile startTile = boardData.GetTileFromWorldPosition(startPosition);
            Tile endTile = boardData.GetTileFromWorldPosition(endPosition);

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
                    GetFinalPath(startTile, endTile);
                    break;
                }

                foreach (Tile tile in boardData.GetNeighbouringTiles(currentTile))
                {
                    if (tile.isPassable || tile.isOccupied || closedList.Contains(tile))
                    {
                        continue;
                    }

                    int moveCost = currentTile.gCost + GetManhattenDistance(currentTile, tile);

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

        private void GetFinalPath(Tile startTile, Tile endTile)
        {
            List<Tile> finalPath = new List<Tile>();
            Tile currentTile = endTile;

            while (currentTile != startTile)
            {
                finalPath.Add(currentTile);
                currentTile = currentTile.parent;
            }

            finalPath.Reverse();
            finalPath.Add(endTile);

            this.finalPath = finalPath;
        }

        private int GetManhattenDistance(Tile firstTile, Tile secondTile)
        {
            int iX = Mathf.Abs(firstTile.boardX - secondTile.boardX);
            int iY = Mathf.Abs(firstTile.boardY - secondTile.boardY);

            return iX + iY;
        }

        private void DrawPath()
        {
            if (finalPath != null)
            {
                ResetTileColours();
                foreach (Tile tile in finalPath)
                {
                    tile.worldReference.GetComponent<Renderer>().material.color = Color.blue;
                }
            }
        }

        private void ResetTileColours()
        {
            foreach (Tile tile in boardData.GetTiles())
            {
                tile.worldReference.GetComponent<Renderer>().material.color = tile.defaultColour;
            }
        }

        private void OnDrawGizmos()
        {
            if (boardData != null && boardData.GetTiles() != null)
            {
                foreach (Tile tile in boardData.GetTiles())
                {
                    Vector3 spawnPosition = tile.worldReference.transform.position;
                    spawnPosition.y += drawHeight * .5f;

                    Gizmos.color = tile.isPassable ? unpassableColour : passableColour;

                    if (finalPath != null)
                    {
                        if (finalPath.Contains(tile))
                        {
                            Gizmos.color = Color.blue;
                        }
                    }

                    Gizmos.DrawWireCube(spawnPosition, new Vector3(1, 1 + drawHeight, 1));
                }
            }
            
        }
    }
}
