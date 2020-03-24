using UnityEngine;

namespace CatGame.CameraMovement
{
    public abstract class CameraState : MonoBehaviour
    {
        public virtual void Update() { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
    }
}

