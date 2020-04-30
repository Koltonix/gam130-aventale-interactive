using UnityEngine;
using CatGame.ControlScheme;

namespace CatGame.Controls
{
    /// <summary>
    /// Input from the Keyboard and Mouse.
    /// </summary>
    public class KeyboardInput : UserInput
    {
        [Header("Mouse")]
        private Vector3 mousePosition;

        public override void Update()
        {
            base.Update();
            RaycastFromCamera();
        }

        /// <summary>Determines whether the player has pressed the movement key.</summary>
        /// <returns>Returns true if they have pressed the key.</returns>
        public override bool IsMovementSelected()
        {
            if (Input.GetKeyDown(Keybinds.KeybindsManager.Select)) return true;
            return false;
        }

        /// <summary>Determines whether the player has pressed the combat key.</summary>
        /// <returns>Returns true if they have pressed the key.</returns>
        /*public override bool IsAttackSelected()
        {
            if (Input.GetKeyDown(Keybinds.KeybindsManager.attackSelect)) return true;
            return false;
        }*/

        public override Ray GetRay()
        {
            base.GetRay();
            return cameraRay;
        }

        public override RaycastHit GetRaycastHit()
        {
            return cameraRaycastHit;    
        }

        /// <summary>
        /// Uses the mouse position to raycast to to get the data of what
        /// the player is currently selecting with their mouse.
        /// </summary>
        public override void RaycastFromCamera()
        {
            cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(cameraRay, out cameraRaycastHit, Mathf.Infinity, layerMask);
            mousePosition = cameraRay.direction;
        }
    }

}

