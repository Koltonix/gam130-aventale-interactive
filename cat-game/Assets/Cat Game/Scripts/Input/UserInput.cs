﻿using UnityEngine;

namespace CatGame.Controls
{
    /// <summary>
    /// Template for the Input for all of the Input Values requried for
    /// the game.
    /// </summary>
    public abstract class UserInput : MonoBehaviour, IGetOnClick
    {
        [Header("Camera")]
        public Camera mainCamera;

        [Header("Raycast")]
        public LayerMask layerMask;
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

        public virtual bool IsMovementSelected()
        {
            return false;
        }

        public virtual bool IsAttackSelected()
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

        public virtual void RaycastFromCamera(){}

        public void DebugDrawRay()
        {
            if (cameraRaycastHit.collider != null) Debug.DrawLine(cameraRay.origin, cameraRaycastHit.point, Color.red);
            else Debug.DrawLine(cameraRay.origin, cameraRay.direction.normalized * 100f, Color.red);
        }
    }
}

