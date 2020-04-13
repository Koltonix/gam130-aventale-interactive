using System.Collections.Generic;
using UnityEngine;

namespace CatGame.Menu
{
    public class MenuController : MonoBehaviour
    {
        [Header("Input")]
        //Not referring to enter specifically...
        [SerializeField]
        private KeyCode returnKey = KeyCode.Escape;

        public Stack<GameObject> menus = new Stack<GameObject>();
        public GameObject firstMenu;

        private void Start()
        {
            menus.Push(firstMenu);
        }

        private void Update()
        {
            if (Input.GetKeyDown(returnKey)) ReturnMenu();
        }

        public void AddFirstMenu(GameObject firstMenu)
        {
            menus.Push(firstMenu);
        }

        public void ReturnMenu()
        {
            //Can't go further back than the first
            if (menus.Count > 1)
            {
                menus.Peek().SetActive(false);
                menus.Pop();
                menus.Peek().SetActive(true);
            }
        }

        public void NextMenu(GameObject objectsToEnable)
        {
            //Setting the previous menu to false
            menus.Peek().SetActive(false);
            //Setting the new menu to true;
            objectsToEnable.SetActive(true);
            //Adding the new menu to the stack
            menus.Push(objectsToEnable);
        }
    }
}
