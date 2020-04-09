using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFading : MonoBehaviour
{
    [SerializeField]
    private float fadingSpeed;
    [SerializeField]
    private Image fadingImage;

    private Coroutine fadingCoroutine;

    [Header("Canvas Settings")]
    private Canvas canvas;

    private void Start()
    {
        canvas = FindObjectOfType<Canvas>();
        if (canvas == null) CreateCanvas();
    }

    private Canvas CreateCanvas()
    {
        GameObject canvasObject = new GameObject("Canvas");
        return null;
    }

    private IEnumerator FadeObject(Image imageToFade, float speed, float targetFade)
    {
        float t = 0;
        while (t < 1)
        { 
            t += speed * Time.deltaTime;

            Color currentFade = imageToFade.color;
            float currentAlpha = Mathf.Lerp(currentFade.a, targetFade, t);
            currentFade.a = currentAlpha;

            imageToFade.color = currentFade;

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
