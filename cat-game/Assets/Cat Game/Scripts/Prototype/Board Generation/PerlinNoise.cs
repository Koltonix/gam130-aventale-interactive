using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace SiegeOfAshes.Board
{
    /// <summary>
    /// A class that stores the data and function implementations for a perlin
    /// noise map.
    /// </summary>
    /// <remarks>
    /// It store all of the data on a texture over a multidimensional
    /// arrays since I can directly translate it over to an image for actual
    /// visual representation which allows for better debugging.
    /// </remarks>
    [Serializable]
    public class PerlinNoise
    {
        public int width;
        public int height;
        public Vector2 offset;
        public Vector2 scale;
        public Texture2D texture;

        /// <summary>
        /// Constructor for the Perlin Noise Data class.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="offset"></param>
        /// <param name="scale"></param>
        public PerlinNoise(int width, int height, Vector2 offset, Vector2 scale)
        {
            this.width = width;
            this.height = height;
            this.offset = offset;
            this.scale = scale;

            GenerateRandomNoise();
        }

        /// <summary>
        /// Assigns a perlin noise value to the texture datatype which acts
        /// like a multidimensional array in terms of assigning.
        /// </summary>
        /// <returns>
        /// Returns a texture with a completed noise map depending on the width
        /// and height provided by the constructor.
        /// </returns>
        public Texture2D GenerateRandomNoise()
        {
            texture = new Texture2D(width, height);
            offset = GetRandomOffset();

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    float noiseValue = GetRandomNoiseValue(x, y);
                    Color noiseColour = new Color(noiseValue, noiseValue, noiseValue, 1);
                    texture.SetPixel(x, y, noiseColour);
                }
            }

            texture.Apply();
            return texture;
        }

        /// <summary>
        /// Returns a random offset value to be used in the perlin noise generation
        /// to ensure that it is random each time it is played
        /// </summary>
        /// <remarks>
        /// Sourced by https://www.youtube.com/watch?v=bG0uEXV6aHQ
        /// </remarks>
        /// <returns>
        /// Returns a Vector2 which holds the X and Y offset values
        /// </returns>
        public Vector2 GetRandomOffset()
        {
            Vector2 offset = Vector2.zero;
            offset.x = Random.Range(0, 9999f);
            offset.y = Random.Range(0, 9999f);
            return offset;
        }

        /// <summary>
        /// A short functions that mirrors the current texture and as a result doubles
        /// the width of the whole image.
        /// </summary>
        /// <param name="texture"></param>
        /// <returns>
        /// Returns a texture of the mirrored image.
        /// </returns>
        public Texture2D MirrorImage(Texture2D texture)
        {
            Texture2D mirroredMap = new Texture2D(texture.width * 2, texture.height);

            for (int y = 0; y < texture.height; y++)
            {
                for (int x = 0; x < texture.width * 2; x++)
                {
                    if (texture.height <= x) mirroredMap.SetPixel(x, y, texture.GetPixel(x, y));
                    else mirroredMap.SetPixel(x, y, texture.GetPixel(texture.width - x - 1, y));
                }
            }

            mirroredMap.Apply();
            return mirroredMap;
        }

        /// <summary>
        /// A function that makes it so that it inverts the impassable and passable
        /// tiles depending on the condition of the board at a given time.
        /// </summary>
        public void BalanceMap()
        {
            if (HasMorePassableThanImpassable())
            {
                for (int x = 0; x < texture.width; x++)
                {
                    for (int y = 0; y < texture.height; y++)
                    {
                        Vector4 invertedColour = (Vector4.one - (Vector4)texture.GetPixel(x, y));
                        invertedColour.w = 1f;
                        texture.SetPixel(x, y, invertedColour);
                    }
                }
            }

            texture.Apply();
        }

        /// <summary>
        /// Checks to see if there are more passable tiles, or more impassable
        /// tiles.
        /// </summary>
        /// <returns>
        /// Returns true if there are more passable tiles than impassable tiles.
        /// </returns>
        public bool HasMorePassableThanImpassable()
        {
            int passableBlocks = 0;
            int unpassableBlocks = 0;

            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    if (texture.GetPixel(x, y).r < 0.5f) passableBlocks++;
                    else unpassableBlocks++;
                }
            }

            if (passableBlocks > unpassableBlocks) return true;
            return false;
        }

        /// <summary>
        /// Increases the contrast of the perlin noise texture to allow for
        /// not only easier debugging, but also to make the perlin noise more
        /// accurate to what the map will be like.
        /// </summary>
        /// <param name="increaseAmount"></param>
        public void IncreaseContrast(float increaseAmount)
        {
            for (int x = 0; x < texture.width; x++)
            {
                for (int y = 0; y < texture.height; y++)
                {
                    Vector4 pixelColour = texture.GetPixel(x, y);
                    if (pixelColour.x > .5f)
                    {
                        pixelColour += (Vector4.one * increaseAmount);
                        pixelColour.w = 1f;
                        texture.SetPixel(x, y, pixelColour);
                    }

                    else if (pixelColour.x <= .5f)
                    {
                        pixelColour -= (Vector4.one * increaseAmount);
                        pixelColour.w = 1f;
                        texture.SetPixel(x, y, pixelColour);
                    }
                }
            }

            texture.Apply();
        }

        /// <summary>
        /// Uses an X and Y determine a perlin noise value at a given coordinate 
        /// which I then later assign to the samples
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <remarks>
        /// Sourced by https://www.youtube.com/watch?v=bG0uEXV6aHQ
        /// </remarks>
        /// <returns>
        /// Returns a perlin noise value depending on the colour of the position
        /// of the coordinate
        /// </returns>
        public float GetRandomNoiseValue(int x, int y)
        {
            float xCoord = (float)x / width * scale.x + offset.x;
            float yCoord = (float)y / height * scale.y + offset.y;

            float perlinSample = Mathf.PerlinNoise(xCoord, yCoord);
            return perlinSample;
        }
    }
}

