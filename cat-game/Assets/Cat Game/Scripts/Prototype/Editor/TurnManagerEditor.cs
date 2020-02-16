using UnityEngine;
using UnityEditor;

namespace CatGame.Data
{
    [CustomEditor(typeof(GameController))]
    public class TurnManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GameController turnManager = (GameController)target;
            
            if (GUILayout.Button("Cycle Players"))
            {
                turnManager.currentPlayerIndex = turnManager.GetNextPlayersTurn(turnManager.currentPlayerIndex);
            }
        }
    }
}

