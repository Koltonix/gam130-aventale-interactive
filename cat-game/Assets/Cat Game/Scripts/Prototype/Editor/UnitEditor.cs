using UnityEngine;
using UnityEditor;
using CatGame.Tiles;

namespace CatGame.Units
{
    [CustomEditor(typeof(Mover))]
    public class UnitEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            Mover unit = (Mover)target;

            if (GUILayout.Button("Tiles Available"))
            {
                unit.DetermineAvailableTiles(BoardManager.Instance.tiles);
            }
        }
    }

}