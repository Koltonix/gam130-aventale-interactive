using UnityEngine;

namespace CatGame.Data
{
    public class Entity : MonoBehaviour
    {
        public Player owner;
        public Player currentPlayer;
        public bool isEnabled;

        protected virtual void Start()
        {
            TurnManager.Instance.onPlayerCycle += OnTurnEnd;
            owner = PlayerManager.Instance.GetCurrentPlayer();
        }

        protected virtual void OnTurnEnd(Player player)
        {
            currentPlayer = player;
            isEnabled = (owner == currentPlayer);
        }
    }
}

