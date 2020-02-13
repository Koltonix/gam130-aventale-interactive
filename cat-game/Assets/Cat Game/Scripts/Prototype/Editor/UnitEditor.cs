using UnityEngine;
using UnityEditor;
using CatGame.Tiles;

namespace CatGame.Movement
{
    [CustomEditor(typeof(Unit))]
    public class UnitEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Unit unit = (Unit)target;

            if (GUILayout.Button("Tiles Available"))
            {
                unit.DetermineAvailableTiles(BoardManager.Instance.tiles);
            }
        }
    }

}