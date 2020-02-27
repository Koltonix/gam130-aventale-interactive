using System;
using UnityEngine;

namespace CatGame.Data
{
    public class PlayerManager : MonoBehaviour, IPlayerManager
    {
        #region Singleton
        public static PlayerManager Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        #endregion

        [Header("Players Settings")]
        [SerializeField]
        private Player[] allPlayers;

        private ITurn turnData;

        private void Start()
        {
            turnData = TurnManager.Instance;

            if (allPlayers.Length == 0) throw new Exception("No Players are in the game");
            AssignPlayerNumbers(allPlayers);
        }

        private void AssignPlayerNumbers(Player[] players)
        {
            for (int i = 0; i < players.Length; i++)
            {
                players[i].number = i;
                if (allPlayers[turnData.GetCurrentPlayerIndex()] == players[i]) players[i].isActive = true;
            }
        }


        #region Contractual Obligations
        public Player GetCurrentPlayer()
        {
            if (turnData == null) turnData = TurnManager.Instance;
            return allPlayers[turnData.GetCurrentPlayerIndex()];
        }

        public Player[] GetAllPlayers()
        {
            return allPlayers;
        }

        public Player GetPlayerFromIndex(int index)
        {
            if (index <= allPlayers.Length - 1) return allPlayers[index];
            return null;
        }
        #endregion
    }
}
