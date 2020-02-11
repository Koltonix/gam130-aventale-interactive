using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SiegeOfAshes.UI
{
    public class WorldMenu : MonoBehaviour
    {
        [SerializeField]
        private Camera worldCamera;
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