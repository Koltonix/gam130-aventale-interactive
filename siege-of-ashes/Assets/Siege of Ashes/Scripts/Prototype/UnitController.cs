using UnityEngine;
using SiegeOfAshes.Controls;
using SiegeOfAshes.Tiles;

public enum SelectionProgress 
{
    UNSELECTED = 0,
    SELECTED = 1,
}

namespace SiegeOfAshes.Movement
{
    [RequireComponent(typeof(UserInput))]
    public class UnitController : MonoBehaviour
    {
        [Header("Input")]
        private UserInput currentInput;
        [Header("Selection Information")]
        private Unit selectedUnit;
        private Tile lastSelectedTile;
        private SelectionProgress selectionProgress = SelectionProgress.UNSELECTED;    

        [Header("Tile Colours")]
        [SerializeField]
        private Color32 availableTileColour;
        [SerializeField]
        private Color32 selectedTileColour;

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
            SelectUnit();
        }

        private void SelectUnit()
        {
            if (currentInput.HasClicked())
            {
                RaycastHit gameObjectHit = currentInput.GetRaycastHit();

                //if (selectionProgress == SelectionProgress.SELECTED)
                //{
                //    //MOVE IT


                //    Unit temp = selectedUnit;
                //    DeselectUnit();

                //    selectedUnit.movementPoints -= 2; //STOP THE HARDCODE LATER

                //    return;
                //}

                if (gameObjectHit.collider != null && gameObjectHit.collider.GetComponent<Unit>() != null)
                {
                    ActivateUnit(gameObjectHit);
                    return;
                }

                else DeselectUnit();
            }

            if (selectionProgress == SelectionProgress.SELECTED)
            {
                RaycastHit gameObjectHit = currentInput.GetRaycastHit();

                if (gameObjectHit.collider != null && gameObjectHit.collider.gameObject.layer == 9)
                {
                    SelectTile(gameObjectHit);
                    return;
                }
            }
        }

        /// <summary>
        /// Selects the unit from the data input from IGetInput
        /// </summary>
        private void ActivateUnit(RaycastHit gameObjectHit)
        {            
            selectionProgress = SelectionProgress.SELECTED;

            selectedUnit = gameObjectHit.collider.GetComponent<Unit>();

            onSelect = selectedUnit.SelectionListener;
            changeTileColours = selectedUnit.ChangeAvailableTilesColour;

            onSelect?.Invoke(true);
            changeTileColours?.Invoke(availableTileColour);
        }

        private void DeselectUnit()
        {
            if (onSelect != null)
            {
                onSelect.Invoke(false);

                onSelect -= selectedUnit.SelectionListener;
                changeTileColours -= selectedUnit.ChangeAvailableTilesColour;

                selectedUnit = null;
                selectionProgress = SelectionProgress.UNSELECTED;
            }
        }

        private void SelectTile(RaycastHit gameObjectHit)
        {
            foreach (Tile tile in selectedUnit.currentTilesAvailable)
            {
                if (gameObjectHit.collider.gameObject == tile.GameObject)
                {
                    lastSelectedTile = tile;
                    lastSelectedTile.GameObject.GetComponent<Renderer>().material.color = selectedTileColour;
                }

                else tile.GameObject.GetComponent<Renderer>().material.color = availableTileColour;
            }
        }
    }

}
