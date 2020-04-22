using UnityEngine;
using CatGame.Data;

namespace CatGame.Units
{
    /// <summary>Stores the extra metadata of the Unit.</summary>
    [RequireComponent(typeof(UnitMovement))]
    public class Unit : Entity, IUnitData
    {
        [Header("Attributes")]
        [SerializeField]
        private new string name;
        [SerializeField]
        private new Renderer renderer;

        protected override void Start()
        {
            base.Start();
            renderer.material.color = owner.colour;

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
