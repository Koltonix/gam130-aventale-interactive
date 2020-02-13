using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SiegeOfAshes.UI
{
    public class SpawnPad : MonoBehaviour
    {
        [SerializeField]
        Material green;
        [SerializeField]
        Material white;
        [SerializeField]
        Material clear;
        MeshRenderer meshRenderer;
        Building building;
        bool tileSelected;

        void Awake()
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            building = gameObject.GetComponentInParent<Building>();
        }

        void OnMouseEnter()
        {
            if (!tileSelected)
            {
                meshRenderer.material = white;
            }
        }

        void OnMouseDown()
        {
            building.SelectTile(this);
            meshRenderer.material = green;
            tileSelected = true;
        }
        public void DeselectTile()
        {
            meshRenderer.material = clear;
            tileSelected = false;
        }

        void OnMouseExit()
        {
            if (!tileSelected)
            {
                meshRenderer.material = clear;
            }
        }
    }
}