using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Pathfinding;

namespace CatGame.Board
{
    /// <summary>
    /// A class that deals with the spawning of the game board using
    /// a perlin noise data structure.
    /// </summary>
    public class BoardGeneration : MonoBehaviour, IGetBoardData
    {
        [Header("Perlin Noise Data")]
        public PerlinNoise currentNoiseData;

        [Header("Board Attributes")]
        private Tile[,] board;
        [SerializeField]
        private List<Tile> debugBoard;
        [HideInInspector]
        public GameObject boardHolder;
        [SerializeField]
        private Vector3 boardSpawnPosition;
        [SerializeField]
        private Vector3 tileGap;
        private Vector3 gridWorldSize;
        [Space]

        [Header("Tile Prefabs")]
        [SerializeField]
        private GameObject loweredTile;
        [SerializeField]
        private GameObject risenTile;

        private void Start()
        {
            CreateBoard();
        }

        /// <summary>
        /// A coroutine that deals with spawning the board as a whole and allows for the rows
        /// to be loaded individually to provide an aesthetically appealing effect when in 
        /// conjunction with the raising coroutine for the individual tiles.
        /// </summary>
        /// <param name="noiseData"></param>
        /// <returns>
        /// Intermittently spawns the rows of tiles every fixed frame rather than based on frame
        /// rate.
        /// </returns>
        public void GenerateBoard(PerlinNoise noiseData)
        {
            DestroyImmediate(boardHolder);

            board = new Tile[noiseData.width, noiseData.height];
            debugBoard = new List<Tile>();

            boardHolder = new GameObject("Board");
            gridWorldSize = new Vector3(currentNoiseData.width * tileGap.x, 0, currentNoiseData.height * tileGap.z);

            for (int x = 0; x < noiseData.width; x++)
            {
                for (int y = 0; y < noiseData.height; y++)
                {
                    Vector3 spawnPosition = new Vector3(x * tileGap.x - ((noiseData.width - 1) * tileGap.x * .5f),
                                                        tileGap.y,
                                                        y * tileGap.z - ((noiseData.height - 1) * tileGap.z * .5f));

                    boardHolder.transform.position = boardSpawnPosition;

                    float heightValue = noiseData.Texture.GetPixel(x, y).grayscale;
                    if (heightValue > .5f)
                    {
                        Tile tile = SpawnTile(x, y, loweredTile, spawnPosition, false);
                        board[x, y] = tile;
                        debugBoard.Add(tile);
                    }


                    else
                    {
                        Tile tile = SpawnTile(x, y, risenTile, spawnPosition, true);
                        board[x, y] = tile;
                        debugBoard.Add(tile);
                    }
                }
            }
        }

        /// <summary>
        /// Spawns the tile and automatically assigns it to the empty board parent object to
        /// ensure the scene is clean.
        /// </summary>
        /// <param name="worldReference"></param>
        /// <param name="spawnPosition"></param>
        public Tile SpawnTile(int x, int y, GameObject worldReference, Vector3 spawnPosition, bool isPassable)
        {
            GameObject clonedTile = Instantiate(worldReference, spawnPosition, Quaternion.identity);
            clonedTile.transform.SetParent(boardHolder.transform);

            Tile tile = new Tile(x, y, clonedTile, spawnPosition, isPassable);


            return tile;
        }

        #region Board Initialisers
        /// <summary>
        /// Generates a Perlin noise and assigns a raw image the texture of the perlin noise.
        /// </summary>
        public void CreatePerlinNoise()
        {
            currentNoiseData = new PerlinNoise(currentNoiseData.width, currentNoiseData.height, currentNoiseData.offset, currentNoiseData.scale);
        }

        /// <summary>
        /// Starts the spawning of the board coroutine since I need to keep a reference of 
        /// the coroutine in this script rather than the editor.
        /// </summary>
        /// <param name="noiseData"></param>
        public void CreateBoard()
        {
            CreatePerlinNoise();
            currentNoiseData.IncreaseContrast(.5f);

            currentNoiseData.BalanceMap();

            GenerateBoard(currentNoiseData);
        }

        public void DestroyBoard()
        {
            DestroyImmediate(boardHolder);

            board = new Tile[currentNoiseData.width, currentNoiseData.height];
            debugBoard = new List<Tile>();
        }

        #endregion

        #region Contractual Obligations

        public Tile[,] GetTiles()
        {
            return board;
        }

        public int GetBoardWidth()
        {
            return currentNoiseData.width;
        }

        public int GetBoardHeight()
        {
            return currentNoiseData.height;
        }

        public Vector3 GetBoardCentre()
        {
            return boardSpawnPosition;
        }

        public Tile GetTileFromWorldPosition(Vector3 position)
        {
            float xPoint = ((position.x + gridWorldSize.x * .5f) / gridWorldSize.x);
            float zPoint = ((position.z + gridWorldSize.z * .5f) / gridWorldSize.z);

            xPoint = Mathf.Clamp01(xPoint);
            zPoint = Mathf.Clamp01(zPoint);

            int x = Mathf.RoundToInt((gridWorldSize.x - 1) * xPoint);
            int y = Mathf.RoundToInt((gridWorldSize.z - 1) * zPoint);

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
            if (xCheck >= 0 && xCheck < currentNoiseData.width)
            {
                if (yCheck >= 0 && yCheck < currentNoiseData.height)
                {
                    return board[xCheck, yCheck];
                }
            }

            return null;
        }

        #endregion

        private void OnDestroy()
        {
            DestroyBoard();
        }
    }

}

