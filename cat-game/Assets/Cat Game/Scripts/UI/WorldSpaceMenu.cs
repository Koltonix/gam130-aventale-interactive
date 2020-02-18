using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatGame.UI
{
    public class WorldSpaceMenu : MonoBehaviour
    {
        Camera worldCamera;
        // Start is called before the first frame update
        void Start()
        {
            worldCamera = Camera.main;
        }

        // Update is called once per frame
        void Update()
        {
            gameObject.transform.rotation = worldCamera.transform.rotation;
        }
    }
}