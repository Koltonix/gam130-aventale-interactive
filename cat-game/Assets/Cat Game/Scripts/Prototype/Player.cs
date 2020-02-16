using System;
using UnityEngine;
using UnityEngine.Events;

namespace CatGame.Data
{
    /// <summary>
    /// Stores the data of each player that is currently playing.
    /// </summary>
    [Serializable]
    public class Player
    {
        public int number;
        public Color32 colour;
        public bool isActive;

        private int actionPoints;
        public int defaultActionPoints = 8;

        public delegate void OnActivation();
        public event OnActivation onActive;

        public void ActivateUnit()
        {
            isActive = true;
            onActive?.Invoke();
        }

        public void ResetActionPoints(Player player)
        {
            if (player == this) actionPoints = defaultActionPoints;
        }
    }
}
