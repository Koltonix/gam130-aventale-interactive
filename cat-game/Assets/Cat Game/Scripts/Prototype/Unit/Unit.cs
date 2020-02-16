using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Data;

namespace CatGame.Units
{
    [RequireComponent(typeof(UnitMovement))]
    public class Unit : MonoBehaviour, IUnitData
    {
        [Header("Attributes")]
        [SerializeField]
        private new string name;

        [Header("Player Settings")]
        public IPlayerData owner;
        public Player currentPlayer;

        private void Start()
        {
            owner = GameController.Instance.GetCurrentPlayer();

            this.GetComponent<Renderer>().material.color = owner.GetPlayerReference().colour;
            GameController.Instance.onPlayerCycle += OnTurnEnd;
        }

        private void OnTurnEnd(Player player)
        {
            currentPlayer = player;
        }

        #region Contractual Obligations
        public Player GetOwner()
        {
            return owner.GetPlayerReference();
        }

        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }
        #endregion
    }
}
