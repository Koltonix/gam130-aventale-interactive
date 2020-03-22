using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
    [RequireComponent(typeof(Camera), typeof(CameraRotator))]
    public class CameraZoom : MonoBehaviour
    {
        [Header("Camera Attributes")]
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private float maxZoom = 5.0f;
        [Space]

        [Header("Position")]
        [SerializeField]
        private float scrollLerpSpeed = 1.25f;
        [SerializeField]
        [Range(0.0f, 1.0f)]
        private float scrollSpeed = 0.2f;
        private float linearPoint = 0.0f;
        private float lerpingLinearPoint = 0.0f;

        private Coroutine scrollCoroutine;

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
            transform.position = Vector3.Lerp(originalPosition, maxTargetPosition, lerpingLinearPoint);
            ZoomCamera(Mathf.RoundToInt(Input.GetAxisRaw("SCROLL_WHEEL")));
        }

        private void ZoomCamera(int direction)
        {
            if (direction != 0)
            {
                //Sanity check to ensure it is either 1 or -1
                int absDirection = Mathf.Abs(direction);
                direction /= absDirection;

                linearPoint += (direction * scrollSpeed);
                if (linearPoint > 1.0f) linearPoint = 1.0f;
                else if (linearPoint <= 0) linearPoint = 0.0f;
            }

            if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
            scrollCoroutine = StartCoroutine(ScrollLerp(scrollLerpSpeed));
        }

        private IEnumerator ScrollLerp(float scrollSpeed)
        {
            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime * scrollSpeed;
                lerpingLinearPoint = Mathf.Lerp(lerpingLinearPoint, linearPoint, t);

                yield return new WaitForEndOfFrame();
            }

            yield return null;
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
