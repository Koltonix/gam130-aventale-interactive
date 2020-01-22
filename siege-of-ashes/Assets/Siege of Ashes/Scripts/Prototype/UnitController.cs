using UnityEngine;
using SiegeOfAshes.Controls;
using SiegeOfAshes.Tiles;

namespace SiegeOfAshes.Movement
{
    public enum SelectionProgress
    {
        UNSELECTED = 0,
        SELECTED = 1,
        MOVING = 2
    }

    /// <summary>
    /// Deals with controlling the Units using a UserInput input type. This allows the units to be move
    /// depending on the available tiles to them and updates the colours appropriately to indicate to the
    /// player.
    /// </summary>
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
            DetermineClick();
        }

        /// <summary>
        /// Used to determine what the click is to do depending on where the player
        /// has clicked on the screen. It can either move to a tile, select a unit
        /// (or a new one) and also deselect itself. It also will constantly update
        /// which tile is being hovered over if a unit has been selected.
        /// </summary>
        private void DetermineClick()
        {
            if (currentInput.HasClicked())
            {
                RaycastHit gameObjectHit = currentInput.GetRaycastHit();

                //Acceping the tile to move to
                if (selectionProgress == SelectionProgress.SELECTED && selectedUnit.movementPoints > 0 && lastSelectedTile != null)
                {
                    MoveToTile();
                    return;
                }

                //Clicking the unit itself
                if (gameObjectHit.collider != null && gameObjectHit.collider.GetComponent<Unit>() != null)
                {
                    DeselectUnit();
                    ActivateUnit(gameObjectHit.collider.GetComponent<Unit>());
                    return;
                }

                else DeselectUnit();
            }

            //If a unit has been selected. The tile picking phase
            if (selectionProgress == SelectionProgress.SELECTED)
            {
                SelectTile();
            }
        }

        /// <summary>
        /// Selects the unit from the data input from the UserInput parent class
        /// </summary>
        private void ActivateUnit(Unit unit)
        {            
            selectionProgress = SelectionProgress.SELECTED;

            selectedUnit = unit;

            onSelect = selectedUnit.SelectionListener;
            changeTileColours = selectedUnit.ChangeAvailableTilesColour;

            onSelect?.Invoke(true);
            changeTileColours?.Invoke(availableTileColour);
        }

        /// <summary>
        /// Deselects the unit that has been currently selected and removes them from the
        /// caller since it no longer needs to listen.
        /// </summary>
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

        /// <summary>
        /// Used to select the current tile that is beinh hovered over currently.
        /// </summary>
        private void SelectTile()
        {
            RaycastHit gameObjectHit = currentInput.GetRaycastHit();

            if (gameObjectHit.collider != null && gameObjectHit.collider.gameObject.layer == 9)
            {
                lastSelectedTile = GetSelectedTile(gameObjectHit);
                return;
            }
        }

        
        /// <summary>
        /// Shows the tile that the player is currently hovering over to be used
        /// in the selection progress later to move the unit.
        /// </summary>
        /// <param name="gameObjectHit">
        /// A RaycastHit that provides the data of what gameobject the player has
        /// currently selected to see if it is a tile and a selectable one at that.
        /// </param>
        /// <returns>
        /// Returns the tile that the player is hovering over
        /// </returns>
        private Tile GetSelectedTile(RaycastHit gameObjectHit)
        {
            foreach (Tile tile in selectedUnit.currentTilesAvailable)
            {
                if (gameObjectHit.collider.gameObject == tile.GameObject)
                {
                    changeTileColours(availableTileColour);
                    tile.GameObject.GetComponent<Renderer>().material.color = selectedTileColour;
                    return tile;
                }

                else tile.GameObject.GetComponent<Renderer>().material.color = availableTileColour;
            }

            return null;
        }

        /// <summary>
        /// Moves the unit to the new tile and deselects and activates it since the functions reset
        /// the new tiles that the unit can move to which is more preferable than repeating the two
        /// functions in here separately.
        /// </summary>
        private void MoveToTile()
        {
            Unit _selectedUnit = selectedUnit;
            DeselectUnit();

            #region Movement Deduction
            _selectedUnit.movementPoints -= Mathf.RoundToInt(Vector3.Distance(
                                                         new Vector3(_selectedUnit.transform.position.x, 0, _selectedUnit.transform.position.z),
                                                         new Vector3(lastSelectedTile.Position.x, 0, lastSelectedTile.Position.z)));
            #endregion

            _selectedUnit.transform.position = new Vector3(lastSelectedTile.Position.x, _selectedUnit.transform.position.y, lastSelectedTile.Position.z);
            ActivateUnit(_selectedUnit);
            return;
        }

    }

}
