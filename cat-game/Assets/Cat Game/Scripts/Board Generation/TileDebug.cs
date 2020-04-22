using UnityEngine;

namespace CatGame.Tiles
{
    /// <summary>
    /// Used for Debugging the Tiles since it has a MonoBehaviour and therefore can
    /// be used to see the Board Position and World Reference quickly.
    /// </summary>
    public class TileDebug : MonoBehaviour
    {
        public int x;
        public int y;

        public GameObject worldReference;

        /// <summary>
        /// Tile Debug Constructor.
        /// </summary>
        /// <param name="x">X Board Position.</param>
        /// <param name="y">Y Board Position.</param>
        /// <param name="worldReference">World GameObject Reference.</param>
        public void TileDebugSetup(int x, int y, GameObject worldReference)
        {
            this.x = x;
            this.y = y;
            this.worldReference = worldReference;
        }
    }
}