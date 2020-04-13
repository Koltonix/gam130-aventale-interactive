using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Following the Unity Architecture: https://unity.com/how-to/architect-game-code-scriptable-objects
/// </summary>
namespace CatGame.Menu
{
    public class GameEventListener : MonoBehaviour
    {
        public GameEvent Event;
        public UnityEvent Reponse;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }

        public void OnEventRaise()
        {
            Reponse.Invoke();
        }
    }
}
