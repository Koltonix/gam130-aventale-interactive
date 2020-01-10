using UnityEngine;

namespace SiegeOfAshes.Tiles
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

        public delegate void OnBoardUpdate(Tile[] boardTiles);
        public event OnBoardUpdate onBoardUpdate;

        private void Start()
        {
            tiles = GetBoardTiles();
        }

        public Tile[] GetBoardTiles()
        {
            int amountOfTiles = board.transform.childCount;
            Tile[] boardTiles = new Tile[amountOfTiles];

            for (int i = 0; i < amountOfTiles; i++)
            {
                GameObject tileChild = board.transform.GetChild(i).gameObject;
                boardTiles[i] = new Tile(tileChild.transform.position, tileChild);
            }

            if (onBoardUpdate != null) onBoardUpdate.Invoke(boardTiles);
            return boardTiles;
        }
    }
}
