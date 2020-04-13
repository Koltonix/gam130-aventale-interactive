using UnityEngine;

namespace CatGame.Menu
{
    [CreateAssetMenu(fileName = "Menu-Point", menuName = "Scriptable-Objects/Menu/Menu-Point")]
    public class MenuPoint : ScriptableObject
    {
        public Transform aimPoint;
    }
}

