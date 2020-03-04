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

        protected override void Awake()
        {
            base.Awake();
            this.GetComponent<Renderer>().material.color = owner.colour;

            if (name.Length == 0) name = "UNNAMED_UNIT";
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
