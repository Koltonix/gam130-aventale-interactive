using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIPauseMenu : MonoBehaviour
{
    private UIController uIController;

    // Start is called before the first frame update
    void Start()
    {
        uIController = GameObject.FindObjectOfType<UIController>();
    }

    public void Resume()
    {
        uIController.togglePauseMenu();
    }

    public void Options()
    {

    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
