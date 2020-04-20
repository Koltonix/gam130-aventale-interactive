using UnityEngine;

namespace CatGame.Data
{
    /// <summary>
    /// Parent Class for any Entity object in the Game so that it can keep 
    /// track of the current player and the owner.
    /// </summary>
    public class Entity : MonoBehaviour
    {
        public Player owner;
        public Player currentPlayer;
        public bool isEnabled;

        protected virtual void Start()
        {
            //Subscribes to the OnPlayerCycle event.
            TurnManager.Instance.onPlayerCycle += OnTurnEnd;
            owner = PlayerManager.Instance.GetCurrentPlayer();

            OnTurnEnd(PlayerManager.Instance.GetCurrentPlayer());
        }

        /// <summary>Sets the current player's turn.</summary>
        /// <param name="player">The player who's turn it is.</param>
        protected virtual void OnTurnEnd(Player player)
        {
            currentPlayer = player;
            isEnabled = (owner == currentPlayer);
        }
    }
}

