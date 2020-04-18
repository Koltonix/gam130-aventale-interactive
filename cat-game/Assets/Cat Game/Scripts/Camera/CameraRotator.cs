using System;
using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
    /// <summary>Cycles the Camera between World points.</summary>
    public class CameraRotator : CameraState
    {
        [Header("Input Settings")]
        [SerializeField]
        private KeyCode positiveButton = KeyCode.E;
        [SerializeField]
        private KeyCode negativeButton = KeyCode.Q;
        [Space]

        [Header("Camera Attributes")]
        [SerializeField]
        private Transform cameraTransform;
        [SerializeField]
        private CameraPoint[] travelPoints;
        private CameraPoint latestPoint;
        [SerializeField]
        private float moveSpeed = 1.25f;
        [SerializeField]
        private float rotateSpeed = 1.25f;
        private int pointIndex = 0;
        [Space]

        [Header("Coroutines")]
        private Coroutine movementCoroutine;
        private Coroutine rotationCoroutine;
   
        private void Start()
        {
            if (!cameraTransform) cameraTransform = this.transform;
            if (travelPoints.Length > 0) latestPoint = travelPoints[0];
        }

        #region Abstract Parent Obligations

        /// <summary>Detects Input from the User while this is active.</summary>
        public override void OnStateStay()
        {
            if (Input.GetKey(positiveButton) && movementCoroutine == null) IterateThroughPoints(1);
            else if (Input.GetKey(negativeButton) && rotationCoroutine == null) IterateThroughPoints(-1);
        }

        public override void OnStateEnter(){ }
        public override void OnStateExit() { }

        /// <summary>Checks if the current mechanic is in progress.</summary>
        /// <returns>Returns true if it is currently running.</returns>
        public override bool IsCurrentlyRunning()
        {
            if (movementCoroutine == null && rotationCoroutine == null) return false;
            else return true;
        }

        #endregion

        /// <summary>Cycles through the CameraPoints in a direction of positive or negative.</summary>
        /// <param name="direction"></param>
        private void IterateThroughPoints(int direction)
        {
            //Ensures that the direction is not zero.
            if (direction != 0)
            {
                //Sanity check to ensure it is either 1 or -1
                int absDirection = Mathf.Abs(direction);
                direction /= absDirection;

                //Ensures that the index is between the 0 and max size of the travel points
                pointIndex += direction;
                if (pointIndex > travelPoints.Length - 1) pointIndex = 0;
                else if (pointIndex < 0) pointIndex = travelPoints.Length - 1;

                UpdateCamera(travelPoints[pointIndex]);
            }
        }

        /// <summary>Moves the Camera to the new position using the CameraPoint Data.</summary>
        /// <param name="point">CameraPoint data holder.</param>
        public void UpdateCamera(CameraPoint point)
        {
            latestPoint = point;

            if (movementCoroutine == null) movementCoroutine = StartCoroutine(MoveToPoint(point.worldReference.transform, moveSpeed));
            if (rotationCoroutine == null) rotationCoroutine = StartCoroutine(RotateToPoint(point.worldReference.transform, rotateSpeed));
        }

        /// <summary>Moves the Camera to the new position using a transform.</summary>
        /// <param name="point">Transform World Position.</param>
        /// <remarks>This is used by Unity Events as Transform is serialised on the Inpsector.</remarks>
        public void UpdateCamera(Transform point)
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);

            movementCoroutine = StartCoroutine(MoveToPoint(point.transform, moveSpeed));
            rotationCoroutine = StartCoroutine(RotateToPoint(point.transform, rotateSpeed));
        }

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

        private void OnDrawGizmos()
        {
            //Connects the lines and points where the Camera cycles through to
            for (int i = 0; i < travelPoints.Length; i++)
            {
                //Drawing the Points
                Gizmos.color = new Color32(255, 113, 13, 255);
                Gizmos.DrawSphere(travelPoints[i].worldReference.transform.position, .5f);

                //Rendering the Lines
                Gizmos.color = new Color32(255, 223, 13, 255);
                if (i == travelPoints.Length - 1) Gizmos.DrawLine(travelPoints[i].worldReference.transform.position, travelPoints[0].worldReference.transform.position);
                else Gizmos.DrawLine(travelPoints[i].worldReference.transform.position, travelPoints[i + 1].worldReference.transform.position);

            }
        }
    }
}