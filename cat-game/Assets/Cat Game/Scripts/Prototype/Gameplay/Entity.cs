using UnityEngine;

namespace CatGame.Data
{
    public class Entity : MonoBehaviour
    {
        public Player owner;
        public Player currentPlayer;
        public bool isEnabled;

        protected virtual void Awake()
        {
            TurnManager.Instance.onPlayerCycle += OnTurnEnd;
            owner = PlayerManager.Instance.GetCurrentPlayer();

            OnTurnEnd(PlayerManager.Instance.GetCurrentPlayer());
        }

        protected virtual void OnTurnEnd(Player player)
        {
            currentPlayer = player;
            isEnabled = (owner == currentPlayer);
        }
    }
}

