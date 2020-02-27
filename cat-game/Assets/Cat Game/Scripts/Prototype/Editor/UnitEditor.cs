using UnityEngine;
using UnityEditor;
using CatGame.Tiles;

namespace CatGame.Units
{
    [CustomEditor(typeof(UnitMovement))]
    public class UnitEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            UnitMovement unit = (UnitMovement)target;

            if (GUILayout.Button("Tiles Available"))
            {
                unit.DetermineTilesInSphere(BoardManager.Instance.tiles);
            }
        }
    }

}