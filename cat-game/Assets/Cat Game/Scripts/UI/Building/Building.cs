using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Data;
using CatGame.Units;

namespace CatGame.UI
{
    public class Building : MonoBehaviour
    {
        [Header("Attributes")]
        //This index is currently needed to get reference to the correct instance of the Player class, not created
        //a new one entirely. This will be fixed when you are able to place buildings using the mouse since we
        //can assign a reference to the class from there.
        [SerializeField]
        private int playerIndex;
        private IPlayerData owner;
        private IPlayerManager globalPlayerData;
        private ITurn turnData;

        private Player currentTurnPlayer;
        private Player debugOwner;

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
            globalPlayerData = PlayerManager.Instance;
            turnData = TurnManager.Instance;

            owner = globalPlayerData.GetPlayerFromIndex(playerIndex);
            debugOwner = owner.GetPlayerReference();

            currentTurnPlayer = globalPlayerData.GetCurrentPlayer();

            SpawnPoints = 2;
            this.GetComponent<Renderer>().material.color = owner.GetPlayerReference().colour;
            turnData.AddToListener += ChangePlayer;
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
            if (owner == currentTurnPlayer)
            {
                uIState = !uIState;
                setBuildingUI(uIState);
            }
        }

        void OnMouseDown()
        {
            if (owner == currentTurnPlayer)
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
            if (selectedPad && SpawnPoints > 0 && (currentTurnPlayer.PlayerUnits.Count <= currentTurnPlayer.unitCap))
            {
                SpawnPoints--;
                GameObject newUnit = Instantiate(unit);
                //newUnit.GetComponent<Unit>().owner = owner;
                newUnit.transform.position = new Vector3(selectedPad.transform.position.x, selectedPad.transform.position.y + 0.7f, selectedPad.transform.position.z);
                toggleBuildingUI();
                currentTurnPlayer.PlayerUnits.Add(newUnit.GetComponent<Unit>());
            }
        }

        private void MakeMenu()
        {
            buildMenu = Instantiate(BuildmenuPrefab, gameObject.transform);
            buildMenu.transform.position = new Vector3(buildMenu.transform.position.x, buildMenu.transform.position.y + 3, buildMenu.transform.position.z);
            BuildMenu buildMenuScript = buildMenu.GetComponent<BuildMenu>();
            buildMenuScript.GenerateButtons(units);
            for (int i = units.Length-1; i >= 0; i--)
            {
                Button button = buildMenuScript.Buttons[i];
                button.gameObject.SetActive(true);
                GameObject unit = units[i];
                button.onClick.AddListener(() => { SpawnUnit(unit); });
                Text text = button.gameObject.GetComponentInChildren<Text>();
                text.text = "Build " + unit.name;
            }
        }

        public void ChangePlayer(Player newPlayer)
        {
            currentTurnPlayer = newPlayer;

            SpawnPoints = 2;
            setBuildingUI(false);
        }
    }  
}
