using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CatGame.UI
{
    public class Catopedia : MonoBehaviour
    {
        [SerializeField] private CatopediaEntry[] catopediaEntries;
        [SerializeField] private int currentEntry;
        [SerializeField] private Text text;
        [SerializeField] private Image image;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button lastButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Dropdown selectionDropdown;

        void Start()
        {
            foreach (CatopediaEntry entry in catopediaEntries)
            {
                selectionDropdown.options.Add(new Dropdown.OptionData() { text = entry.name });
            }
            UpdateEntry();
            selectionDropdown.onValueChanged.AddListener(SelectFromList);
            exitButton.onClick.AddListener(Toggle);
            nextButton.onClick.AddListener(Next);
            lastButton.onClick.AddListener(Previous);
        }

        void UpdateEntry()
        {
            text.text = catopediaEntries[currentEntry].description;
            selectionDropdown.value = currentEntry;
            image.sprite = catopediaEntries[currentEntry].sprite;
        }

        public void Toggle()
        {
            gameObject.SetActive(!gameObject.activeSelf);
        }

        void Previous()
        {
            if (currentEntry != 0)
            {
                currentEntry--;
                UpdateEntry();
            }
        }

        void Next()
        {
            if (currentEntry != catopediaEntries.Length - 1)
            {
                currentEntry++;
                UpdateEntry();
            }
        }
        void SelectFromList(int value)
        {
            currentEntry = value;
            UpdateEntry();
        }
    }
}