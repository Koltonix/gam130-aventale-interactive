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

        private IGetBoardData boardData;

        private void Start()
        {
            boardData = GenerateDebugBoard.Instance.GetComponent<IGetBoardData>();
        }

        private void OnDrawGizmos()
        {
            if (boardData != null)
            {
                Gizmos.DrawWireCube(boardData.GetBoardCentre(), new Vector3(boardData.GetBoardWidth(), boardData.GetBoardCentre().y, boardData.GetBoardHeight()));
            }
            
        }
    }
}
