using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SiegeOfAshes.UI
{
    public class Building : MonoBehaviour
    {
        [SerializeField]
        private GameObject BuildmenuPrefab;
        private GameObject buildMenu;
        [SerializeField]
        private SpawnPad[] spawnPads;
        [SerializeField]
        private GameObject[] units;
        private SpawnPad selectedPad;

        void Start()
        {
            buildMenu = Instantiate(BuildmenuPrefab, gameObject.transform);
            buildMenu.transform.position = new Vector3(buildMenu.transform.position.x, buildMenu.transform.position.y + 3, buildMenu.transform.position.z);
            buildingSwitch();
        }

        void buildingSwitch()
        {
            buildMenu.SetActive(!buildMenu.activeSelf);
            foreach (SpawnPad pad in spawnPads)
            {
                pad.DeselectTile();
                selectedPad = null;
                pad.gameObject.SetActive(!pad.gameObject.activeSelf);
            }
        }

        void OnMouseDown()
        {
            buildingSwitch();
        }

        public void SelectTile(SpawnPad pad)
        {
            if (selectedPad)
            {
                selectedPad.DeselectTile();
            }
            selectedPad = pad;
        }

        public void SpawnUnit(Transform pad)
        {
            GameObject newUnit = Instantiate(units[0]);
            newUnit.transform.position = new Vector3(pad.position.x, pad.position.y+0.7f, pad.position.z);
            buildingSwitch();
        }
    }
}
