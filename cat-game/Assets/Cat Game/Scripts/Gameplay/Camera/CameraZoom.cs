using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
    public class CameraZoom : CameraState
    {
        [Header("Camera Attributes")]
        [SerializeField]
        private Camera mainCamera;
        [SerializeField]
        private float maxZoom = 5.0f;
        [Space]

        [Header("Position")]
        [SerializeField]
        private float moveSpeed = 1.25f;
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

            coroutines = new Coroutine[1] { scrollCoroutine };
        }

        #region Abstract Parent Obligations

        public override void OnStateStay()
        {
            DetermineLinearPoint(Mathf.RoundToInt(Input.GetAxisRaw("SCROLL_WHEEL")));
        }

        public override void OnStateEnter()
        {
            originalPosition = this.transform.position;
            maxTargetPosition = GetMaxPositionUsingZoom(maxZoom);
        }

        public override void OnStateExit()
        {
            linearPoint = 0.0f;

            if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
            scrollCoroutine = StartCoroutine(ScrollLerp(originalPosition, maxTargetPosition, moveSpeed, scrollLerpSpeed));
        }

        #endregion

        private void DetermineLinearPoint(int direction)
        {
            if (direction != 0)
            {
                //Sanity check to ensure it is either 1 or -1
                int absDirection = Mathf.Abs(direction);
                direction /= absDirection;

                linearPoint += (direction * scrollSpeed);
                if (linearPoint > 1.0f) linearPoint = 1.0f;
                else if (linearPoint <= 0) linearPoint = 0.0f;

                if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
                scrollCoroutine = StartCoroutine(ScrollLerp(originalPosition, maxTargetPosition, moveSpeed, scrollLerpSpeed));
            }
        }

        private IEnumerator ScrollLerp(Vector3 startPosition, Vector3 endPosition, float moveSpeed, float scrollSpeed)
        {
            float t = 0.0f;
            while (t <= 1.0f)
            {
                t += Time.deltaTime * scrollSpeed;
                lerpingLinearPoint = Mathf.Lerp(lerpingLinearPoint, linearPoint, t);
                transform.position = Vector3.Lerp(startPosition, endPosition, lerpingLinearPoint);

                yield return new WaitForEndOfFrame();
            }

            scrollCoroutine = null;
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
