using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SiegeOfAshes.UI
{
    public class Building : MonoBehaviour
    {
        [SerializeField]
        private GameObject BuildingMenu;
        private GameObject buildingMenu;
        [SerializeField]
        private GameObject[] spawnPads;
        [SerializeField]
        private GameObject[] units;

        void Start()
        {
            buildingMenu = Instantiate(BuildingMenu, gameObject.transform);
            buildingMenu.SetActive(false);
            buildingMenu.transform.position = new Vector3(buildingMenu.transform.position.x, buildingMenu.transform.position.y + 3, buildingMenu.transform.position.z);
            foreach (GameObject pad in spawnPads)
            {
                pad.SetActive(!pad.activeSelf);
            }
        }

        void OnMouseDown()
        {
            buildingMenu.SetActive(!buildingMenu.activeSelf);
            foreach (GameObject pad in spawnPads)
            {
                pad.SetActive(!pad.activeSelf);
            }
        }

        public void SpawnUnit(Transform pad)
        {
            GameObject newUnit = Instantiate(units[0]);
            newUnit.transform.position = new Vector3(pad.position.x, pad.position.y+0.7f, pad.position.z);
        }
    }
}
