using System.Collections;
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

        private IEnumerator changingState;

        private void Start()
        {
            rotating = this.GetComponent<CameraRotator>();
            zooming = this.GetComponent<CameraZoom>();

            changingState = ChangeState(rotating);
            StartCoroutine(changingState);
        }

        private void Update()
        {
            if (currentState) currentState.OnStateStay();

            if (changingState == null)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                {
                    changingState = ChangeState(rotating);
                    StartCoroutine(changingState);
                }

                else if (Input.GetAxisRaw("SCROLL_WHEEL") != 0)
                {
                    changingState = ChangeState(zooming);
                    StartCoroutine(changingState);
                }
            }
        }

        private IEnumerator ChangeState(CameraState newState)
        {
           // Can't go from the same state to the same
            if (currentState != newState)
            {
                //First time will always be null
                if (currentState)
                {
                    currentState.OnStateExit();

                    while (currentState.IsCurrentlyRunning())
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

                //Run the new state
                currentState = newState;
                newState.OnStateEnter(); 
            }

            changingState = null;
            yield return null;
        }

    }
}