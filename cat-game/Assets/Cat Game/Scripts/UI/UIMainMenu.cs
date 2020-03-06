using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatGame.UI
{
    // This is the main menu script. All of the functions are designed to be called buy buttons.
    public class UIMainMenu : MonoBehaviour
    {
        public void NewGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void Quit()
        {
            Application.Quit();
        }

        public void Options()
        {

        }

        public void Debug()
        {
            SceneManager.LoadScene(1);
        }
    }
}