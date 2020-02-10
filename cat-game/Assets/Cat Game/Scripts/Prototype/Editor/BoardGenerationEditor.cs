using UnityEngine;
using UnityEditor;

namespace SiegeOfAshes.Board
{
    [CustomEditor(typeof(BoardGeneration))]
    public class BoardGenerationEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BoardGeneration boardGeneration = (BoardGeneration)target;

            if (GUILayout.Button("Automatic"))
            {
                boardGeneration.CreatePerlinNoise();
                boardGeneration.currentNoiseData.IncreaseContrast(.5f);

                boardGeneration.currentNoiseData.BalanceMap();

                boardGeneration.CreateBoard();
            }

            if (GUILayout.Button("Generate Image"))
            {
                boardGeneration.CreatePerlinNoise();
            }

            if (GUILayout.Button("Increase Contrast"))
            {
                boardGeneration.currentNoiseData.IncreaseContrast(.5f);
            }

            if (GUILayout.Button("Create Map"))
            {
                if (boardGeneration.currentNoiseData == null) boardGeneration.CreatePerlinNoise();
                boardGeneration.CreateBoard();
            }

            if (GUILayout.Button("Mirror"))
            {
                boardGeneration.currentNoiseData.Texture = boardGeneration.currentNoiseData.MirrorImage(boardGeneration.currentNoiseData.Texture);
            }

            if (GUILayout.Button("Destroy Board"))
            {
                boardGeneration.DestroyBoard();
            }
        }
    }

}
