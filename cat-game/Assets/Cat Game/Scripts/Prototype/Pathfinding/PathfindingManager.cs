using System.Collections.Generic;
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

        [Header("Pathfinding Values")]
        private List<Tile> finalPath;

        [Header("Interface Initialisers")]
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

        private void CreatePathfindingGrid()
        {
           
        }

        private void OnDrawGizmos()
        {
            if (boardData != null && boardData.GetTiles() != null)
            {
                foreach (Tile tile in boardData.GetTiles())
                {
                    Vector3 spawnPosition = tile.worldReference.transform.position;
                    spawnPosition.y += drawHeight * .5f;

                    Gizmos.color = tile.isPassable ? unpassableColour : passableColour;

                    if (finalPath != null)
                    {
                        if (finalPath.Contains(tile))
                        {
                            Gizmos.color = Color.blue;
                        }
                    }

                    Gizmos.DrawWireCube(spawnPosition, new Vector3(1, 1 + drawHeight, 1));
                }
            }
            
        }
    }
}
