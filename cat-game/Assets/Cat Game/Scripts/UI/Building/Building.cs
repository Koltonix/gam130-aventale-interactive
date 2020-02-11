using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour, IClickable
{
    [SerializeField]
    private GameObject BuildingMenu;
    private GameObject buildingMenu;
    void Start()
    {
        buildingMenu = Instantiate(BuildingMenu, gameObject.transform);
        buildingMenu.SetActive(false);
        buildingMenu.transform.position = new Vector3(buildingMenu.transform.position.x, buildingMenu.transform.position.y + 2, buildingMenu.transform.position.z);
    }

    public void ActionOnClick()
    {
        buildingMenu.SetActive(!buildingMenu.activeSelf);
    }
}
