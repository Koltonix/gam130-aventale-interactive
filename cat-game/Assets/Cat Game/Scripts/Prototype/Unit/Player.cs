using System;
using UnityEngine;
using UnityEngine.Events;

namespace CatGame.Data
{
    /// <summary>
    /// Stores the data of each player that is currently playing.
    /// </summary>
    [Serializable]
    public class Player : IPlayerData
    {
        [Header("Information")]
        public int number;
        public bool isActive;

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

        public void ActivateUnit(bool isEnabled)
        {
            ActionPoints = defaultActionPoints;

            isActive = isEnabled;
            onActive?.Invoke();
        }

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
