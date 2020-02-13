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
        MeshRenderer meshRenderer;
        Building building;

        void Start()
        {
            meshRenderer = gameObject.GetComponent<MeshRenderer>();
            building = gameObject.GetComponentInParent<Building>();
        }

        void OnMouseOver()
        {
            meshRenderer.material = green;
        }

        void OnMouseDown()
        {
            building.SpawnUnit(gameObject.transform);
        }

        void OnMouseExit()
        {
            meshRenderer.material = white;
        }
    }
}