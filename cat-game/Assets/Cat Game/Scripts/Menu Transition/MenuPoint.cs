using UnityEngine;

namespace CatGame.Menu
{
    /// <summary>
    /// A Scriptable Object that holds a transform point.
    /// </summary>
    [CreateAssetMenu(fileName = "Menu-Point", menuName = "Scriptable-Objects/Menu/Menu-Point")]
    public class MenuPoint : ScriptableObject
    {
        public Transform aimPoint;
    }
}

