using UnityEngine;
using UnityEngine.SceneManagement;
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

        public int mainMenuBuildIndex = 0;

        public void CheckIfWon()
        {
            Player winningPlayer = PlayerManager.Instance.GetCurrentPlayer();

            //DISABLE SCRIPTS
            //LOAD WIN SCREEN

            SceneController.Instance.LoadScene(mainMenuBuildIndex);
        }
    }
}

