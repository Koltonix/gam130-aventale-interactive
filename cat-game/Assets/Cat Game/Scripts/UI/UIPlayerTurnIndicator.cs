using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Data;
using TMPro;

namespace CatGame.UI
{
    public class UIPlayerTurnIndicator : MonoBehaviour
    {
        TextMeshProUGUI currentPlayerDisplay;
        private void Start()
        {
            currentPlayerDisplay = gameObject.GetComponent<TextMeshProUGUI>();
            TurnManager.Instance.onPlayerCycle += TurnEnd;
        }

        public void TurnEnd(Player currentPlayer)
        {
            currentPlayerDisplay.text = ("It's Player " + (currentPlayer.number + 1).ToString() + "'s turn");
        }
    }
}
