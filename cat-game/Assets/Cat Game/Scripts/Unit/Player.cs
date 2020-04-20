using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using CatGame.Units;

namespace CatGame.Data
{
    /// <summary>
    /// Stores the data of each player that is currently playing.
    /// </summary>
    [Serializable]
    public class Player : IPlayerData
    {
        [Header("Information")]
        public int hash;
        public int number;
        public bool isActive;

        public int unitCap = 10;
        public List<Unit> PlayerUnits = new List<Unit>();

        private int actionPoints;
        public int ActionPoints
        {
            get { return actionPoints; }
            set
            {
                actionPoints = value;
                onAP?.Invoke(actionPoints);
            }
        }

        public int defaultActionPoints = 8;

        [Header("Aesthetic")]
        public Color32 colour;

        public delegate void OnActivation();
        public event OnActivation onActive;

        public delegate void OnAP(int actionPoints);
        public event OnAP onAP;

        public Player()
        {
            ActionPoints = defaultActionPoints;
            hash = this.GetHashCode();
        }

        /// <summary>Enables the Unit</summary>
        /// <param name="isEnabled">Unit's current state</param>
        public void ActivateUnit(bool isEnabled)
        {
            ActionPoints = defaultActionPoints;

            isActive = isEnabled;
            onActive?.Invoke();
        }

        /// <summary>Resets the Action Points of the Player.</summary>
        /// <param name="player">Current Player's Turn.</param>
        public void ResetActionPoints(Player player)
        {
            if (player == this) ActionPoints = defaultActionPoints;
        }

        #region Contractual Obligations
        public Player GetPlayerReference()
        {
            return this;
        }

        public int GetCurrentActionPoints()
        {
            return ActionPoints;
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
