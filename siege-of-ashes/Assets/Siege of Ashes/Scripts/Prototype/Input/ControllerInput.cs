using UnityEngine;

namespace SiegeOfAshes.Controls
{
    public class ControllerInput : UserInput
    {
        public float horizontalJoystick;
        public float aButton;

        public override void Update()
        {
            horizontalJoystick = Input.GetAxis("JOYSTICK_AXIS_HORIZONTAL");
            aButton = Input.GetAxis("JOYSTICK_BUTTON_A");
        }
    }
}

