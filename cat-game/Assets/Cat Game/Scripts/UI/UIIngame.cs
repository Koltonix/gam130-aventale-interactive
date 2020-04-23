using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Data;
using CatGame.Units;
using TMPro;

namespace CatGame.UI
{
    // This is the ingame UI script.
    public class UIIngame : MonoBehaviour
    {
        private UIController uIController;
        private Player currentPlayer;
        [SerializeField]
        private TextMeshProUGUI aPCounter;
        [SerializeField]
        private TextMeshProUGUI unitCounter;

        // Start is called before the first frame update
        void Start()
        {
            uIController = GameObject.FindObjectOfType<UIController>();
            currentPlayer = PlayerManager.Instance.GetCurrentPlayer();
            TurnManager.Instance.onPlayerCycle += PlayerCycle;
            UpdateUI();
        }

        void Update()
        {
            UnitCounter();
        }

        public void Menu()
        {
            uIController.TogglePauseMenu();
        }

        public void Scores()
        {

        }

        public void APCounter(int AP)
        {
            aPCounter.text = AP.ToString() + " AP";
        }

        public void UnitCounter()
        {
            unitCounter.text =  currentPlayer.PlayerUnits.Count.ToString() + " / " + currentPlayer.unitCap.ToString();
        }

        public void Flag()
        {
            uIController.ToggleCatopedia();
        }

        public void PlayerCycle(Player player)
        {
            currentPlayer.onAP -= APCounter;
            currentPlayer = player;
            UpdateUI();
        }

        private void UpdateUI()
        {
            currentPlayer.onAP += APCounter;
            APCounter(currentPlayer.ActionPoints);
        }
    }
}