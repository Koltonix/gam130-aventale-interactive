using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Units;
using CatGame.Controls;

namespace CatGame.Combat
{
    public class Attacker : MonoBehaviour
    {
        public int Damage;
        //public GameObject ArrowPrefab;
        [SerializeField]
        private GameObject arrow;
        private bool active;

        void Start()
        {
            //arrow = Instantiate(ArrowPrefab);
            active = false;
            arrow.SetActive(active);
        }

        void Update()
        {
            if (active)
            {
                
            }
        }
    /// <summary>
    /// void OnMouseClick()
    ///{
    ///active = !active;
    ///arrow.SetActive(active);
    ///}
    /// </summary>

}
}