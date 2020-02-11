using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycaster : MonoBehaviour
{
    Ray ray;
    RaycastHit hit;

    void Update()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetButtonDown("Fire1"))
            {
                IClickable building = hit.collider.gameObject.GetComponent<IClickable>();
                if (building != null)
                {
                    building.ActionOnClick();
                }
            }
                
        }
    }
}
