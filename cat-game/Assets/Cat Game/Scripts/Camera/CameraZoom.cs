using System.Collections;
using UnityEngine;

namespace CatGame.CameraMovement
{
    /// <summary>
    /// Zooms the Camera in by moving the Camera Position. This **DOES NOT** change the FOV.
    /// </summary>
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

            //Calculates the Maximum Position for it to Zoom to.
            maxTargetPosition = GetMaxPositionUsingZoom(maxZoom);
            originalPosition = this.transform.position;
        }

        #region Abstract Parent Obligations

        /// <summary>
        /// Uses the Mouse Scroll Wheel to determine where to travel between the 
        /// minimum and maximum point of the Camera Zoom.
        /// </summary>
        public override void OnStateStay()
        {
            DetermineLinearPoint(Mathf.RoundToInt(Input.GetAxisRaw("SCROLL_WHEEL")));
        }

        /// <summary>Calculates the maximum positino and caches the minimum.</summary>
        public override void OnStateEnter()
        {
            originalPosition = this.transform.position;
            maxTargetPosition = GetMaxPositionUsingZoom(maxZoom);
        }

        /// <summary>Zooms back to the original point.</summary>
        public override void OnStateExit()
        {
            linearPoint = 0.0f;

            if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
            scrollCoroutine = StartCoroutine(ScrollLerp(originalPosition, maxTargetPosition, scrollLerpSpeed));
        }

        /// <summary>Checks if the Zooming Camera coroutine is running.</summary>
        /// <returns>Returns true if is running.</returns>
        public override bool IsCurrentlyRunning()
        {
            if (scrollCoroutine == null) return false;
            else return true;
        }

        #endregion

        /// <summary>Calculates the new point to lerp to base on a direction.</summary>
        /// <param name="direction">A Positive or Negative Integer.</param>
        private void DetermineLinearPoint(int direction)
        {
            if (direction != 0)
            {
                //Sanity check to ensure it is either 1 or -1
                int absDirection = Mathf.Abs(direction);
                direction /= absDirection;

                //Determines the point to lerp to
                linearPoint += (direction * scrollSpeed);

                //Contrains the linearPoint to be between 0 and 1
                if (linearPoint > 1.0f) linearPoint = 1.0f;
                else if (linearPoint <= 0) linearPoint = 0.0f;

                //Lerps to the new Zoom Point
                if (scrollCoroutine != null) StopCoroutine(scrollCoroutine);
                scrollCoroutine = StartCoroutine(ScrollLerp(originalPosition, maxTargetPosition, scrollLerpSpeed));
            }
        }

        /// <summary>Lerps the Camera Position from one position to another.</summary>
        /// <param name="startPosition">World Position Start Point.</param>
        /// <param name="endPosition">World Position End Point.</param>
        /// <param name="scrollSpeed">Speed at which the Camera linearly interpolates at.</param>
        /// <returns>NULL</returns>
        private IEnumerator ScrollLerp(Vector3 startPosition, Vector3 endPosition, float scrollSpeed)
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

        /// <summary>Calculates the Maximum World Position away to Zoom to.</summary>
        /// <param name="maxDistance">Distance Away from the Camera.</param>
        /// <returns>Max Zoom World Position.</returns>
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
