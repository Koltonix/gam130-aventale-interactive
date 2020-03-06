using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatGame.UI
{
    public class FlyCamera : MonoBehaviour
    {
        [SerializeField][Range(0.1f, 1f)]
        float camSpeed = 0.4f;
        [SerializeField][Range(0.1f, 4f)]
        float camSprintSpeed = 5f;
        [SerializeField][Range(0.1f, 10f)]
        float camSens = 3f; //How sensitive it with mouse
        float maxYAngle = 80f;
        Vector2 currentRotation;
        private float totalRun = 1.0f;

        void Update()
        {
            if (Input.GetMouseButton(1))
            {
                currentRotation.x += Input.GetAxis("Mouse X") * camSens;
                currentRotation.y -= Input.GetAxis("Mouse Y") * camSens;
                currentRotation.x = Mathf.Repeat(currentRotation.x, 360);
                currentRotation.y = Mathf.Clamp(currentRotation.y, -maxYAngle, maxYAngle);
                Camera.main.transform.rotation = Quaternion.Euler(currentRotation.y, currentRotation.x, 0);
            }

            if (Input.GetMouseButtonDown(1))
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            if (Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            Vector3 cameraInput = CameraInput();

            if (Input.GetButton("Sprint"))
            {
                Camera.main.transform.position += Camera.main.transform.forward * cameraInput.x * camSprintSpeed;
                Camera.main.transform.position += Camera.main.transform.right * cameraInput.y * camSprintSpeed;
                Camera.main.transform.position += Camera.main.transform.up * cameraInput.z * camSprintSpeed;
            }
            else
            {
                Camera.main.transform.position += Camera.main.transform.forward * cameraInput.x * camSpeed;
                Camera.main.transform.position += Camera.main.transform.right * cameraInput.y * camSpeed;
                Camera.main.transform.position += Camera.main.transform.up * cameraInput.z * camSpeed;
            }
        }

        Vector3 CameraInput()
        {
            Vector3 returnVector = new Vector3(0, 0, 0);
            if (Input.GetKey(KeyCode.E))
            {
                returnVector.z += 1;
            }
            if (Input.GetKey(KeyCode.Q))
            {
                returnVector.z -= 1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                returnVector.x += 1;
            }
            if (Input.GetKey(KeyCode.S))
            {
                returnVector.x -= 1;
            }
            if (Input.GetKey(KeyCode.A))
            {
                returnVector.y -= 1;
            }
            if (Input.GetKey(KeyCode.D))
            {
                returnVector.y += 1;
            }
            return returnVector;
        }
    }
}