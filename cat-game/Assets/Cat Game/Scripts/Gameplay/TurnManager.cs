using System;
using UnityEngine;

namespace CatGame.Data
{
    /// <summary>
    /// Deals with ending the turn of the current player and ensuring it can
    /// only be done when no other actions are currently occuring.
    /// </summary>
    public class TurnManager : MonoBehaviour
    {
        #region Singleton
        public static TurnManager Instance;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        #endregion

        [Header("Current Turn")]
        public int currentPlayerIndex;
        public bool objectIsMoving = false;
        public bool objectIsAttacking = false;

        [Header("Player Settings")]
        private IPlayerManager playerManagerData;
        public delegate void OnPlayerCycle(Player player);
        public event OnPlayerCycle onPlayerCycle;

        private void Start()
        {
            playerManagerData = PlayerManager.Instance;
        }

        /// <summary>Ends the current turn.</summary>
        /// <remarks>Only ends the turn once all plays have ceased.</remarks>
        public void EndTurn()
        {
            if (!objectIsMoving && !objectIsAttacking) currentPlayerIndex = GetNextPlayersTurn(currentPlayerIndex);
            else Debug.Log("OBJECT INTERACTING");
        }

        /// <summary>Cycles through the players in the game and disables everything of theirs.</summary>
        /// <param name="playerIndex">Current Player's index.</param>
        /// <returns>The next player's index.</returns>
        public int GetNextPlayersTurn(int playerIndex)
        {
            playerManagerData.GetAllPlayers()[playerIndex].ActivateUnit(false);

            playerIndex++;
            if (playerIndex >= playerManagerData.GetAllPlayers().Length)
            {
                playerIndex = 0;

                Debug.Log(String.Format("Player {0}'s turn", playerIndex));

                onPlayerCycle?.Invoke(playerManagerData.GetAllPlayers()[playerIndex]);
                playerManagerData.GetAllPlayers()[playerIndex].ActivateUnit(true);
                return 0;
            }

            Debug.Log(String.Format("Player {0}'s turn", playerIndex));

            onPlayerCycle?.Invoke(playerManagerData.GetAllPlayers()[playerIndex]);
            playerManagerData.GetAllPlayers()[playerIndex].ActivateUnit(true);

            return playerIndex;
        }

        public int GetCurrentPlayerIndex()
        {
            return currentPlayerIndex;
        }
    }
}

