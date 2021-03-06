﻿using System.Collections;
using UnityEngine;
using CatGame.ControlScheme;

namespace CatGame.CameraMovement
{
    /// <summary>
    /// A State Handler which deals with the Zooming and Moving on the Camera
    /// between points.
    /// </summary>
    [RequireComponent(typeof(CameraZoom), typeof(CameraRotator), typeof(CameraBirdsEyeView))]
    public class CameraStateHandler : MonoBehaviour
    {
        [Header("States")]
        private CameraState rotating;
        private CameraState zooming;
        private CameraState birdsEye;
        private CameraState currentState;

        private IEnumerator changingState;

        private void Start()
        {
            rotating = this.GetComponent<CameraRotator>();
            zooming = this.GetComponent<CameraZoom>();
            birdsEye = this.GetComponent<CameraBirdsEyeView>();

            //Starts the entrance function for the first state
            changingState = ChangeState(rotating);
            StartCoroutine(changingState);
        }

        private void Update()
        {
            if (currentState) currentState.OnStateStay();

            //Checks to see what next state to go to.
            if (changingState == null)
            {
                if (Input.GetKey(Keybinds.KeybindsManager.LastCamera) || Input.GetKey(Keybinds.KeybindsManager.NextCamera) && changingState == null)
                {
                    changingState = ChangeState(rotating);
                    StartCoroutine(changingState);
                    return;
                }

                else if (Input.GetAxisRaw("SCROLL_WHEEL") != 0 && changingState == null)
                {
                    changingState = ChangeState(zooming);
                    StartCoroutine(changingState);
                    return;
                }

                else if (Input.GetKeyDown(Keybinds.KeybindsManager.BirdsEye) && changingState == null)
                {
                    changingState = ChangeState(birdsEye);
                    StartCoroutine(changingState);
                    return;
                }
            }
        }

        /// <summary>Transitions from the old state to the new state.</summary>
        /// <param name="newState">Next Camera State to go to.</param>
        /// <returns>NULL</returns>
        private IEnumerator ChangeState(CameraState newState)
        {
           // Can't go from the same state to the same
            if (currentState != newState)
            {
                //First time will always be null
                if (currentState)
                {
                    currentState.OnStateExit();

                    //Will wait until the previous state has finished
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