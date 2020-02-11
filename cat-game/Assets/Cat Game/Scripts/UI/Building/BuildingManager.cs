using UnityEngine;
using SiegeOfAshes.Controls;

namespace SiegeOfAshes.UI
{
    public class BuildingManager : MonoBehaviour
    {
        public GameObject inputReference;
        public IGetOnClick clickData;

        private void Start()
        {
            clickData = inputReference.GetComponent<IGetOnClick>();
        }

        private void Update()
        {
            if (clickData.HasClicked() && clickData.GetRaycastHit().collider != null)
            {           
                IClickable clickable = clickData.GetRaycastHit().collider.gameObject.GetComponent<IClickable>();
                if (clickable != null)
                {
                    clickable.ActionOnClick();
                }
            }
        }

    }
}