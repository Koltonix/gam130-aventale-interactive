using System;
using UnityEngine;

namespace CatGame.Data
{
    public class GameController : MonoBehaviour
    {
        #region Singleton
        public static GameController Instance;
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
        public int currentPlayerIndex;

        public delegate void OnPlayerCycle(Player player);
        public event OnPlayerCycle onPlayerCycle;

        private void Start()
        {
            if (allPlayers.Length == 0) throw new Exception("No Players are in the game");
            AssignPlayerNumbers(allPlayers);

            onPlayerCycle?.Invoke(allPlayers[currentPlayerIndex]);
        }

        private void AssignPlayerNumbers(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].number = i;
                if (allPlayers[currentPlayerIndex] == players[i]) players[i].isActive = true;
            }
        }

        public void EndTurn()
        {
            currentPlayerIndex = GetNextPlayersTurn(currentPlayerIndex);
        }

        public int GetNextPlayersTurn(int playerIndex)
        {
            allPlayers[playerIndex].ActivateUnit(false);

            playerIndex++;
            if (playerIndex >= allPlayers.Length)
            {
                playerIndex = 0;

                Debug.Log(String.Format("Player {0}'s turn", playerIndex));

                onPlayerCycle?.Invoke(allPlayers[playerIndex]);
                allPlayers[playerIndex].ActivateUnit(true);
                return 0;
            }

            Debug.Log(String.Format("Player {0}'s turn", playerIndex));

            onPlayerCycle?.Invoke(allPlayers[playerIndex]);
            allPlayers[playerIndex].ActivateUnit(true);

            return playerIndex;
        }

        public Player GetCurrentPlayer()
        {
            return allPlayers[currentPlayerIndex];
        }

        public Player GetPlayerFromIndex(int index)
        {
            if (index <= allPlayers.Length - 1) return allPlayers[index];
            return null;
        }
    }
}
