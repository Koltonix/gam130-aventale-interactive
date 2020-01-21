﻿using UnityEngine;

namespace SiegeOfAshes.Controls
{
    public class KeyboardInput : UserInput
    {
        [Header("Mouse")]
        private Vector3 mousePosition;

        public override void Update()
        {
            base.Update();
            RaycastFromCamera();
        }

        /// <summary>
        /// Determines whether the player has clicked the mouse or not
        /// </summary>
        /// <returns>
        /// Returns true if the player has clicked, otherwise  it is false
        /// </returns>
        public override bool HasClicked()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0)) return true;
            return false;
        }

        /// <summary>
        /// Returns the ray to fulfill the contract set out by the IGetInput
        /// interface
        /// </summary>
        /// <returns>
        /// Returns the Ray from the main scene camera
        /// </returns>
        public override Ray GetRay()
        {
            base.GetRay();
            return cameraRay;
        }

        /// <summary>
        /// Returns the raycast that hit the object to fulfill the contract set
        /// out by the IGetInput interface
        /// </summary>
        /// <returns>
        /// Returns the RaycastHit that the the camera is receiving from the Ray
        /// </returns>
        public override RaycastHit GetRaycastHit()
        {
            return cameraRaycastHit;
        }

        /// <summary>
        /// Uses the mouse position to raycast to to get the data of what
        /// the player is currently selecting with their mouse
        /// </summary>
        public override void RaycastFromCamera()
        {
            cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(cameraRay, out cameraRaycastHit);
            mousePosition = cameraRay.direction;
        }
    }

}

