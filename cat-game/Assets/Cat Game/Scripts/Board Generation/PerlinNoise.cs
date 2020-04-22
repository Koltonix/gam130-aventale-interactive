using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CatGame.Board
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

        private Texture2D texture;
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
                width = texture.width;
                height = texture.height;

                debugTexture = texture;
            }
        }
        public Texture2D debugTexture;

        /// <summary>
        /// Constructor for the Perlin Noise Data class.
        /// </summary>
        /// <param name="width">Width of the Noise.</param>
        /// <param name="height">Height of the Noise.</param>
        /// <param name="offset">Offset of the Noise Randomiser</param>
        /// <param name="scale">Scale of the Noise</param>
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
        /// <returns>A Texture2D Noise Map Image.</returns>
        public Texture2D GenerateRandomNoise()
        {
            Texture = new Texture2D(width, height);
            offset = GetRandomOffset();

            //Iterating through each pixel
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //Assigning the noise value
                    float noiseValue = GetRandomNoiseValue(x, y);
                    Color noiseColour = new Color(noiseValue, noiseValue, noiseValue, 1);
                    Texture.SetPixel(x, y, noiseColour);
                }
            }

            Texture.Apply();
            return Texture;
        }

        /// <summary>
        /// Returns a random offset value to be used in the perlin noise generation
        /// to ensure that it is random each time it is played
        /// </summary>
        /// <remarks>Sourced by: https://www.youtube.com/watch?v=bG0uEXV6aHQ </remarks>
        /// <returns>Returns a Vector2 which holds the X and Y offset values.</returns>
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
        /// <param name="texture">The current Noise Map Image.</param>
        /// <returns>Returns a texture of the mirrored image.</returns>
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
            //Only will balance if there is a greater proportion of impassable tiles than passable
            if (!HasMorePassableThanImpassable())
            {
                for (int x = 0; x < Texture.width; x++)
                {
                    for (int y = 0; y < Texture.height; y++)
                    {
                        //Inverts the pixel colour
                        Vector4 invertedColour = (Vector4.one - (Vector4)Texture.GetPixel(x, y));
                        invertedColour.w = 1f;
                        Texture.SetPixel(x, y, invertedColour);
                    }
                }
            }

            Texture.Apply();
        }

        /// <summary>
        /// Checks to see if there are more passable tiles, or more impassable
        /// tiles.
        /// </summary>
        /// <returns>Returns true if there are more passable tiles than impassable tiles.</returns>
        public bool HasMorePassableThanImpassable()
        {
            int passableBlocks = 0;
            int unpassableBlocks = 0;

            for (int x = 0; x < Texture.width; x++)
            {
                for (int y = 0; y < Texture.height; y++)
                {
                    if (Texture.GetPixel(x, y).r < 0.5f) passableBlocks++;
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
        /// <param name="increaseAmount">Contrast increase amount.</param>
        public void IncreaseContrast(float increaseAmount)
        {
            for (int x = 0; x < Texture.width; x++)
            {
                for (int y = 0; y < Texture.height; y++)
                {
                    Vector4 pixelColour = Texture.GetPixel(x, y);
                    if (pixelColour.x > .5f)
                    {
                        pixelColour += (Vector4.one * increaseAmount);
                        pixelColour.w = 1f;
                        Texture.SetPixel(x, y, pixelColour);
                    }

                    else if (pixelColour.x <= .5f)
                    {
                        pixelColour -= (Vector4.one * increaseAmount);
                        pixelColour.w = 1f;
                        Texture.SetPixel(x, y, pixelColour);
                    }
                }
            }

            Texture.Apply();
        }

        /// <summary>
        /// Uses an X and Y determine a perlin noise value at a given coordinate 
        /// which I then later assign to the samples.
        /// </summary>
        /// <remarks>
        /// Sourced by: https://www.youtube.com/watch?v=bG0uEXV6aHQ </remarks>
        /// <param name="x">X Position of the Pixel</param>
        /// <param name="y">Y Position of the Pixel</param>
        /// <returns>Returns a perlin noise pixel value based on the coordinate and offset.</returns>
        public float GetRandomNoiseValue(int x, int y)
        {
            float xCoord = (float)x / width * scale.x + offset.x;
            float yCoord = (float)y / height * scale.y + offset.y;

            float perlinSample = Mathf.PerlinNoise(xCoord, yCoord);
            return perlinSample;
        }
    }
}

