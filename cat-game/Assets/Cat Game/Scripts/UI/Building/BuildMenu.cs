using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CatGame.UI
{
    public class BuildMenu : MonoBehaviour
    {
        [SerializeField]
        private Camera worldCamera;
        [SerializeField]
        public Button[] Buttons;
        [SerializeField]
        private GameObject buttonPrefab;

        // Update is called once per frame
        void Start()
        {
            worldCamera = Camera.main;
        }

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

        void Update()
        {
            gameObject.transform.rotation = worldCamera.transform.rotation;
        }
    }
}