using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Catopedia : MonoBehaviour
{
    [SerializeField] private CatopediaEntry entry;
    [SerializeField] private Text text;
    [SerializeField] private Text title;
    [SerializeField] private Button exitButton;
    void Start()
    {
        text.text = entry.description;
        title.text = entry.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
