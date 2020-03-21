using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
    public class CameraRotator : MonoBehaviour
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
        }

        private void Update()
        {
            if (Input.GetKeyDown(positiveButton) && movementCoroutine == null) IterateThroughPoints(1);
            else if (Input.GetKeyDown(negativeButton) && rotationCoroutine == null) IterateThroughPoints(-1);
        }

        private void IterateThroughPoints(int direction)
        {
            //Sanity check to ensure it is either 1 or -1
            int absDirection = Mathf.Abs(direction);
            direction = direction / absDirection;

            pointIndex += direction;
            if (pointIndex > travelPoints.Length - 1) pointIndex = 0;
            else if (pointIndex < 0) pointIndex = travelPoints.Length - 1;

            UpdateCamera(travelPoints[pointIndex]);
        }

        private void UpdateCamera(CameraPoint point)
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);

            movementCoroutine = StartCoroutine(MoveToPoint(point, moveSpeed));
            rotationCoroutine = StartCoroutine(RotateToPoint(point, rotateSpeed));
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