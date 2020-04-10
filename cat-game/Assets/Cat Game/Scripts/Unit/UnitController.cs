using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Controls;
using CatGame.Tiles;
using CatGame.Data;

namespace CatGame.Units
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
        [Space]

        [Header("Movement Settings")]
        [SerializeField]
        private float movementSpeed = 1.25f;
        [SerializeField]
        private float rotateSpeed = 1.25f;
        [SerializeField]
        private float minDistanceToCheck = 0.5f;
        private Coroutine movingCoroutine;

        [Header("Selection Information")]
        private UnitMovement selectedUnit;
        private Tile lastSelectedTile;
        private Tile[] lastSelectedPath;
        private Queue<Tile> pathToDraw = new Queue<Tile>();
        private SelectionProgress selectionProgress = SelectionProgress.UNSELECTED;    
        [Space]

        [Header("Tile Colours")]
        [SerializeField]
        private Color32 availableTileColour;
        [SerializeField]
        private Color32 selectedTileColour;
        [SerializeField]
        private Color32 pathColour;

        #region Event System
        public delegate void OnSelected(bool isSelected);
        public event OnSelected onSelect;

        public delegate void ChangeTileColours(Color32 colour);
        public event ChangeTileColours changeTileColours;
        #endregion

        public Player currentPlayer;

        private void Start()
        {
            currentInput = this.GetComponent<UserInput>();
            TurnManager.Instance.onPlayerCycle += ChangePlayer;
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
            if (currentInput.IsMovementSelected() && movingCoroutine == null)
            {
                RaycastHit gameObjectHit = currentInput.GetRaycastHit();

                //Makes every Unit check its own tiles again
                BoardManager.Instance.GetBoardTiles();

                //Acceping the tile to move to
                if (selectionProgress == SelectionProgress.SELECTED && selectedUnit.owner.GetCurrentActionPoints() > 0 && lastSelectedTile != null && lastSelectedTile.OccupiedUnit == null)
                {
                    MoveToTile();
                    return;
                }

                //Checking to see if it is a moveable object
                if (gameObjectHit.collider != null && gameObjectHit.collider.GetComponent<UnitMovement>() != null)
                {
                    //Seeing if the Unit can moved based on the current player
                    if (gameObjectHit.collider.GetComponent<Unit>().owner.GetActiveState())
                    {
                        DeselectUnit();
                        UnitClicked(gameObjectHit.collider.GetComponent<UnitMovement>());
                        return;
                    }
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
        private void UnitClicked(UnitMovement unitMovement)
        {            
            selectionProgress = SelectionProgress.SELECTED;

            selectedUnit = unitMovement;

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

            if (gameObjectHit.collider != null)
            {
                lastSelectedTile = GetSelectedTile(gameObjectHit);
                Tile[] path = selectedUnit.GetAvailableTilesFromPathfinding(lastSelectedTile);

                if (path != null)
                {
                    DrawPath(path, pathColour);
                    lastSelectedPath = path;
                    pathToDraw = PathArrayToQueue(path);
                }
                
                return;
            }
        }

        private void DrawPath(Tile[] path, Color32 pathColour)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] != null) path[i].WorldReference.GetComponent<Renderer>().material.color = pathColour;
            }
        }

        private Queue<Tile> PathArrayToQueue(Tile[] paths)
        {
            Queue<Tile> pathList = new Queue<Tile>(paths.Length);
            foreach(Tile path in paths)
            {
                pathList.Enqueue(path);
            }

            return pathList;
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
            if (gameObjectHit.collider != null)
            {
                foreach (Tile tile in selectedUnit.availableTiles)
                {
                    if (gameObjectHit.collider.gameObject == tile.WorldReference)
                    {
                        changeTileColours(availableTileColour);
                        tile.WorldReference.GetComponent<Renderer>().material.color = selectedTileColour;
                        return tile;
                    }

                    else tile.WorldReference.GetComponent<Renderer>().material.color = availableTileColour;
                }
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
            UnitMovement _selectedUnit = selectedUnit;
            DeselectUnit();

            #region Movement Deduction
            _selectedUnit.owner.GetPlayerReference().ActionPoints -= Mathf.RoundToInt(Vector3.Distance(
                                                         new Vector3(_selectedUnit.transform.position.x, 0, _selectedUnit.transform.position.z),
                                                         new Vector3(lastSelectedTile.Position.x, 0, lastSelectedTile.Position.z)));
            #endregion
            
            movingCoroutine = StartCoroutine(PathfindObject(_selectedUnit, lastSelectedPath, _selectedUnit.transform)); ;

            return;
        }

        private IEnumerator PathfindObject(UnitMovement _selectedUnit, Tile[] path, Transform objectToMove)
        {
            TurnManager.Instance.objectIsMoving = true;

            foreach (Tile tile in path)
            {
                DrawPath(pathToDraw.ToArray(), pathColour);
                Vector3 nextPosition = new Vector3(tile.WorldReference.transform.position.x, objectToMove.position.y, tile.WorldReference.transform.position.z);
                yield return MoveToPosition(objectToMove, nextPosition, movementSpeed);

                tile.WorldReference.GetComponent<Renderer>().material.color = tile.DefaultColour;
                pathToDraw.Dequeue();
            }

            float t = 0.0f;
            while (t < 1.0f)
            {
                t += Time.deltaTime * movementSpeed;

                Tile finalTile = path[path.Length - 1];
                Vector3 finalPosition = new Vector3(finalTile.WorldReference.transform.position.x, objectToMove.position.y, finalTile.WorldReference.transform.position.z);
                objectToMove.position = Vector3.Lerp(objectToMove.position, finalPosition, t);
                yield return new WaitForEndOfFrame();
            }

            //Change this if you do not want it to reselect upon completion.
            //UnitClicked(_selectedUnit);

            movingCoroutine = null;
            TurnManager.Instance.objectIsMoving = false;
        }

        private IEnumerator MoveToPosition(Transform objectToMove, Vector3 target, float speed)
        {
            float t = 0.0f;

            while (Vector3.Distance(objectToMove.position, target) > minDistanceToCheck)
            {
                Vector3 direction = (target - objectToMove.position).normalized;
                direction.y = 0;

                t += Time.deltaTime * rotateSpeed;

                if (direction == Vector3.zero) continue;

                objectToMove.position += (objectToMove.forward * Time.deltaTime * movementSpeed);
                objectToMove.forward = Vector3.Lerp(objectToMove.forward, direction, t);

                yield return new WaitForEndOfFrame();
            }

           yield return null;
        }

        public void ChangePlayer(Player newPlayer)
        {
            DeselectUnit();
            currentPlayer = newPlayer;
        }

    }
}
