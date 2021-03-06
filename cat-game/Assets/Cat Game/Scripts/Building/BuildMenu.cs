﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CatGame.Data
{
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField]
        public Button[] Buttons;
        [SerializeField]
        private GameObject buttonPrefab;

        public void GenerateButtons(GameObject[] units)
        {
            Button[] newButtons = new Button[units.Length];
            for (int i = units.Length - 1; i >= 0; i--)
            {
                Button newButton = Instantiate(buttonPrefab, this.transform).GetComponent<Button>();
                newButton.gameObject.transform.localPosition = new Vector3(0, i*200, 0);
                newButtons[i] = newButton;
            }
            Buttons = newButtons;
        }
    }
}