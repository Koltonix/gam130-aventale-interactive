using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CatGame.ControlScheme;

namespace CatGame.ControlScheme
{
    public class Options : MonoBehaviour
    {
        [SerializeField]
        Button[] configButtons;
        [SerializeField]
        Button backButton;
        [SerializeField]
        GameObject prompt;

        bool waitingForInput = false;
        string buttonToSet;
        string lastButtonPressed;
        
        
        // Start is called before the first frame update
        void Start()
        {
            backButton.onClick.AddListener(BackButton);
            configButtons[0].onClick.AddListener(SetUp);
            configButtons[1].onClick.AddListener(SetDown);
            configButtons[2].onClick.AddListener(SetForward);
            configButtons[3].onClick.AddListener(SetBack);
            configButtons[4].onClick.AddListener(SetLeft);
            configButtons[5].onClick.AddListener(SetRight);
            configButtons[6].onClick.AddListener(SetBoost);
            configButtons[7].onClick.AddListener(SetCameraMove);
            configButtons[8].onClick.AddListener(SetMovementSelect);
            configButtons[9].onClick.AddListener(SetAttackSelect);
            UpdateButtonText();
            prompt.SetActive(false);
        }

        void OnGUI()
        {
            if (waitingForInput)
            {
                foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKey(key))
                    {
                        lastButtonPressed = key.ToString();
                        Debug.Log(lastButtonPressed);
                        waitingForInput = false;
                        Keybinds.KeybindsManager.SetBinding(buttonToSet, lastButtonPressed);
                        prompt.SetActive(false);
                        UpdateButtonText();
                    }
                }
            }
        }

        void BackButton()
        {
            SceneManager.LoadScene("MainMenu_SCN");
        }

        void UpdateButtonText()
        {
            configButtons[0].GetComponentInChildren<Text>().text = "Configure Camera Up Key (" + PlayerPrefs.GetString("upKey", "E") + ")";
            configButtons[1].GetComponentInChildren<Text>().text = "Configure Camera Down Key (" + PlayerPrefs.GetString("downKey", "Q") + ")";
            configButtons[2].GetComponentInChildren<Text>().text = "Configure Camera Forward Key (" + PlayerPrefs.GetString("forwardKey", "W") + ")";
            configButtons[3].GetComponentInChildren<Text>().text = "Configure Camera Backward Key (" + PlayerPrefs.GetString("backwardKey", "S") + ")";
            configButtons[4].GetComponentInChildren<Text>().text = "Configure Camera Left Key (" + PlayerPrefs.GetString("leftKey", "A") + ")";
            configButtons[5].GetComponentInChildren<Text>().text = "Configure Camera Right Key (" + PlayerPrefs.GetString("rightKey", "D") + ")";
            configButtons[6].GetComponentInChildren<Text>().text = "Configure Camera Boost Key (" + PlayerPrefs.GetString("boostKey", "LeftShift") + ")";
            configButtons[7].GetComponentInChildren<Text>().text = "Configure Camera Movement Key (" + PlayerPrefs.GetString("cameraMoveKey", "Mouse2") + ")";
            configButtons[8].GetComponentInChildren<Text>().text = "Configure Movement Select Key (" + PlayerPrefs.GetString("movementSelectKey", "Mouse0") + ")";
            configButtons[9].GetComponentInChildren<Text>().text = "Configure Attack Select Key (" + PlayerPrefs.GetString("attackSelectKey", "Mouse2") + ")";
        }

        void SetUp()
        {
            waitingForInput = true;
            buttonToSet = "upKey";
            prompt.SetActive(true);
        }

        void SetDown()
        {
            waitingForInput = true;
            buttonToSet = "downKey";
            prompt.SetActive(true);
        }
        void SetForward()
        {
            waitingForInput = true;
            buttonToSet = "forwardKey";
            prompt.SetActive(true);
        }
        void SetBack()
        {
            waitingForInput = true;
            buttonToSet = "backwardKey";
            prompt.SetActive(true);
        }
        void SetLeft()
        {
            waitingForInput = true;
            buttonToSet = "leftKey";
            prompt.SetActive(true);
        }
        void SetRight()
        {
            waitingForInput = true;
            buttonToSet = "rightKey";
            prompt.SetActive(true);
        }
        void SetBoost()
        {
            waitingForInput = true;
            buttonToSet = "boostKey";
            prompt.SetActive(true);
        }
        void SetCameraMove()
        {
            waitingForInput = true;
            buttonToSet = "cameraMoveKey";
            prompt.SetActive(true);
        }
        void SetAttackSelect()
        {
            waitingForInput = true;
            buttonToSet = "attackSelectKey";
            prompt.SetActive(true);
        }
        void SetMovementSelect()
        {
            waitingForInput = true;
            buttonToSet = "movementSelectKey";
            prompt.SetActive(true);
        }
    }
}
