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
        public bool isActive;

        [Header("Player Settings")]
        public Player owner;
        public Player currentPlayer;

        [Header("Movement")]
        public UnitMovement unitMovement;

        private void Start()
        {
            unitMovement = this.GetComponent<UnitMovement>();
            this.GetComponent<Renderer>().material.color = owner.colour;

            TurnManager.Instance.onPlayerCycle += OnPlayerCycle;
        }

        private void OnPlayerCycle(Player player)
        {
            if (owner == currentPlayer) isActive = true;
            else isActive = false;
        }

        #region Contractual Obligations
        public Player GetOwner()
        {
            return owner;
        }

        public Player GetCurrentPlayer()
        {
            return currentPlayer;
        }
        #endregion
    }
}
