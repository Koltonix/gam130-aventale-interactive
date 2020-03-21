using System;
using UnityEngine;

namespace CatGame.CameraMovement
{
    [Serializable]
    public struct CameraPoint
    {
        public Transform worldTransform;
        public Vector3 rotation;
        public bool useObjectRotation;
    }
}
