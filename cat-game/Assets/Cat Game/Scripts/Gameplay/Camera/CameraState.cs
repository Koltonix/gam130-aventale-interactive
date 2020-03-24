using UnityEngine;

namespace CatGame.CameraMovement
{
    public abstract class CameraState : MonoBehaviour
    {
        [Header("Coroutines")]
        public Coroutine[] coroutines;

        public virtual void OnStateStay() { }
        public virtual void OnStateEnter() { }
        public virtual void OnStateExit() { }
    }
}

