using UnityEngine;
using UnityEditor;

namespace CatGame.Data
{
    [CustomEditor(typeof(TurnManager))]
    public class TurnManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            TurnManager turnManager = (TurnManager)target;
            
            if (GUILayout.Button("Cycle Players"))
            {
                turnManager.currentPlayer = turnManager.GetNextPlayersTurn(turnManager.currentPlayer);
            }
        }
    }
}

