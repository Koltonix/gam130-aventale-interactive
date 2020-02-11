using UnityEngine;

namespace SiegeOfAshes.Controls
{
    public abstract class UserInput : MonoBehaviour
    {
        [Header("Camera")]
        public Camera mainCamera;

        [Header("Raycast")]
        public Ray cameraRay;
        public RaycastHit cameraRaycastHit;

        public virtual void Start()
        {
            if (mainCamera == null) mainCamera = Camera.main;
        }

        public virtual void Update()
        {
            DebugDrawRay();
        }

        public virtual bool HasClicked()
        {
            return false;
        }
        
        public virtual Ray GetRay()
        { 
            return cameraRay;
        }

        public virtual RaycastHit GetRaycastHit()
        {
            return new RaycastHit();
        }

        public virtual void RaycastFromCamera()
        {

        }

        public void DebugDrawRay()
        {
            if (cameraRaycastHit.collider != null) Debug.DrawLine(cameraRay.origin, cameraRaycastHit.point, Color.red);
            else Debug.DrawLine(cameraRay.origin, cameraRay.direction.normalized * 100f, Color.red);
        }
    }
}

