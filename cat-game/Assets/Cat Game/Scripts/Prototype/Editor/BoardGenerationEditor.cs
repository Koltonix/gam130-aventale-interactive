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

            if (GUILayout.Button("Generate Image"))
            {
                boardGeneration.CreatePerlinNoise();
            }

            if (GUILayout.Button("Create Map"))
            {
                if (boardGeneration.currentNoiseData == null) boardGeneration.CreatePerlinNoise();
                boardGeneration.CreateBoard(boardGeneration.currentNoiseData);
            }

            if (GUILayout.Button("Increase Contrast"))
            {
                boardGeneration.currentNoiseData.IncreaseContrast(.1f);
            }

            if (GUILayout.Button("Automatic"))
            {
                boardGeneration.CreatePerlinNoise();
                boardGeneration.currentNoiseData.IncreaseContrast(.5f);

                boardGeneration.currentNoiseData.texture = boardGeneration.currentNoiseData.MirrorImage(boardGeneration.currentNoiseData.texture);
                if (boardGeneration.NoiseMapImage != null) boardGeneration.NoiseMapImage.texture = boardGeneration.currentNoiseData.texture;
                boardGeneration.currentNoiseData.BalanceMap();

                boardGeneration.CreateBoard(boardGeneration.currentNoiseData);
            }

            if (GUILayout.Button("Mirror"))
            {
                //caveGeneration.Mirror(caveGeneration.currentNoiseData, caveGeneration.tileParent);
                boardGeneration.currentNoiseData.texture = boardGeneration.currentNoiseData.MirrorImage(boardGeneration.currentNoiseData.texture);
                if (boardGeneration.NoiseMapImage != null) boardGeneration.NoiseMapImage.texture = boardGeneration.currentNoiseData.texture;
            }

            if (GUILayout.Button("Stop Spawning"))
            {
                boardGeneration.StopBoardSpawning();
            }

            if (boardGeneration.NoiseMapImage != null) boardGeneration.NoiseMapImage.SetNativeSize();
        }
    }

}