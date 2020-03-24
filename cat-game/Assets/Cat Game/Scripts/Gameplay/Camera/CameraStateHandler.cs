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

            ChangeState(rotating);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E)) ChangeState(rotating);
            else if (Input.GetAxisRaw("SCROLL_WHEEL") != 0) ChangeState(zooming);

            currentState.OnStateStay();
        }

        private bool lastIsRunning;

        private void ChangeState(CameraState newState)
        {
            //Not transitioning to the same state
            if (currentState != newState)
            {
                //First will always be null
                if (currentState)
                {
                    currentState.OnStateExit();

                    do
                    {
                        lastIsRunning = false;

                        for (int i = 0; i < currentState.coroutines.Length; i++)
                        {
                            if (currentState.coroutines[i] != null) lastIsRunning = true;
                            Debug.Log(currentState.coroutines[i]);
                        }

                    } while (lastIsRunning);
                }

                //Run the new state
                currentState = newState;
                currentState.OnStateEnter();
            }
            
        }

    }
}