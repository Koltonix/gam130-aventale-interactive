using UnityEngine;

namespace CatGame.CameraMovement
{
    [RequireComponent(typeof(CameraZoom), typeof(CameraRotator))]
    public class CameraStateHandler : MonoBehaviour
    {
        [Header("States")]
        private CameraState rotating;
        private CameraState zooming;
        private CameraState currentState;

        private void Start()
        {
            rotating = this.GetComponent<CameraRotator>();
            zooming = this.GetComponent<CameraZoom>();

            ChangeState(zooming);
        }

        private void Update()
        {
            currentState.OnStateStay();
        }

        private void ChangeState(CameraState newState)
        {
            //Run the previous state
            if (currentState) currentState.OnStateExit();

            //Run the new state
            currentState = newState;
            currentState.OnStateEnter();
        }

    }
}