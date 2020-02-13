using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Data;

namespace CatGame.UI
{
    public class Building : MonoBehaviour
    {

        [Header("Attributes")]
        [SerializeField]
        public Player CurrentPlayer;
        public int SpawnPoints;
        
        [Space]

        [Header("Object and Script references")]
        [SerializeField]
        private GameObject BuildmenuPrefab;
        private GameObject buildMenu;

        [Space][SerializeField]
        private SpawnPad[] spawnPads;
        
        [Space][SerializeField]
        private GameObject[] units;
        private SpawnPad selectedPad;
        private bool uIState;

        void Start()
        {
            MakeMenu();
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

        public void SpawnUnit(GameObject unit)
        {
            if (selectedPad)
            {
                GameObject newUnit = Instantiate(unit);
                newUnit.transform.position = new Vector3(selectedPad.transform.position.x, selectedPad.transform.position.y + 0.7f, selectedPad.transform.position.z);
                toggleBuildingUI();
            }
        }

        private void MakeMenu()
        {
            buildMenu = Instantiate(BuildmenuPrefab, gameObject.transform);
            buildMenu.transform.position = new Vector3(buildMenu.transform.position.x, buildMenu.transform.position.y + 3, buildMenu.transform.position.z);
            WorldMenu buildMenuScript = buildMenu.GetComponent<WorldMenu>();
            for (int i = 0; i < units.Length; i++)
            {
                Button button = buildMenuScript.Buttons[i];
                button.gameObject.SetActive(true);
                GameObject unit = units[i];
                button.onClick.AddListener(() => { SpawnUnit(unit); });
            }
        }
    }
}
