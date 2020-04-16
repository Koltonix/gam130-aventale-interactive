﻿using UnityEngine;
using UnityEditor;

/// <summary>
/// Created by Christopher Robertson
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