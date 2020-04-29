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
        private Renderer[] renderers;

        [Header("Animations")]
        public Animator anim;
        public Rigidbody rigidBody;

        protected override void Start()
        {
            base.Start();
            if (renderers != null) ChangeRendererColour(owner.colour, renderers);

            if (name.Length == 0) name = "UNNAMED_UNIT";
        }

        private void ChangeRendererColour(Color32 colour, Renderer[] renderers)
        {
            foreach (Renderer renderer in renderers)
            {
                renderer.material.color = colour;
            }
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
