using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CatGame.Data;

namespace CatGame.UI
{
    // This is the ingame UI script.
    public class UIIngame : MonoBehaviour
    {
        private UIController uIController;
        private Player currentPlayer;
        private Text aPCounter;        

        // Start is called before the first frame update
        void Start()
        {
            uIController = GameObject.FindObjectOfType<UIController>();
            currentPlayer = PlayerManager.Instance.GetCurrentPlayer();
            currentPlayer.onAP += APCounter;
            APCounter(currentPlayer.ActionPoints);
            TurnManager.Instance.onPlayerCycle += PlayerCycle;
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

        public void Flag()
        {
            uIController.ToggleCatopedia();
        }

        public void PlayerCycle(Player player)
        {
            currentPlayer.onAP -= APCounter;
            currentPlayer = player;
            currentPlayer.onAP += APCounter;
            APCounter(currentPlayer.ActionPoints);
        }
    }
}