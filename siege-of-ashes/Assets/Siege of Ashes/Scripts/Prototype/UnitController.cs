using UnityEngine;
using SiegeOfAshes.Controls;

namespace SiegeOfAshes.Movement
{
    [RequireComponent(typeof(UserInput))]
    public class UnitController : MonoBehaviour
    {
        [Header("Input")]
        private UserInput currentInput;
        [Header("Selection Information")]
        private Unit selectedUnit;


        [Header("Tile Colours")]
        [SerializeField]
        private Color32 selectedColour;

        public delegate void OnSelected(bool isSelected);
        public event OnSelected onSelect;

        public delegate void ChangeTileColours(Color32 colour);
        public event ChangeTileColours changeTileColours;

        private void Start()
        {
            currentInput = this.GetComponent<UserInput>();
        }

        private void Update()
        {
            if (currentInput.HasClicked())
            {
                SelectUnit();
            }
        }

        /// <summary>
        /// Selects the unit from the data input from IGetInput
        /// </summary>
        private void SelectUnit()
        {
            RaycastHit gameObjectHit = currentInput.GetRaycastHit();

            //More checks can be done later to make it so you can only
            //select your units, but for now it will be one player
            // for this prototype
            if (gameObjectHit.collider != null && gameObjectHit.collider.GetComponent<Unit>() != null)
            {
                selectedUnit = gameObjectHit.collider.GetComponent<Unit>();

                onSelect += selectedUnit.SelectionListener;
                changeTileColours += selectedUnit.ChangeAvailableTilesColour;

                if (onSelect != null) onSelect.Invoke(true);
                if (changeTileColours != null) changeTileColours(selectedColour);
            }

            else
            {
                if (onSelect != null)
                {
                    onSelect.Invoke(false);

                    onSelect -= selectedUnit.SelectionListener;
                    changeTileColours -= selectedUnit.ChangeAvailableTilesColour;
                }

               selectedUnit = null;
            }
        }
    }

}
