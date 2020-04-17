using System.Collections.Generic;
using UnityEngine;

namespace CatGame.Menu
{
    /// <summary>
    /// A State Handler which manages the currently state of the menu using a Stack
    /// which can be popped to determine where to go back to.
    /// </summary>
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

        /// <summary>Adds the root menu.</summary>
        /// <param name="firstMenu">GameObject of a Button Menu</param>
        public void AddFirstMenu(GameObject firstMenu)
        {
            menus.Push(firstMenu);
        }

        /// <summary>Goes back in the Menu Stack.</summary>
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

        /// <summary>Adds the next menu GameObject to the stack.</summary>
        /// <param name="objectsToEnable">Next Menu to Load.</param>
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
