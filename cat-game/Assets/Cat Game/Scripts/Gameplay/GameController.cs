using UnityEngine;
using UnityEngine.SceneManagement;
using CatGame.Data;

namespace CatGame
{
    /// <summary>
    /// Controls the state of the game and loads up relevant data
    /// depending on that state.
    /// </summary>
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

        public int mainMenuBuildIndex = 0;

        /// <summary>Checks which Player is currently active to see which one won.</summary>
        public void CheckIfWon()
        {
            Player winningPlayer = PlayerManager.Instance.GetCurrentPlayer();

            //DISABLE SCRIPTS
            //LOAD WIN SCREEN

            SceneController.Instance.LoadScene(mainMenuBuildIndex);
        }
    }
}

