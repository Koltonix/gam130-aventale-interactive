using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>();
            Debug.Log(allMonoBehaviours.Length);

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
            if (Input.GetKeyDown(KeyCode.Escape))
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