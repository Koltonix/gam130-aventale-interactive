using UnityEngine;

namespace CatGame.Tiles
{
    public class BoardManager : MonoBehaviour
    {   
        #region Singleton
        public static BoardManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        #endregion

        [Header("Board Settings")]
        [SerializeField]
        private GameObject board;
        public Tile[] tiles;

        [SerializeField]
        private int boardWidth;
        [SerializeField]
        private int boardHeight;
        private Vector2 gridWorldSize;

        public delegate void OnBoardUpdate(Tile[] boardTiles);
        public event OnBoardUpdate onBoardUpdate;

        private void Start()
        {
            tiles = GetBoardTiles();
        }

        /// <summary>
        /// Retreives all of the board tiles from a singular parent gameobject.
        /// </summary>
        /// <remarks>
        /// This is mainly a debugging tool since in the final build it is anticipated
        /// that we will be using the procedural generation which will have all of the
        /// tiles in one array already and therefore will not need this check.
        /// </remarks>
        /// <returns></returns>
        public Tile[] GetBoardTiles()
        {
            int amountOfTiles = board.transform.childCount;
            Tile[] boardTiles = new Tile[amountOfTiles];

            for (int i = 0; i < amountOfTiles; i++)
            {
                GameObject tileChild = board.transform.GetChild(i).gameObject;
                Vector2Int tilePosition = GetTilePositionFromWorld(tileChild.transform.position);

                boardTiles[i] = new Tile(tileChild.transform.position, tileChild, tilePosition.x, tilePosition.y);
            }

            if (onBoardUpdate != null) onBoardUpdate.Invoke(boardTiles);
            return boardTiles;
        }
        
        public Vector2Int GetTilePositionFromWorld(Vector3 position)
        {
            gridWorldSize = new Vector2(boardWidth * 1, boardHeight * 1);

            float xPoint = ((position.x + gridWorldSize.x * .5f) / gridWorldSize.x);
            float yPoint = ((position.z + gridWorldSize.y * .5f) / gridWorldSize.y);

            xPoint = Mathf.Clamp01(xPoint);
            yPoint = Mathf.Clamp01(yPoint);

            int x = Mathf.RoundToInt((gridWorldSize.x - 1) * xPoint);
            int y = Mathf.RoundToInt((gridWorldSize.y - 1) * yPoint);

            return new Vector2Int(x, y);
        }

        public void CheckForDuplicates(Tile[] tiles)
        {
            for (int x = 1; x < tiles.Length; x++)
            {
                for (int y = 0; y < tiles.Length; y++)
                {
                    if (tiles[x].Position == tiles[y].Position && x != y)
                    {
                        Vector3 newPos = tiles[x].WorldReference.transform.position;
                        newPos.y += 1f;
                        tiles[x].WorldReference.transform.position = newPos;
                    }
                }  
            }
        }
    }
}
