using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CatGame.UI
{
    public class WorldMenu : MonoBehaviour
    {
        [SerializeField]
        private Camera worldCamera;
        [SerializeField]
        public Button[] Buttons;

        // Update is called once per frame
        void Start()
        {
            worldCamera = Camera.main;
        }
        void Update()
        {
            gameObject.transform.rotation = worldCamera.transform.rotation;
        }
    }
}