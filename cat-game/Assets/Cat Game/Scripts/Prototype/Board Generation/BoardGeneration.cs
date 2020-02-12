using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SiegeOfAshes.Pathfinding;

namespace SiegeOfAshes.Board
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
        [Space]

        [Header("Tile Prefabs")]
        [SerializeField]
        private GameObject loweredTile;
        [SerializeField]
        private GameObject risenTile;

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

            for (int x = 0; x < noiseData.width; x++)
            {
                for (int z = 0; z < noiseData.height; z++)
                {
                    Vector3 spawnPosition = new Vector3(x * tileGap.x - ((noiseData.width - 1) * tileGap.x * .5f),
                                                        tileGap.y,
                                                        z * tileGap.z - ((noiseData.height - 1) * tileGap.z * .5f));

                    boardHolder.transform.position = boardSpawnPosition;

                    float heightValue = noiseData.Texture.GetPixel(x, z).grayscale;
                    if (heightValue > .5f)
                    {
                        Tile tile = SpawnTile(loweredTile, spawnPosition, false);
                        board[x, z] = tile;
                        debugBoard.Add(tile);
                    }


                    else
                    {
                        Tile tile = SpawnTile(risenTile, spawnPosition, true);
                        board[x, z] = tile;
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
        public Tile SpawnTile(GameObject worldReference, Vector3 spawnPosition, bool isPassable)
        {
            GameObject clonedTile = Instantiate(worldReference, spawnPosition, Quaternion.identity);
            clonedTile.transform.SetParent(boardHolder.transform);

            Tile tile = new Tile(clonedTile, spawnPosition, isPassable);


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

        #endregion
    }

}

