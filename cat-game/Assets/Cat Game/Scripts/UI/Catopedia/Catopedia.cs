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
    [SerializeField] private Text title;
    [SerializeField] private Button exitButton;
    [SerializeField] private Button lastButton;
    [SerializeField] private Button nextButton;

    void Start()
    {
        UpdateEntry();
        exitButton.onClick.AddListener(Toggle);
        nextButton.onClick.AddListener(Next);
        lastButton.onClick.AddListener(Previous);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void UpdateEntry()
    {
        title.text = catopediaEntries[currentEntry].name;
        text.text = catopediaEntries[currentEntry].description;
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
        if (currentEntry != catopediaEntries.Length-1)
        {
            currentEntry++;
            UpdateEntry();
        }
    }
}
}