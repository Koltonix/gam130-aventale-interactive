using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatGame.ControlScheme
{
    public class Keybinds : MonoBehaviour
    {

        public static Keybinds KeybindsManager;

        public KeyCode NextCamera { get; set; }
        public KeyCode LastCamera { get; set; }
        public KeyCode ZoomIn { get; set; }
        public KeyCode ZoomOut { get; set; }
        public KeyCode PauseBack { get; set; }
        public KeyCode BirdsEye { get; set; }
        public KeyCode Select { get; set; }

        void Awake()
        {

            if (KeybindsManager == null)
            {
                DontDestroyOnLoad(gameObject);
                KeybindsManager = this;
            }
            else if (KeybindsManager != this)
            {
                Destroy(gameObject);
            }

            UpdateKeys();
        }

        void UpdateKeys()
        {
            NextCamera = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("NextCamera", "E"));
            LastCamera = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("LastCamera", "Q"));
            ZoomIn = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ZoomIn", "W"));
            ZoomOut = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("ZoomOut", "S"));
            PauseBack = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("PauseBack", "Escape"));
            BirdsEye = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("BirdsEye", "Tab"));
            Select = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Select", "Mouse0"));

        }

        public void SetBinding(string bindingName, string newBinding)
        {
            PlayerPrefs.SetString(bindingName, newBinding);
            UpdateKeys();
        }
    }
}
