using UnityEngine;

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

        [Header("Ray Settings")]
        private Vector3 cursorScreenPosition = new Vector3(0, 2.5f, 0);

        public override void Update()
        {
            AccessInputData();
            MoveCursor();
        }

        public override bool HasClicked()
        {
            if (Input.GetButtonDown(enterButton.name)) return true;
            return false;
        }

        public void MoveCursor()
        {
            cursorScreenPosition.x += horizontalAxis.value * axisSensitivity;
            cursorScreenPosition.z += verticalAxis.value * axisSensitivity;
        }

        public override Ray GetRay()
        {
            return base.GetRay();
        }

        public override RaycastHit GetRaycastHit()
        {
            return base.GetRaycastHit();
        }

        private void AccessInputData()
        {
            horizontalAxis.value = Input.GetAxis(horizontalAxis.name);
            verticalAxis.value = Input.GetAxis(verticalAxis.name);
            enterButton.value = Input.GetAxis(enterButton.name);

            HasClicked();
        }

        private void OnDrawGizmos()
        {
            //Gizmos.color = new Vector4(44, 199, 79, 128);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(cursorScreenPosition, .5f);
        }
    }
}

