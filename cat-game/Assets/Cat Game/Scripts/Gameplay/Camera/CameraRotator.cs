using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
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
            latestPoint = travelPoints[0];
        }

        #region Abstract Parent Obligations

        public override void OnStateStay()
        {
            if (Input.GetKey(positiveButton) && movementCoroutine == null) IterateThroughPoints(1);
            else if (Input.GetKey(negativeButton) && rotationCoroutine == null) IterateThroughPoints(-1);
        }

        public override void OnStateEnter(){ }
        public override void OnStateExit() { }

        #endregion

        private void IterateThroughPoints(int direction)
        {
            if (direction != 0)
            {
                //Sanity check to ensure it is either 1 or -1
                int absDirection = Mathf.Abs(direction);
                direction /= absDirection;

                pointIndex += direction;
                if (pointIndex > travelPoints.Length - 1) pointIndex = 0;
                else if (pointIndex < 0) pointIndex = travelPoints.Length - 1;

                UpdateCamera(travelPoints[pointIndex]);
            }
        }

        private void UpdateCamera(CameraPoint point)
        {
            latestPoint = point;
        }

        public void OnZoomReset()
        {
            Debug.Log("MOVING");
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);

            movementCoroutine = StartCoroutine(MoveToPoint(latestPoint, moveSpeed));
            rotationCoroutine = StartCoroutine(RotateToPoint(latestPoint, rotateSpeed));
        }

        private IEnumerator MoveToPoint(CameraPoint point, float moveSpeed)
        {
            Vector3 targetPoint = point.worldTransform.position;

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

        private IEnumerator RotateToPoint(CameraPoint point, float rotateSpeed)
        {
            Quaternion targetRotation = GetRotationFromCameraPoint(point);

            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime * rotateSpeed;
                cameraTransform.rotation = Quaternion.Lerp(cameraTransform.rotation, targetRotation, t);

                yield return new WaitForEndOfFrame();
            }

            rotationCoroutine = null;
            yield return null;
        }

        private Quaternion GetRotationFromCameraPoint(CameraPoint point)
        {
            if (point.useObjectRotation) return point.worldTransform.rotation;
            return Quaternion.Euler(point.rotation);
        }
    }
}