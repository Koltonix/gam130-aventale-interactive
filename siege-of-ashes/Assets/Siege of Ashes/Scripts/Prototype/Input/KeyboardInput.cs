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

    public bool HasClicked()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) return true;
        return false;
    }

    private void RaycastFromCamera()
    {
        cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(cameraRay, out cameraRaycastHit);
    }

    public Ray GetRay()
    {
        return cameraRay;
    }
    
    public RaycastHit GetRaycastHit()
    {
        return cameraRaycastHit;
    }
}
