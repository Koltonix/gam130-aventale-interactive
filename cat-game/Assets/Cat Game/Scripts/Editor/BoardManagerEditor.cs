using UnityEngine;
using UnityEditor;

namespace CatGame.Tiles
{
    [CustomEditor(typeof(BoardManager))]
    public class BoardManagerEditor : Editor
    {

        private BoardManager boardManager;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            boardManager = (BoardManager)target;

            if (GUILayout.Button("CREATE BOARD"))
            {
                if (boardManager.board) DestroyImmediate(boardManager.board);
                boardManager.board = SpawnBoard();
            }
        }

        private GameObject SpawnBoard()
        {
            GameObject parent = new GameObject("Board");
            GameObject prefab = boardManager.passableTilePrefab;

            Vector2Int tileGap = boardManager.tileGap;
            int boardWidth = boardManager.GetBoardWidth();
            int boardHeight = boardManager.GetBoardHeight();

            for (int x = 0; x < boardWidth; x++)
            {
                for (int y = 0; y < boardHeight; y++)
                {
                    Vector3 spawnPosition = new Vector3(x * tileGap.x - ((boardWidth - 1) * tileGap.x * .5f),
                                                        0.0f,
                                                        y * tileGap.y - ((boardHeight - 1) * tileGap.y * .5f));

                    GameObject clone = Instantiate(prefab, spawnPosition, Quaternion.identity, parent.transform);

                    clone.transform.localScale = new Vector3(clone.transform.localScale.x * tileGap.x, 
                                                             boardManager.yScale, 
                                                             clone.transform.localScale.z * tileGap.y);
                }
            }

            return parent;
        }
    }
}

