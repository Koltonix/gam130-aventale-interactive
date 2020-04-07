using UnityEngine;

namespace CatGame.CameraMovement
{
    public abstract class CameraState : MonoBehaviour
    {
        public virtual void OnStateStay() { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
        public virtual bool IsCurrentlyRunning() { return false; }
    }
}

