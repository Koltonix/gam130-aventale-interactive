using System;
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
        private bool uIState;

        void Start()
        {
            buildMenu = Instantiate(BuildmenuPrefab, gameObject.transform);
            buildMenu.transform.position = new Vector3(buildMenu.transform.position.x, buildMenu.transform.position.y + 3, buildMenu.transform.position.z);
            setBuildingUI(false);
        }

        private void setBuildingUI(bool toggle)
        {
            uIState = toggle;
            buildMenu.SetActive(toggle);
            foreach (SpawnPad pad in spawnPads)
            {
                pad.DeselectTile();
                selectedPad = null;
                pad.gameObject.SetActive(toggle);
            }
        }

        void toggleBuildingUI()
        {
            uIState = !uIState;
            setBuildingUI(uIState);
        }

        void OnMouseDown()
        {
            toggleBuildingUI();
        }

        public void SelectTile(SpawnPad pad)
        {
            if (selectedPad)
            {
                selectedPad.DeselectTile();
            }
            selectedPad = pad;
        }

        public void SpawnUnit()
        {
            if (selectedPad)
            {
                GameObject newUnit = Instantiate(units[0]);
                newUnit.transform.position = new Vector3(selectedPad.transform.position.x, selectedPad.transform.position.y + 0.7f, selectedPad.transform.position.z);
                toggleBuildingUI();
            }
        }
    }
}
