﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    }

    public void Quit()
    {
        Application.Quit();
    }
}
