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
                if (allPlayers[TurnManager.Instance.GetCurrentPlayerIndex()] == players[i]) players[i].isActive = true;
            }
        }


        #region Contractual Obligations
        public Player GetCurrentPlayer()
        {
            return allPlayers[TurnManager.Instance.GetCurrentPlayerIndex()];
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