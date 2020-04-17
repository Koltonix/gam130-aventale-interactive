using UnityEngine;
using UnityEditor;

/// <summary>
/// Christopher Robertson 2020
/// Provides Editor GUI Buttons on the Inspect for the 
/// PrefabNumbering class.
/// </summary>
[CustomEditor(typeof(PrefabNumbering))]
public class PrefabNumberingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PrefabNumbering numbering = (PrefabNumbering)target;

        if (GUILayout.Button("Rename all Prefabs"))
        {
            numbering.RenameAllGameObjects();
        }
    }
}
