using UnityEngine;
using UnityEditor;
using CatGame.Tiles;

namespace CatGame.Units
{
    /// <summary>
    /// Editor GUI Buttons for the Inspector for the UnitMovement
    /// for testing purposes.
    /// </summary>
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