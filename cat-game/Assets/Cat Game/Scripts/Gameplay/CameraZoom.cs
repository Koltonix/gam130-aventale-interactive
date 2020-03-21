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
        private Vector3 lastPosition;
        private Vector3 maxTargetPosition;

        private void Start()
        {
            if (!mainCamera) mainCamera = Camera.main;

            lastPosition = this.transform.position;
            maxTargetPosition = GetMaxPositionUsingZoom(maxZoom);
        }

        private void Update()
        {
            transform.position = Vector3.Lerp(lastPosition, maxTargetPosition, linearPoint);
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

        private Vector3 GetMaxPositionUsingZoom(float maxZoom)
        {
            return transform.forward * maxZoom;
        }
    }
}
