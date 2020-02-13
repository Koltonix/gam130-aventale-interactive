using System;
using UnityEngine;
using UnityEngine.Events;

namespace CatGame.Data
{
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

        [Header("Game Settings")]
        [SerializeField]
        private Player[] allPlayers;

        [Header("Current Turn")]
        public int currentPlayer;

        public delegate void OnPlayerCycle(Player player);
        public event OnPlayerCycle onPlayerCycle;

        private void Start()
        {
            if (allPlayers.Length == 0) throw new Exception("No Players are in the game");
            AssignPlayerNumbers(allPlayers);
        }

        private void AssignPlayerNumbers(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].number = i;
            }
        }

        public void EndTurn()
        {
            currentPlayer = GetNextPlayersTurn(currentPlayer);
        }

        public int GetNextPlayersTurn(int playerIndex)
        {
            playerIndex++;
            if (playerIndex >= allPlayers.Length)
            {
                playerIndex = 0;
                onPlayerCycle?.Invoke(allPlayers[playerIndex]);
                return 0;
            }

            onPlayerCycle?.Invoke(allPlayers[playerIndex]);
            return playerIndex;
        }
    }
}
