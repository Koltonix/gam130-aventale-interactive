using System;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Pathfinding;

namespace CatGame.Tiles
{
    /// <summary>
    /// Deals with identifying a pre-built board using the width and height. It then deals with
    /// converting this into a multi-dimensional array and regular array to be used in the Pathfinding and Movement
    /// in the game.
    /// </summary>
    public class BoardManager : MonoBehaviour, IGetBoardData
    {   
        #region Singleton
        public static BoardManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        #endregion

        [Header("Board Settings")]
        [SerializeField]
        private GameObject board;
        public Tile[] tiles;
        public Tile[,] gridTiles;

        [SerializeField]
        private int boardWidth;
        [SerializeField]
        private int boardHeight;
        private Vector2 gridWorldSize;
        [SerializeField]
        public Vector2Int tileGap;
        private bool hasInitialised = false;

        [Header("Tile Settings")]
        [SerializeField]
        private GameObject passableTilePrefab;
        [SerializeField]
        private GameObject impassableTilePrefab;
        [SerializeField]
        private LayerMask impassableMask;

        //Used to inform subscribers of any changes to the board.
        public delegate void OnBoardUpdate(Tile[] boardTiles);
        public event OnBoardUpdate onBoardUpdate;

        private void Start()
        {
            GameObject[] allTiles = GetAllChildrenFromParent(board.transform);
            CheckForDuplicates(allTiles);
            
            tiles = GetBoardTiles();
        }

        /// <summary>
        /// Retreives all of the board tiles from a singular parent gameobject.
        /// </summary>
        /// <returns>An array of all of the Tiles in the board world.</returns>
        public Tile[] GetBoardTiles()
        {
            if (board == null) return null;

            //Initialisers
            int amountOfTiles = board.transform.childCount;
            Tile[] boardTiles = new Tile[amountOfTiles];
            gridTiles = new Tile[boardWidth, boardHeight];

            //Iterating through all of the board's child objects to make them into Tile classes
            for (int i = 0; i < amountOfTiles; i++)
            {
                GameObject tileChild = board.transform.GetChild(i).gameObject;

                //Tile Board Position
                Vector2Int tilePosition = GetTilePositionFromWorld(tileChild.transform.position);
                //Tile World Position
                Vector3 spawnPosition = tileChild.transform.position;

                //Replaces the current tiles placed down, but only once
                if (!hasInitialised)
                {
                    Destroy(tileChild);
                    tileChild = DetermineIfTileIsPassable(spawnPosition);
                }
                
                //Assigning the Tile data
                Tile newTile = new Tile(tileChild.transform.position, tileChild, tilePosition.x, tilePosition.y);
                boardTiles[i] = newTile;
                gridTiles[tilePosition.x, tilePosition.y] = newTile;

                //Debugging
                boardTiles[i].WorldReference.GetComponent<TileDebug>().TileDebugSetup(tilePosition.x, tilePosition.y, boardTiles[i].WorldReference);
            }

            if (onBoardUpdate != null) onBoardUpdate.Invoke(boardTiles);

            hasInitialised = true;
            return boardTiles;
        }

        /// <summary>
        /// Determines if a Tile being spawned should be passable or impassable
        /// based on if there is an object there or not.
        /// </summary>
        /// <param name="tilePosition">World Position.</param>
        /// <returns>A GameObject of the new Tile GameObject that has been setup.</returns>
        private GameObject DetermineIfTileIsPassable(Vector3 tilePosition)
        {
            Vector3 checkPosition = tilePosition;
            checkPosition.y += 1.0f;

            bool isPassable = !(Physics.CheckSphere(checkPosition, .75f, impassableMask));

            if (isPassable)
            {    
                GameObject tileReference = Instantiate(passableTilePrefab, tilePosition, Quaternion.identity);

                tileReference.layer = 9;
                tileReference.transform.localScale = new Vector3(tileGap.x * tileReference.transform.localScale.x,
                                                                 tileGap.y * tileReference.transform.localScale.y,
                                                                 tileGap.y * tileReference.transform.localScale.z);

                tileReference.transform.SetParent(board.transform);
                return tileReference;
            }

            else
            {
                GameObject tileReference = Instantiate(impassableTilePrefab, tilePosition, Quaternion.identity);

                tileReference.layer = 10;
                tileReference.transform.localScale = new Vector3(tileGap.x * tileReference.transform.localScale.x,
                                                                 tileGap.y * tileReference.transform.localScale.y,
                                                                 tileGap.y * tileReference.transform.localScale.z);

                tileReference.transform.SetParent(board.transform);
                return tileReference;
            }
        }

        /// <summary>
        /// Checks for Duplicate Tiles and moves them up.
        /// </summary>
        /// <param name="tiles">All of the Tiles in the world.</param>
        public void CheckForDuplicates(Tile[] tiles)
        {
            for (int x = 0; x < tiles.Length; x++)
            {
                for (int y = 0; y < tiles.Length; y++)
                {
                    if (tiles[x].Position == tiles[y].Position && x != y)
                    {
                        Vector3 newPos = tiles[x].WorldReference.transform.position;
                        newPos.y += 2f;
                        tiles[x].WorldReference.transform.position = newPos;
                    }
                }  
            }
        }

        /// <summary>
        /// Checks for duplicate tile gameobjects and moves them up.
        /// </summary>
        /// <param name="tiles">All of the Tile GameObjects in the world.</param>
        public void CheckForDuplicates(GameObject[] tiles)
        {
            Dictionary<Vector3, GameObject> nonDuplicateTiles = new Dictionary<Vector3, GameObject>();
            List<GameObject> duplicates = new List<GameObject>();

            for (int i = 0; i < tiles.Length; i++)
            {
                //If no duplicates exist
                if (!nonDuplicateTiles.ContainsKey(tiles[i].transform.position)) nonDuplicateTiles.Add(tiles[i].transform.position, tiles[i]);
                else
                {
                    Vector3 newPos = tiles[i].transform.position;
                    newPos.y += 2f;
                    tiles[i].transform.position = newPos;

                    duplicates.Add(tiles[i]);
                }
            }

            if (duplicates.Count > 0) throw new Exception("THERE ARE " + duplicates.Count + " DUPLICATE TILES. THEY HAVE BEEN MOVED UP BY 2 UNITS");
        }

        /// <summary>
        /// Gets all of the childen gameobjects in a parent.
        /// </summary>
        /// <remarks>
        /// This is not any kind of search like depth search and will only do one layer.
        /// </remarks>
        /// <param name="parent">Parent GameObject.</param>
        /// <returns>An array of children GameObjects.</returns>
        private GameObject[] GetAllChildrenFromParent(Transform parent)
        {
            GameObject[] childObjects = new GameObject[parent.childCount];
            for (int i = 0; i < childObjects.Length; i++)
            {
                childObjects[i] = parent.GetChild(i).gameObject;
            }

            return childObjects;
        }

        #region Contractual Obligations
        public Tile[,] GetTiles()
        {
            if (gridTiles != null) return gridTiles;
            return null;
        }

        public Vector2Int GetTilePositionFromWorld(Vector3 position)
        {
            gridWorldSize = new Vector2(boardWidth * tileGap.x, boardHeight * tileGap.y);

            float xPoint = ((position.x + gridWorldSize.x * .5f) / gridWorldSize.x) / tileGap.x;
            float yPoint = ((position.z + gridWorldSize.y * .5f) / gridWorldSize.y) / tileGap.y;

            xPoint = Mathf.Clamp01(xPoint);
            yPoint = Mathf.Clamp01(yPoint);

            int x = Mathf.RoundToInt((gridWorldSize.x - tileGap.x) * xPoint);
            int y = Mathf.RoundToInt((gridWorldSize.y - tileGap.y) * yPoint);

            return new Vector2Int(x, y);
        }

        public Tile GetTileFromWorldPosition(Vector3 position)
        {
            if (gridTiles != null)
            {
                gridWorldSize = new Vector2(boardWidth * tileGap.x, boardHeight * tileGap.y);

                float xPoint = ((position.x + gridWorldSize.x * .5f) / gridWorldSize.x) / tileGap.x;
                float yPoint = ((position.z + gridWorldSize.y * .5f) / gridWorldSize.y) / tileGap.y;

                xPoint = Mathf.Clamp01(xPoint);
                yPoint = Mathf.Clamp01(yPoint);

                int x = Mathf.RoundToInt((gridWorldSize.x - tileGap.x) * xPoint);
                int y = Mathf.RoundToInt((gridWorldSize.y - tileGap.y) * yPoint);

                return gridTiles[x, y];
            }

            return null;
        }

        public Tile[] GetNeighbouringTiles(Tile tile)
        {
            List<Tile> neighbouringTiles = new List<Tile>();

            Tile checkingTile;
            int xCheck;
            int yCheck;

            //Right hand check
            xCheck = tile.boardX + 1;
            yCheck = tile.boardY;

            checkingTile = CheckForNeighbour(xCheck, yCheck);
            if (checkingTile != null) neighbouringTiles.Add(checkingTile);


            //Left hand check
            xCheck = tile.boardX - 1;
            yCheck = tile.boardY;

            checkingTile = CheckForNeighbour(xCheck, yCheck);
            if (checkingTile != null) neighbouringTiles.Add(checkingTile);

            //Upper hand check
            xCheck = tile.boardX;
            yCheck = tile.boardY + 1;

            checkingTile = CheckForNeighbour(xCheck, yCheck);
            if (checkingTile != null) neighbouringTiles.Add(checkingTile);

            //Left hand check
            xCheck = tile.boardX;
            yCheck = tile.boardY - 1;

            checkingTile = CheckForNeighbour(xCheck, yCheck);
            if (checkingTile != null) neighbouringTiles.Add(checkingTile);

            return neighbouringTiles.ToArray();
        }

        private Tile CheckForNeighbour(int xCheck, int yCheck)
        {
            if (gridTiles != null)
            {
                if (xCheck >= 0 && xCheck < boardWidth)
                {
                    if (yCheck >= 0 && yCheck < boardHeight)
                    {
                        return gridTiles[xCheck, yCheck];
                    }
                }
            }

            return null;
        }

        public int GetBoardWidth()
        {
            return boardWidth;
        }

        public int GetBoardHeight()
        {
            return boardHeight;
        }

        public Vector3 GetBoardCentre()
        {
            return board.transform.position;
        }

        #endregion
    }
}
