using System;
using UnityEngine;
using UnityEngine.Events;

namespace SiegeOfAshes.Data
{
    public class TurnManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField]
        private Player[] allPlayers;

        [Header("Current Turn")]
        public int currentPlayer;

        public delegate void OnPlayerCycle(Player player);
        private event OnPlayerCycle onPlayerCycle;

        private void Start()
        {
            if (allPlayers.Length == 0) throw new Exception("No Players are in the game");
            AssignPlayerNumbers(allPlayers);
        }

        private void AssignPlayerNumbers(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].number = i + 1;
            }
        }

        public int CyclePlayers(int playerIndex)
        {
            playerIndex++;
            if (playerIndex >= allPlayers.Length)
            {
                onPlayerCycle.Invoke(allPlayers[playerIndex]);
                return 0;
            }

            onPlayerCycle.Invoke(allPlayers[playerIndex]);
            return playerIndex;
        }
    }
}
