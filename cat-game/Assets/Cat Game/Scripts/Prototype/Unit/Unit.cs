using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Data;

namespace CatGame.Units
{
    [RequireComponent(typeof(UnitMovement))]
    public class Unit : Entity, IUnitData
    {
        [Header("Attributes")]
        [SerializeField]
        private new string name;

        protected override void Start()
        {
            base.Start();
            this.GetComponent<Renderer>().material.color = owner.colour;

            if (name == null) name = "UNNAMED_UNIT";
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
