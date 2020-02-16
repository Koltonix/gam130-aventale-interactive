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

        [Header("Global Settings")]
        private ITurn turnData;

        private void Start()
        {
            turnData = TurnManager.Instance;
            owner = PlayerManager.Instance.GetCurrentPlayer();

            this.GetComponent<Renderer>().material.color = owner.GetPlayerReference().colour;
            turnData.AddToListener += OnTurnEnd;
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
