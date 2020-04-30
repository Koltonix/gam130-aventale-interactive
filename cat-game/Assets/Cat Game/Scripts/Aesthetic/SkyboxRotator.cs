using UnityEngine;


/// <summary>Rotates the Skybox.</summary>
/// <remarks>
/// Sources: 
/// https://www.youtube.com/watch?v=cqGq__JjhMM 
/// https://forum.unity.com/threads/rotate-a-skybox.130639/
/// </remarks>
public class SkyboxRotator : MonoBehaviour
{
    public float rotateSpeed = 1.25f;

    private float currentRotation;
    public float CurrentRotation
    { 
        get { return currentRotation; }
        set
        {
            currentRotation = value;
            if (currentRotation > 360) currentRotation = 0;
            else if (currentRotation < 0) currentRotation = 360;
        }
    }

    private void Start()
    {
        currentRotation = RenderSettings.skybox.GetFloat("_Rotation");
    }

    private void Update()
    {
        CurrentRotation += rotateSpeed;
        RenderSettings.skybox.SetFloat("_Rotation", CurrentRotation);
    }
}
