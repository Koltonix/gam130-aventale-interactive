using System;
using UnityEngine;

namespace CatGame.Data
{
    /// <summary>
    /// Stores the data of each player that is currently playing.
    /// </summary>
    [Serializable]
    public class Player : IPlayerData
    {
        [Header("Information")]
        private int hashCode;

        public int number;
        public bool isActive;

        public int actionPoints;
        public int defaultActionPoints = 8;

        [Header("Aesthetic")]
        public Color32 colour;

        public delegate void OnActivation();
        public event OnActivation onActive;

        public Player()
        {
            hashCode = this.GetHashCode();
        }

        public void ActivateUnit(bool isEnabled)
        {
            actionPoints = defaultActionPoints;

            isActive = isEnabled;
            onActive?.Invoke();
        }

        public void ResetActionPoints(Player player)
        {
            if (player == this) actionPoints = defaultActionPoints;
        }

        #region Contractual Obligations
        public Player GetPlayerReference()
        {
            return this;
        }

        public int GetCurrentActionPoints()
        {
            return actionPoints;
        }

        public int GetDefaultActionPoints()
        {
            return defaultActionPoints;
        }

        public bool GetActiveState()
        {
            return isActive;
        }
        #endregion
    }
}
