using System;
using UnityEngine;

namespace CatGame.Data
{
    public delegate void OnPlayerCycle(Player player);
   
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
        public bool objectIsMoving;

        [Header("Player Settings")]
        private IPlayerManager playerManagerData;
        public event OnPlayerCycle onPlayerCycle;

        private void Start()
        {
            playerManagerData = PlayerManager.Instance;
        }

        public void EndTurn()
        {
            if (!objectIsMoving) currentPlayerIndex = GetNextPlayersTurn(currentPlayerIndex);
            else Debug.Log("OBJECT IS MOVING STILL");
        }

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

