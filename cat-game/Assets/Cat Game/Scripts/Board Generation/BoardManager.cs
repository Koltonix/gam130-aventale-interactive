using System;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Pathfinding;

namespace CatGame.Tiles
{
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
        private Vector2Int tileGap;

        public delegate void OnBoardUpdate(Tile[] boardTiles);
        public event OnBoardUpdate onBoardUpdate;

        private void Start()
        {
            CheckForDuplicates(GetAllChildrenFromParent(board.transform));
            tiles = GetBoardTiles();
        }

        /// <summary>
        /// Retreives all of the board tiles from a singular parent gameobject.
        /// </summary>
        /// <remarks>
        /// This is mainly a debugging tool since in the final build it is anticipated
        /// that we will be using the procedural generation which will have all of the
        /// tiles in one array already and therefore will not need this check.
        /// </remarks>
        /// <returns></returns>
        public Tile[] GetBoardTiles()
        {
            int amountOfTiles = board.transform.childCount;
            Tile[] boardTiles = new Tile[amountOfTiles];
            gridTiles = new Tile[boardWidth, boardHeight];

            for (int i = 0; i < amountOfTiles; i++)
            {
                GameObject tileChild = board.transform.GetChild(i).gameObject;
                Vector2Int tilePosition = GetTilePositionFromWorld(tileChild.transform.position);

                Tile newTile = new Tile(tileChild.transform.position, tileChild, tilePosition.x, tilePosition.y);
                boardTiles[i] = newTile;
                gridTiles[tilePosition.x, tilePosition.y] = newTile;
            }

            if (onBoardUpdate != null) onBoardUpdate.Invoke(boardTiles);
            return boardTiles;
        }

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
