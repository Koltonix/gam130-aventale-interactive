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

        [SerializeField]
        private Transform tileHolder;

        [Header("Tile Settings")]
        [SerializeField]
        private GameObject tilePrefab;

        public void GenerateBoard()
        {
            board = new Tile[width, height];
            debugBoard = new List<Tile>();

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    Vector3 spawnPosition = new Vector3(x * tileGap.x - ((width - 1) * tileGap.x * .5f),
                                                        tileGap.y,
                                                        z * tileGap.z - ((height - 1) * tileGap.z * .5f));

                    GameObject worldTile = Instantiate(tilePrefab, spawnPosition, Quaternion.identity);
                    worldTile.transform.parent = tileHolder;

                    Tile tile = new Tile(worldTile, spawnPosition, false);
                    board[x, z] = tile;
                    debugBoard.Add(tile);
                }
            }
        }

        public void DeleteBoard()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    DestroyImmediate(board[x, y].WorldReference);
                }
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
        #endregion
    }
}

