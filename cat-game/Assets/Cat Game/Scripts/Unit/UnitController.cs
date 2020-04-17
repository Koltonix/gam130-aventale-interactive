using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatGame.Controls;
using CatGame.Tiles;
using CatGame.Data;

namespace CatGame.Units
{
    /// <summary>Selection State.</summary>
    public enum SelectionProgress
    {
        UNSELECTED = 0,
        SELECTED = 1,
        MOVING = 2
    }

    /// <summary>State Handler for the Unit Input, Movement and Combat</summary>
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

        #region Event System
        public delegate void OnSelected(bool isSelected);
        public event OnSelected onSelect;
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


        /// <summary>Determines the Click depending on the current state of the Unit.</summary>
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

                //ATTACKING
                if (selectionProgress == SelectionProgress.SELECTED && gameObjectHit.collider)
                {
                    Unit unitHit = gameObjectHit.collider.GetComponent<Unit>();
                    if (unitHit && unitHit.owner != selectedUnit.owner)
                    {
                        Debug.Log("ATTACK THE ENEMY!");

                    }

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

            //If a unit has been selected then it is the Tile picking phase
            if (selectionProgress == SelectionProgress.SELECTED)
            {
                SelectTile();
            }
        }

        /// <summary>Assigns the current Unit Data and triggers the listener in the UnitMovement</summary>
        /// <param name="unitMovement">A Unit's UnitMovement</param>
        private void UnitClicked(UnitMovement unitMovement)
        {            
            selectionProgress = SelectionProgress.SELECTED;
            selectedUnit = unitMovement;

            onSelect = selectedUnit.SelectionListener;
            onSelect?.Invoke(true);
        }

        /// <summary>Removes selection from the Unit and triggers the listener to disable</summary>
        private void DeselectUnit()
        {
            if (onSelect != null)
            {
                selectionProgress = SelectionProgress.UNSELECTED;
                onSelect.Invoke(false);
                //Required after the recent addition of the removal of unused tiles in pathfinding...
                //Not ideal, but is necessary.
                selectedUnit.ResetTileColours(BoardManager.Instance.tiles);
                onSelect -= selectedUnit.SelectionListener;

                selectedUnit = null;
            }
        }

        /// <summary>Selects the current tile that is being hovered over.</summary>
        private void SelectTile()
        {
            RaycastHit gameObjectHit = currentInput.GetRaycastHit();
            
            if (gameObjectHit.collider != null)
            {
                lastSelectedTile = GetSelectedTile(gameObjectHit);
                Tile[] path = selectedUnit.GetAvailableTilesFromPathfinding(lastSelectedTile);

                if (path != null)
                {
                    DrawPath(path, selectedUnit.pathColour);
                    lastSelectedPath = path;
                    pathToDraw = PathArrayToQueue(path);
                }
                
                return;
            }
        }

        /// <summary>Draws the path provided.</summary>
        /// <param name="path">The Tiles that create the path.</param>
        /// <param name="pathColour">The colour of the path</param>
        private void DrawPath(Tile[] path, Color32 pathColour)
        {
            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] != null) path[i].WorldReference.GetComponent<Renderer>().material.color = pathColour;
            }
        }

        /// <summary>Converts an array of Tiles to a Queue.</summary>
        /// <param name="paths">The Tiles in order to make a path.</param>
        /// <returns>A queue path.</returns>
        private Queue<Tile> PathArrayToQueue(Tile[] paths)
        {
            Queue<Tile> pathList = new Queue<Tile>(paths.Length);
            foreach(Tile path in paths)
            {
                pathList.Enqueue(path);
            }

            return pathList;
        }

        
        /// <summary>Gets the tile that the player is currently hovering over.</summary>
        /// <param name="gameObjectHit">The input raycast.</param>
        /// <returns>Returns the tile that the player is hovering over.</returns>
        private Tile GetSelectedTile(RaycastHit gameObjectHit)
        {
            if (gameObjectHit.collider != null)
            {
                foreach (Tile tile in selectedUnit.availableTiles)
                {
                    if (gameObjectHit.collider.gameObject == tile.WorldReference)
                    {
                        onSelect?.Invoke(false);
                        onSelect?.Invoke(true);

                        tile.WorldReference.GetComponent<Renderer>().material.color = selectedUnit.selectedTileColour;
                        return tile;
                    }

                    else tile.WorldReference.GetComponent<Renderer>().material.color = selectedUnit.availableTileColour;
                }
            }
           
            return null;
        }

        /// <summary>Starts the movement coroutone of the Unit to the Tile and then deselects it.</summary
        private void MoveToTile()
        {
            UnitMovement _selectedUnit = selectedUnit;
            DeselectUnit();

            _selectedUnit.owner.GetPlayerReference().ActionPoints -= lastSelectedPath.Length - 1;         
            movingCoroutine = StartCoroutine(PathfindObject(_selectedUnit, lastSelectedPath, _selectedUnit.transform)); ;

            return;
        }

        /// <summary>Invokes the Tile by Tile movement. Also centres the Unit on the final Tile.</summary>
        /// <param name="_selectedUnit">Unit last selected</param>
        /// <param name="path">Tile Path to take in order.</param>
        /// <param name="objectToMove">The GameObject to move in world space.</param>
        /// <returns>NULL</returns>
        private IEnumerator PathfindObject(UnitMovement _selectedUnit, Tile[] path, Transform objectToMove)
        {
            TurnManager.Instance.objectIsMoving = true;
            if (path != null)
            {
                //Move to each tile one by one
                foreach (Tile tile in path)
                {
                    DrawPath(pathToDraw.ToArray(), _selectedUnit.pathColour);
                    Vector3 nextPosition = new Vector3(tile.WorldReference.transform.position.x, objectToMove.position.y, tile.WorldReference.transform.position.z);
                    yield return MoveToPosition(objectToMove, nextPosition, movementSpeed);

                    tile.WorldReference.GetComponent<Renderer>().material.color = tile.DefaultColour;
                    pathToDraw.Dequeue();

                    onSelect?.Invoke(false);
                }

                //Re-centres the Unit on the final tile so that it doesn't offset itself
                float t = 0.0f;
                while (t < 1.0f)
                {
                    t += Time.deltaTime * movementSpeed;

                    Tile finalTile = path[path.Length - 1];
                    Vector3 finalPosition = new Vector3(finalTile.WorldReference.transform.position.x, objectToMove.position.y, finalTile.WorldReference.transform.position.z);
                    objectToMove.position = Vector3.Lerp(objectToMove.position, finalPosition, t);
                    yield return new WaitForEndOfFrame();
                }
            }     

            //Change this if you do not want it to reselect upon completion.
            //UnitClicked(_selectedUnit);

            movingCoroutine = null;
            TurnManager.Instance.objectIsMoving = false;
        }

        /// <summary>Linearly moves the Unit from its current position to the target.</summary>
        /// <param name="objectToMove">The GameObject to move in world space.</param>
        /// <param name="target">The point at which to move to.</param>
        /// <param name="speed">Speed it linearly moves at.</param>
        /// <returns>NULL</returns>
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

        /// <summary>Informs the State of the new player.</summary>
        /// <param name="newPlayer">Current Player's turn.</param>
        public void ChangePlayer(Player newPlayer)
        {
            DeselectUnit();
            currentPlayer = newPlayer;
        }

    }
}
