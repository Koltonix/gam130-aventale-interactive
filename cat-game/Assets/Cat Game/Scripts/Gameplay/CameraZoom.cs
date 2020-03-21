using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
    [RequireComponent(typeof(Camera))]
    public class CameraZoom : MonoBehaviour
    {
        [Header("Camera Attributes")]
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private float maxZoom = 5.0f;
        [Space]

        [Header("Position")]
        private float linearPoint = 0.0f;
        private Vector3 originalPosition;
        private Vector3 maxTargetPosition;

        private void Start()
        {
            if (!mainCamera) mainCamera = Camera.main;

            maxTargetPosition = GetMaxPositionUsingZoom(maxZoom);
            originalPosition = this.transform.position;
        }

        private void Update()
        {
            maxTargetPosition = GetMaxPositionUsingZoom(maxZoom);
            transform.position = Vector3.Lerp(originalPosition, maxTargetPosition, linearPoint);
            ZoomCamera(Mathf.RoundToInt(Input.GetAxisRaw("SCROLL_WHEEL")));
        }

        private void ZoomCamera(int direction)
        {
            if (direction != 0)
            {
                //Sanity check to ensure it is either 1 or -1
                int absDirection = Mathf.Abs(direction);
                direction /= absDirection;

                linearPoint += (direction * 0.1f);
                if (linearPoint > 1.0f) linearPoint = 1.0f;
                else if (linearPoint <= 0) linearPoint = 0.0f;
            }
        }

        private Vector3 GetMaxPositionUsingZoom(float maxDistance)
        {
            return transform.forward * maxDistance + transform.position;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(maxTargetPosition, 2.5f);
        }
    }
}
