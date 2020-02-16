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
        [Header("Information")]
        public int number;
        public bool isActive;

        private int actionPoints;
        public int defaultActionPoints = 8;

        [Header("Aesthetic")]
        public Color32 colour;

        public delegate void OnActivation();
        public event OnActivation onActive;

        public void ActivateUnit(bool isEnabled)
        {
            isActive = isEnabled;
            onActive?.Invoke();
        }

        public void ResetActionPoints(Player player)
        {
            if (player == this) actionPoints = defaultActionPoints;
        }
    }
}
