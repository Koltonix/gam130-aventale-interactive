using UnityEngine;
using UnityEngine.UI;

namespace SiegeOfAshes.Controls
{
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

        public override bool HasClicked()
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

        public override void RaycastFromCamera()
        {
            controllerIcon.transform.position = new Vector3(controllerIcon.transform.position.x + (horizontalAxis.value * axisSensitivity),
                                                            controllerIcon.transform.position.y + (verticalAxis.value * axisSensitivity),
                                                            0);

            cameraRay = mainCamera.ScreenPointToRay(controllerIcon.transform.position);
            Physics.Raycast(cameraRay, out cameraRaycastHit);
        }

        private void AccessInputData()
        {
            horizontalAxis.value = Input.GetAxis(horizontalAxis.name);
            verticalAxis.value = Input.GetAxis(verticalAxis.name);
            enterButton.value = Input.GetAxis(enterButton.name);

            HasClicked();
        }


    }
}

