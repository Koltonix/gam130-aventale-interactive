using System.Collections.Generic;
using UnityEngine;

namespace SiegeOfAshes.Pathfinding
{
    public class GenerateDebugBoard : MonoBehaviour, IGetBoardData
    {

        #region Singleton
        public static GenerateDebugBoard Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        #endregion

        [Header("Board Settings")]
        private Tile[,] board;
        private List<Tile> debugBoard;

        [SerializeField]
        private int width;
        [SerializeField]
        private int height;
        [SerializeField]
        private Vector3 tileGap;
        private Vector3 gridWorldSize;

        [SerializeField]
        private Transform tileHolder;

        [Header("Tile Settings")]
        [SerializeField]
        private GameObject tilePrefab;

        public void GenerateBoard()
        {
            board = new Tile[width, height];
            debugBoard = new List<Tile>();

            gridWorldSize = new Vector3(width * tileGap.x, 0, height * tileGap.z);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector3 spawnPosition = new Vector3(x * tileGap.x - ((width - 1) * tileGap.x * .5f),
                                                        tileGap.y,
                                                        y * tileGap.z - ((height - 1) * tileGap.z * .5f));

                    GameObject worldTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                    worldTile.transform.parent = tileHolder;

                    bool isPassable = Random.Range(0, 2) == 1 ? true : false;
                    if (isPassable) worldTile.GetComponent<Renderer>().material.color *= .5f;

                    Tile tile = new Tile(x, y, worldTile, spawnPosition, isPassable);
                    board[x, y] = tile;
                    debugBoard.Add(tile);
                }
            }
        }

        public void DeleteBoard()
        {
            for (int i = tileHolder.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(tileHolder.GetChild(i).gameObject);
            }

            board = new Tile[width, height];
            debugBoard = new List<Tile>();
        }

        #region Contractual Obligations
        public Tile[,] GetTiles()
        {
            return board;
        }

        public int GetBoardWidth()
        {
            return width;
        }

        public int GetBoardHeight()
        {
            return height;
        }

        public Vector3 GetBoardCentre()
        {
            return this.transform.position;
        }

        public Tile GetTileFromWorldPosition(Vector3 position)
        {
            float xPoint = ((position.x + gridWorldSize.x * .5f) / gridWorldSize.x);
            float zPoint = ((position.z + gridWorldSize.z * .5f) / gridWorldSize.z);

            xPoint = Mathf.Clamp01(xPoint);
            zPoint = Mathf.Clamp01(zPoint);

            int x = Mathf.RoundToInt((gridWorldSize.x - 1) * xPoint);
            int y = Mathf.RoundToInt((gridWorldSize.y - 1) * zPoint);

            return board[x, y];
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
            if (xCheck >= 0 && xCheck < width)
            {
                if (yCheck >= 0 && yCheck < height)
                {
                    return board[xCheck, yCheck];
                }
            }

            return null;
        }

        #endregion
    }
}

