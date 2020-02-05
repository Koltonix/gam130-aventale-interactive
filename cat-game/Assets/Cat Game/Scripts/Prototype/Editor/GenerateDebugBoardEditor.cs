using UnityEngine;
using UnityEditor;

namespace SiegeOfAshes.Pathfinding
{
    [CustomEditor(typeof(GenerateDebugBoard))]
    public class GenerateDebugBoardEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GenerateDebugBoard generateDebugBoard = (GenerateDebugBoard)target;

            if (GUILayout.Button("Generate Board"))
            {
                generateDebugBoard.GenerateBoard();
            }

            if (GUILayout.Button("Delete Board"))
            {
                generateDebugBoard.DeleteBoard();
            }
        }
    }
}

