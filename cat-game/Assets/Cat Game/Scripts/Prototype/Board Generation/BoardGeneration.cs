using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SiegeOfAshes.Pathfinding;

namespace SiegeOfAshes.Board
{
    /// <summary>
    /// A class that deals with the spawning of the game board using
    /// a perlin noise data structure.
    /// </summary>
    public class BoardGeneration : MonoBehaviour
    {
        public PerlinNoise currentNoiseData;

        [SerializeField]
        private RawImage noiseMapImage;
        public RawImage NoiseMapImage
        {
            get { return noiseMapImage; }
            set
            {
                noiseMapImage = value;
                noiseMapImage.SetNativeSize();
                noiseMapImage.transform.localScale *= 10f;
            }
        }

        [Header("Board Attributes")]
        [HideInInspector]
        public GameObject tileParent;
        [SerializeField]
        private Vector3 boardStartPosition;
        [SerializeField]
        private Vector3 tileOffset;
        [SerializeField]
        private GameObject loweredTile;
        [SerializeField]
        private GameObject risenTile;
        public Coroutine generatingBoard;

        [Header("Board Animation Attributes")]
        [SerializeField]
        private float raiseSpeed = 0.125f;
        [SerializeField]
        private float raiseDistance = 10f;
        [SerializeField]
        private float spawnHeight = -10f;

        /// <summary>
        /// Generates a Perlin noise and assigns a raw image the texture of the perlin noise.
        /// </summary>
        public void CreatePerlinNoise()
        {
            currentNoiseData = new PerlinNoise(currentNoiseData.width, currentNoiseData.height, currentNoiseData.offset, currentNoiseData.scale);
            if (NoiseMapImage != null) NoiseMapImage.texture = currentNoiseData.texture;
        }

        /// <summary>
        /// Starts the spawning of the board coroutine since I need to keep a reference of 
        /// the coroutine in this script rather than the editor.
        /// </summary>
        /// <param name="noiseData"></param>
        public void CreateBoard(PerlinNoise noiseData)
        {
            if (generatingBoard != null) StopCoroutine(generatingBoard);
            generatingBoard = StartCoroutine(GenerateBoard(noiseData));
        }

        /// <summary>
        /// A debug feature that stops the spawning coroutine since coroutines are not able to
        /// be run during editor runtime without specific implementation.
        /// </summary>
        /// <remarks>
        /// I did attempt to use [ExecuteInEditMode], but unfortunately this would only update
        /// when you interacting with the editor.
        /// </remarks>
        public void StopBoardSpawning()
        {
            if (generatingBoard != null) StopCoroutine(generatingBoard);
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
        public IEnumerator GenerateBoard(PerlinNoise noiseData)
        {
            DestroyImmediate(tileParent);
            tileParent = new GameObject("Board");

            for (int x = 0; x < noiseData.texture.width; x++)
            {
                for (int z = 0; z < noiseData.texture.height; z++)
                {
                    Vector3 spawnPosition = new Vector3((boardStartPosition.x + x) * tileOffset.x, spawnHeight, (boardStartPosition.z + z) * tileOffset.z);

                    float heightValue = noiseData.texture.GetPixel(x, z).grayscale;
                    if (heightValue > .5f) SpawnTile(loweredTile, spawnPosition);

                    else SpawnTile(risenTile, spawnPosition);
                }

                yield return new WaitForFixedUpdate();
            }

            yield return null;
        }

        /// <summary>
        /// Spawns the tile and automatically assigns it to the empty board parent object to
        /// ensure the scene is clean.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="spawnPosition"></param>
        public void SpawnTile(GameObject tile, Vector3 spawnPosition)
        {
            GameObject clonedTile = Instantiate(tile, spawnPosition, Quaternion.identity);
            clonedTile.transform.SetParent(tileParent.transform);

            StartCoroutine(ElevateTile(clonedTile.transform, raiseDistance, raiseSpeed));
        }

        /// <summary>
        /// Moves the tile from its spawn position to a provided height.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="raiseAmount"></param>
        /// <param name="moveSpeed"></param>
        /// <remarks>
        /// I did intend to use Unity's animations, but animating positions becomes rather a
        /// bit awkward using Unity's animator, so I opted for this coroutin method instead.
        /// </remarks>
        /// <returns>
        /// Moves the tile every frame and returns until the next frame has passed.
        /// </returns>
        public IEnumerator ElevateTile(Transform tile, float raiseAmount, float moveSpeed)
        {
            Vector3 targetPosition = tile.position;
            targetPosition.y += raiseAmount;

            float t = 0;
            while (t < 1 && tile != null)
            {
                tile.position = Vector3.Lerp(tile.position, targetPosition, t);
                t += Time.deltaTime * moveSpeed;
                yield return new WaitForFixedUpdate();
            }
        }
    }

}

