using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Pathfinding;
using CatGame.Tiles;

namespace CatGame.Board
{
    /// <summary>
    /// Produces a randomly generated board in the world using a Perlin Noise class 
    /// as the data container.
    /// Implements 
    /// </summary
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
        /// Generates the board in the world.
        /// </summary>
        /// <param name="noiseData">Perlin Noise Data.</param>
        public void GenerateBoard(PerlinNoise noiseData)
        {
            //Removes the previous instance of the board.
            DestroyImmediate(boardHolder);

            board = new Tile[noiseData.width, noiseData.height];
            debugBoard = new List<Tile>();

            boardHolder = new GameObject("Board");
            gridWorldSize = new Vector3(currentNoiseData.width * tileGap.x, 0, currentNoiseData.height * tileGap.z);

            //Going through the total width and height of the board.
            for (int x = 0; x < noiseData.width; x++)
            {
                for (int y = 0; y < noiseData.height; y++)
                {
                    //Using some math to centre the board rather than it being in a corner.
                    Vector3 spawnPosition = new Vector3(x * tileGap.x - ((noiseData.width - 1) * tileGap.x * .5f),
                                                        tileGap.y,
                                                        y * tileGap.z - ((noiseData.height - 1) * tileGap.z * .5f));

                    boardHolder.transform.position = boardSpawnPosition;

                    //Determining if the space is white or black which determines if it is passable or not.
                    float heightValue = noiseData.Texture.GetPixel(x, y).grayscale;
                    //It  is white and therefore passable.
                    if (heightValue > .5f)
                    {
                        Tile tile = SpawnTile(x, y, loweredTile, spawnPosition, false);
                        board[x, y] = tile;
                        debugBoard.Add(tile);
                    }

                    //It  is black and therefore impassable.
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
        /// Spawns a Tile Clone in the world.
        /// </summary>
        /// <param name="x">X Board Position.</param>
        /// <param name="y">Y Board Position.</param>
        /// <param name="prefab">Prefab of the Tile.</param>
        /// <param name="spawnPosition">World Position to spawn the Tile Prefab.</param>
        /// <param name="isPassable">Whether the tile is passable, or impassable.</param>
        /// <returns></returns>
        public Tile SpawnTile(int x, int y, GameObject prefab, Vector3 spawnPosition, bool isPassable)
        {
            GameObject clonedTile = Instantiate(prefab, spawnPosition, Quaternion.identity);
            clonedTile.transform.SetParent(boardHolder.transform);

            Tile tile = new Tile(spawnPosition, clonedTile, x, y);


            return tile;
        }

        #region Board Initialisers
        /// <summary>
        /// Generates a new Perlin Noise.
        /// </summary>
        public void CreatePerlinNoise()
        {
            currentNoiseData = new PerlinNoise(currentNoiseData.width, currentNoiseData.height, currentNoiseData.offset, currentNoiseData.scale);
        }

        /// <summary>
        /// Creates the Perlin Noise Data and then creates the Board.
        /// </summary>
        public void CreateBoard()
        {
            CreatePerlinNoise();
            currentNoiseData.IncreaseContrast(.5f);

            currentNoiseData.BalanceMap();

            GenerateBoard(currentNoiseData);
        }

        /// <summary>
        /// Destroys the board.
        /// </summary>
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
    }

}

