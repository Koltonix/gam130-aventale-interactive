﻿using UnityEngine;
using UnityEditor;

/// <summary>
/// Editor GUI Buttons for the GameEvents to trigger them..
/// Sourced: https://www.youtube.com/watch?v=dtRwpcegzuc
/// </summary>
namespace CatGame.Menu
{
    [CustomEditor(typeof(GameEvent))]
    public class GameEventEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUI.enabled = Application.isPlaying;

            GameEvent gameEvent = (GameEvent)target;
            if (GUILayout.Button("Raise")) gameEvent.Raise();
        }
    }
}
