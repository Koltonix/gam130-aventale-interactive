using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatGame.ControlScheme
{
    public class Keybinds : MonoBehaviour
    {

        public static Keybinds KeybindsManager;

        public KeyCode up { get; set; }
        public KeyCode down { get; set; }
        public KeyCode forward { get; set; }
        public KeyCode backward { get; set; }
        public KeyCode left { get; set; }
        public KeyCode right { get; set; }
        public KeyCode boost { get; set; }

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
            up = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("upKey", "E"));
            down = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("downKey", "Q"));
            forward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("forwardKey", "W"));
            backward = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("backwardKey", "S"));
            left = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("leftKey", "A"));
            right = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("rightKey", "D"));
            boost = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("boostKey", "LeftShift"));
        }

        public void SetBinding(string bindingName, string newBinding)
        {
            PlayerPrefs.SetString(bindingName, newBinding);
            UpdateKeys();
        }
    }
}
