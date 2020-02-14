using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Data;

namespace CatGame.Units
{
    [RequireComponent(typeof(Mover))]
    public class Unit : MonoBehaviour
    {
        [Header("Attributes")]
        [SerializeField]
        private new string name;
        public Player owner;
        public Player currentPlayer;
        public bool IsActive;
        public Mover mover;

        void Start()
        {
            mover = this.GetComponent<Mover>();
            this.GetComponent<Renderer>().material.color = owner.colour;
            TurnManager.Instance.onPlayerCycle += OnPlayerCycle;
        }

        private void OnPlayerCycle(Player player)
        {
            if (owner.number == currentPlayer.number)
            {
                mover.ResetActionPoints();
                IsActive = true;
            }
            else
            {
                IsActive = false;
            }
        }
    }
}
