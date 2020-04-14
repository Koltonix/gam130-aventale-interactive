using System;
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
            if (travelPoints.Length > 0) latestPoint = travelPoints[0];
        }

        #region Abstract Parent Obligations

        public override void OnStateStay()
        {
            if (Input.GetKey(positiveButton) && movementCoroutine == null) IterateThroughPoints(1);
            else if (Input.GetKey(negativeButton) && rotationCoroutine == null) IterateThroughPoints(-1);
        }

        public override void OnStateEnter(){ }
        public override void OnStateExit() { }

        public override bool IsCurrentlyRunning()
        {
            if (movementCoroutine == null && rotationCoroutine == null) return false;
            else return true;
        }

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

        public void UpdateCamera(CameraPoint point)
        {
            latestPoint = point;

            if (movementCoroutine == null) movementCoroutine = StartCoroutine(MoveToPoint(point.worldReference.transform, moveSpeed));
            if (rotationCoroutine == null) rotationCoroutine = StartCoroutine(RotateToPoint(point.worldReference.transform, rotateSpeed));
        }

        public void UpdateCamera(Transform point)
        {
            if (movementCoroutine != null) StopCoroutine(movementCoroutine);
            if (rotationCoroutine != null) StopCoroutine(rotationCoroutine);

            movementCoroutine = StartCoroutine(MoveToPoint(point.transform, moveSpeed));
            rotationCoroutine = StartCoroutine(RotateToPoint(point.transform, rotateSpeed));
        }

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