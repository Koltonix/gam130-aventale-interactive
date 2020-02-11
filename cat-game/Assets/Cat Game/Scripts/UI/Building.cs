using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IClickable
{
    [SerializeField]
    private GameObject buildingMenu;
    void Start()
    {
        buildingMenu.SetActive(false);
    }

    public void ActionOnClick()
    {
        buildingMenu.SetActive(!buildingMenu.activeSelf);
    }
}
