using UnityEngine;
using SiegeOfAshes.Input;

public class KeyboardInput : MonoBehaviour, IGetInput
{
    [Header("Camera")]
    [SerializeField]
    private Camera mainCamera;

    [Header("Mouse")]
    private Vector3 mousePosition;

    [Header("Raycast")]
    private Ray cameraRay;
    private RaycastHit cameraRaycastHit;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;
    }

    private void Update()
    {
        RaycastFromCamera();
    }

    /// <summary>
    /// Determines whether the player has clicked the mouse or not
    /// </summary>
    /// <returns>
    /// Returns true if the player has clicked, otherwise  it is false
    /// </returns>
    public bool HasClicked()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) return true;
        return false;
    }

    /// <summary>
    /// Uses the mouse position to raycast to to get the data of what
    /// the player is currently selecting with their mouse
    /// </summary>
    private void RaycastFromCamera()
    {
        cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(cameraRay, out cameraRaycastHit);
    }

    /// <summary>
    /// Returns the ray to fulfill the contract set out by the IGetInput
    /// interface
    /// </summary>
    /// <returns>
    /// Returns the Ray from the main scene camera
    /// </returns>
    public Ray GetRay()
    {
        return cameraRay;
    }
    
    /// <summary>
    /// Returns the raycast that hit the object to fulfill the contract set
    /// out by the IGetInput interface
    /// </summary>
    /// <returns>
    /// Returns the RaycastHit that the the camera is receiving from the Ray
    /// </returns>
    public RaycastHit GetRaycastHit()
    {
        return cameraRaycastHit;
    }
}
