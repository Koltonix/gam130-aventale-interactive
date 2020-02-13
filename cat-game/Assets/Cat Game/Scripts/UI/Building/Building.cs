using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Data;
using CatGame.Movement;

namespace CatGame.UI
{
    public class Building : MonoBehaviour
    {

        [Header("Attributes")]
        [SerializeField]
        private Player owner;
        public int CurrentPlayer;
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
            this.GetComponent<Renderer>().material.color = owner.colour;
            TurnManager.Instance.onPlayerCycle += ChangePlayer;
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
            if (owner.number == CurrentPlayer)
            {
                uIState = !uIState;
                setBuildingUI(uIState);
            }
        }

        void OnMouseDown()
        {
            if (owner.number == CurrentPlayer)
            {
                toggleBuildingUI();
            }
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
                newUnit.GetComponent<Unit>().currentPlayer = owner;
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

        /// <summary>
        /// This needs fixing when we get around to working on the turn-change system.
        /// </summary>
        /// <param name="newPlayer"></param>
        public void ChangePlayer(Player newPlayer)
        {
            setBuildingUI(false);
            if (CurrentPlayer == 0)
            {
                CurrentPlayer = 1;
            }
            else
            {
                CurrentPlayer = 0;
            }
        }
    }  
}
