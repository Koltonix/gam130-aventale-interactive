﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// This is the ingame UI script.
public class UIIngame : MonoBehaviour
{
    private UIController uIController;


    // Start is called before the first frame update
    void Start()
    {
        uIController = GameObject.FindObjectOfType<UIController>();
    }

    public void Menu()
    {
        uIController.TogglePauseMenu();
    }

    public void Scores()
    {

    }

    public void Flag()
    {
        uIController.ToggleCatopedia();
    }
}
