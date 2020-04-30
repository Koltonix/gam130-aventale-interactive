using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CatGame.ControlScheme;
using TMPro;

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
            if (backButton) backButton.onClick.AddListener(BackButton);
            configButtons[0].onClick.AddListener(SetNextCamera);
            configButtons[1].onClick.AddListener(SetLastCamera);
            configButtons[2].onClick.AddListener(SetZoomIn);
            configButtons[3].onClick.AddListener(SetZoomOut);
            configButtons[4].onClick.AddListener(SetPauseBack);
            configButtons[5].onClick.AddListener(SetBirdsEye);
            configButtons[6].onClick.AddListener(SetSelect);
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
            configButtons[0].GetComponentInChildren<TextMeshProUGUI>().text = "Configure next camera Key (" + PlayerPrefs.GetString("NextCamera", "E") + ")";
            configButtons[1].GetComponentInChildren<TextMeshProUGUI>().text = "Configure last camera Key (" + PlayerPrefs.GetString("LastCamera", "Q") + ")";
            configButtons[2].GetComponentInChildren<TextMeshProUGUI>().text = "Configure Zoom In Key (" + PlayerPrefs.GetString("ZoomIn", "W") + ")";
            configButtons[3].GetComponentInChildren<TextMeshProUGUI>().text = "Configure Zoom Out Key (" + PlayerPrefs.GetString("ZoomOut", "S") + ")";
            configButtons[4].GetComponentInChildren<TextMeshProUGUI>().text = "Configure Pause/Back Key (" + PlayerPrefs.GetString("PauseBack", "A") + ")";
            configButtons[5].GetComponentInChildren<TextMeshProUGUI>().text = "Configure Toggle Bird's Eye Key (" + PlayerPrefs.GetString("BirdsEye", "Tab") + ")";
            configButtons[6].GetComponentInChildren<TextMeshProUGUI>().text = "Configure Select Key (" + PlayerPrefs.GetString("Select", "Mouse0") + ")";
        }

        void SetNextCamera()
        {
            waitingForInput = true;
            buttonToSet = "NextCamera";
            prompt.SetActive(true);
        }

        void SetLastCamera()
        {
            waitingForInput = true;
            buttonToSet = "LastCamera";
            prompt.SetActive(true);
        }
        void SetZoomIn()
        {
            waitingForInput = true;
            buttonToSet = "ZoomIn";
            prompt.SetActive(true);
        }
        void SetZoomOut()
        {
            waitingForInput = true;
            buttonToSet = "ZoomOut";
            prompt.SetActive(true);
        }
        void SetPauseBack()
        {
            waitingForInput = true;
            buttonToSet = "PauseBack";
            prompt.SetActive(true);
        }
        void SetBirdsEye()
        {
            waitingForInput = true;
            buttonToSet = "BirdsEye";
            prompt.SetActive(true);
        }
        void SetSelect()
        {
            waitingForInput = true;
            buttonToSet = "Select";
            prompt.SetActive(true);
        }
    }
}
