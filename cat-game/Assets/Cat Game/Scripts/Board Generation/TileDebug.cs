using UnityEngine;

namespace CatGame.Tiles
{
    public class TileDebug : MonoBehaviour
    {
        public int x;
        public int y;

        public GameObject worldReference;

        public void TileDebugSetup(int x, int y, GameObject worldReference)
        {
            this.x = x;
            this.y = y;
            this.worldReference = worldReference;
        }
    }
}