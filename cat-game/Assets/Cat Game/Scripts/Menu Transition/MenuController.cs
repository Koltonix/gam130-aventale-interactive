using System.Collections.Generic;
using UnityEngine;

namespace CatGame.Menu
{
    public class MenuController : MonoBehaviour
    {
        public Stack<GameObject> menus = new Stack<GameObject>();

        public void ReturnMenu()
        {
            Debug.Log("Return Menu");
        }
    }
}
