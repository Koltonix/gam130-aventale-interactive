using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Data;
using CatGame.Units;

namespace CatGame.Data
{
    public class Building : Entity
    {
        [Header("Attributes")]
        private Player currentTurnPlayer;
        private Player debugOwner;

        public int SpawnPoints = 2;
        [SerializeField]
        private float uiHeight = 5f;
        
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

        protected override void Start()
        {
            base.Start();
            debugOwner = owner.GetPlayerReference();

            currentTurnPlayer = PlayerManager.Instance.GetCurrentPlayer();

            this.GetComponent<Renderer>().material.color = owner.GetPlayerReference().colour;
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
            //You cannot build a unit while one is moving
            if (TurnManager.Instance.objectIsMoving || TurnManager.Instance.objectIsAttacking) return;

            //Checking if there is a Unit above it.
            //Not ideal, but time constraints result in this.
            //Spherecast was done due to layermask still picking up default objects.
            if (selectedPad == null) return;
            Collider[] cols = Physics.OverlapSphere(selectedPad.transform.position, 1.0f);
            foreach (Collider nearbyObject in cols)
            {
                if (nearbyObject.GetComponent<Unit>() != null) return;
            }

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
            buildMenu.transform.position = new Vector3(buildMenu.transform.position.x, buildMenu.transform.position.y + uiHeight, buildMenu.transform.position.z);
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

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            if (selectedPad != null) Gizmos.DrawRay(selectedPad.transform.position, Vector3.up * 2.5f);
        }
    }  
}
