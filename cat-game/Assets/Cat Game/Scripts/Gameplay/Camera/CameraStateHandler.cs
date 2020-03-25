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

        private Coroutine changingState;

        private void Start()
        {
            rotating = this.GetComponent<CameraRotator>();
            zooming = this.GetComponent<CameraZoom>();

            changingState = StartCoroutine(ChangeState(rotating));
        }

        private void Update()
        {
            Debug.Log(changingState == null);
            if (changingState == null)
            {
                if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.E))
                {
                    changingState = StartCoroutine(ChangeState(rotating));
                }

                else if (Input.GetAxisRaw("SCROLL_WHEEL") != 0)
                {
                    changingState = StartCoroutine(ChangeState(zooming));
                }
            }

            if (currentState) currentState.OnStateStay();
        }

        private IEnumerator ChangeState(CameraState newState)
        {
            
           
            
            //Can't go from the same state to the same
            if (currentState != newState)
            {
                //First time will always be null
                if (currentState)
                {
                    currentState.OnStateExit();

                    //while (currentState.IsCurrentlyRunning())
                    //{
                    //    Debug.Log("WAITING");
                    //    yield return new WaitForEndOfFrame();
                    //}
                }

                //Run the new state
                currentState = newState;
                currentState.OnStateEnter();

            }      

            changingState = null;
            yield return null;
        }

    }
}