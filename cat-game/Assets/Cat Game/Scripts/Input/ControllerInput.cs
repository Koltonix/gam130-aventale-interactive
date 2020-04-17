using UnityEngine;
using UnityEngine.UI;

namespace CatGame.Controls
{
    /// <summary>
    /// Input from a traditional Xbox Controller.
    /// </summary>
    public class ControllerInput : UserInput
    {
        [Header("Controller Axis")]
        [SerializeField]
        private float axisSensitivity;
        [SerializeField]
        private InputName horizontalAxis;
        [SerializeField]
        private InputName verticalAxis;

        [Header("Controller Buttons")]
        [SerializeField]
        private InputName enterButton;

        [Header("Debug Settings")]
        [SerializeField]
        private Image controllerIcon;

        public override void Update()
        {
            base.Update();
            AccessInputData();
            RaycastFromCamera();
        }

        /// <summary>Checks to see if the movement button has been clicked.</summary>
        /// <returns>Returns true if the button has been pressed.</returns>
        public override bool IsMovementSelected()
        {
            if (Input.GetButtonDown(enterButton.name)) return true;
            return false;
        }

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
        /// Raycasts from the Main Camera using the position of the Controller Icon which is moved
        /// using the thumbsticks.
        /// </summary>
        public override void RaycastFromCamera()
        {
            controllerIcon.transform.position = new Vector3(controllerIcon.transform.position.x + (horizontalAxis.value * axisSensitivity),
                                                            controllerIcon.transform.position.y + (verticalAxis.value * axisSensitivity),
                                                            0);

            cameraRay = mainCamera.ScreenPointToRay(controllerIcon.transform.position);
            Physics.Raycast(cameraRay, out cameraRaycastHit);
        }

        /// <summary>Retrieves all of the Controller Input Values from the Input Manager.</summary>
        private void AccessInputData()
        {
            horizontalAxis.value = Input.GetAxis(horizontalAxis.name);
            verticalAxis.value = Input.GetAxis(verticalAxis.name);
            enterButton.value = Input.GetAxis(enterButton.name);

            IsMovementSelected();
        }


    }
}

