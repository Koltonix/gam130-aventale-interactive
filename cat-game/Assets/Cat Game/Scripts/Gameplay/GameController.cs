using UnityEngine;
using CatGame.Data;

namespace CatGame
{
    public class GameController : MonoBehaviour
    {
        #region Singleton Reference
        public static GameController Instance;
        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }
        #endregion

        public void CheckIfWon()
        {
            Player winningPlayer = PlayerManager.Instance.GetCurrentPlayer();
            if (winningPlayer == null) return;

            //DISABLE SCRIPTS
            //LOAD WIN SCREEN
            Debug.Log(winningPlayer.colour);
        }
    }
}

