using UnityEngine;

namespace SiegeOfAshes.Controls
{
    public abstract class UserInput : MonoBehaviour
    {
        [Header("Raycast")]
        public Ray cameraRay;
        public RaycastHit cameraRaycastHit;
        private Vector3 debugRayHitPoint = Vector3.zero;

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

        public void DebugDrawRay()
        {
            if (cameraRaycastHit.collider != null) Debug.DrawLine(cameraRay.origin, cameraRaycastHit.point, Color.red);
            else Debug.DrawLine(cameraRay.origin, cameraRay.direction.normalized * 100f, Color.red);
        }
    }
}

