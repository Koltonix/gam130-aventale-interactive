using UnityEngine;
using SiegeOfAshes.Data;

namespace SiegeOfAshes.Pathfinding
{
    public class PathfindingManager : MonoBehaviour
    {
        #region Singleton
        public static PathfindingManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(Instance);
        }
        #endregion

        [SerializeField]
        private GameObject boardGenerator;
        private IGetBoardData boardData;

        [Header("Debug Colour Settings")]
        [SerializeField]
        private Color32 passableColour;
        [SerializeField]
        private Color32 unpassableColour;
        private float drawHeight = 0.25f;

        private void Start()
        {
            boardData = boardGenerator.GetComponent<IGetBoardData>();
        }

        private void OnDrawGizmos()
        {
            if (boardData != null && boardData.GetTiles() != null)
            {
                foreach (Tile tile in boardData.GetTiles())
                {
                    Vector3 spawnPosition = tile.WorldReference.transform.position;
                    spawnPosition.y += drawHeight * .5f;

                    Gizmos.color = tile.IsPassable ? unpassableColour : passableColour;
                    Gizmos.DrawWireCube(spawnPosition, new Vector3(1, 1 + drawHeight, 1));
                }
            }
            
        }
    }
}
