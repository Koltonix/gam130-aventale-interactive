using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Data;
using CatGame.Combat;

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

        [Header("Unit Attributes")]
        [SerializeField]
        [Range(0.1f, 10f)]
        public float MovementModifier = 1;
        private Attacker attackType;

        [Header("Model Renderer")]
        [SerializeField]
        private Renderer renderer;

        private void Start()
        {
            turnData = TurnManager.Instance;
            owner = PlayerManager.Instance.GetCurrentPlayer();

            attackType = gameObject.GetComponent<Attacker>();

            renderer.material.color = owner.GetPlayerReference().colour;
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
