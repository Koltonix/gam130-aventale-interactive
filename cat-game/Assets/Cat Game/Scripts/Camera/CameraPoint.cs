using System;
using UnityEngine;

/// <summary>
/// Stores the GameObject World Reference as well as the rotation
/// and whether to use it or not.
/// </summary>
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
