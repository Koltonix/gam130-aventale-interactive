using System;
using UnityEngine;

namespace CatGame.CameraMovement
{
    [Ser]
    public struct CameraPoint
    {
        public Transform worldTransform;
        public Vector3 rotation;
        public bool useObjectRotation;
    }
}
