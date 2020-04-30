using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.ControlScheme;

namespace CatGame.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenu;
        [SerializeField] private Catopedia catopedia;
        [SerializeField]
        private MonoBehaviour[] onPauseDisable;

        // Called by buttons.
        public void TogglePauseMenu()
        {
            if (pauseMenu.activeSelf)
            {
                pauseMenu.SetActive(false);
                Time.timeScale = 1.0f;
                ChangeMonoBehaviourState(onPauseDisable, true);
            }
            else
            {
                pauseMenu.SetActive(true);
                Time.timeScale = 0.0f;
                ChangeMonoBehaviourState(onPauseDisable, false);
            }
        }

        private void ChangeMonoBehaviourState(MonoBehaviour[] behaviours, bool state)
        {
            foreach (MonoBehaviour behaviour in behaviours) behaviour.enabled = state;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(Keybinds.KeybindsManager.PauseBack))
            {
                TogglePauseMenu();
            }
        }

        public void ToggleCatopedia()
        {
            catopedia.Toggle();
        }
    }
}