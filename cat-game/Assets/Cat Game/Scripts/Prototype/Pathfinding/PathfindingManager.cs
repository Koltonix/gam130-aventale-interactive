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

        private void Start()
        {
            boardData = boardGenerator.GetComponent<IGetBoardData>();
        }

        private void OnDrawGizmos()
        {
            if (boardData != null && boardData.GetTiles() != null)
            {
                Gizmos.DrawWireCube(boardData.GetBoardCentre(), new Vector3(boardData.GetBoardWidth(), boardData.GetBoardCentre().y + 1, boardData.GetBoardHeight()));

                foreach (Tile tile in boardData.GetTiles())
                {
                    Vector3 spawnPosition = tile.WorldReference.transform.position;
                    spawnPosition.y++;

                    Gizmos.color = tile.IsPassable ? unpassableColour : passableColour;
                    Gizmos.DrawCube(spawnPosition, Vector3.one);
                }
            }
            
        }
    }
}
