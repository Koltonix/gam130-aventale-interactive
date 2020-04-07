using System;
using UnityEngine;

namespace CatGame.CameraMovement
{
    [Serializable]
    public struct CameraPoint
    {
        public GameObject worldReference;
        public Vector3 rotation;
        public bool useObjectRotation;
    }
}
