using System;
using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
    public class CameraBirdsEyeView : CameraState
    {
        [SerializeField]
        private float moveSpeed = 1.25f;
        [SerializeField]
        private float rotationSpeed = 1.25f;
        [SerializeField]
        private Transform birdsEyePoint;
        [SerializeField]
        private Transform cameraTransform;

        private Coroutine movementCoroutine;
        private Coroutine rotationCoroutine;

        private void Start()
        {
            if (!cameraTransform) cameraTransform = this.transform;
        }

        public override void OnStateStay() { }
        public override void OnStateEnter() 
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);

            movementCoroutine = StartCoroutine(MoveToPoint(birdsEyePoint, moveSpeed));
            rotationCoroutine = StartCoroutine(RotateToPoint(birdsEyePoint, rotationSpeed));
        }

        public override void OnStateExit() { }

        /// <summary>Linearly moves the Camera to the new point.</summary>
        /// <param name="point">New World Point to move to.</param>
        /// <param name="moveSpeed">Speed at which to move at.</param>
        /// <returns>NULL</returns>
        private IEnumerator MoveToPoint(Transform point, float moveSpeed)
        {
            if (point == null) throw new Exception("ASSIGN CAMERA POINTS IN THE INSPECTOR");
            Vector3 targetPoint = point.position;

            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime * moveSpeed;
                cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPoint, t);

                yield return new WaitForEndOfFrame();
            }

            movementCoroutine = null;
            yield return null;
        }

        /// <summary>Linearly Rotates to the new point.</summary>
        /// <param name="point">New World Point to Rotate to.</param>
        /// <param name="rotateSpeed">Speed at which to rotate at.</param>
        /// <returns>NULL</returns>
        private IEnumerator RotateToPoint(Transform point, float rotateSpeed)
        {
            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime * rotateSpeed;
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, point.rotation, t);

                yield return new WaitForEndOfFrame();
            }

            rotationCoroutine = null;
            yield return null;
        }
    }
}

