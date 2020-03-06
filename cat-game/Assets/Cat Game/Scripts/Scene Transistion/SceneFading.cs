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

    private void Start()
    {
        if (fadingCoroutine == null) fadingCoroutine = StartCoroutine(FadeObject(fadingImage, fadingSpeed));
    }

    private IEnumerator FadeObject(Image imageToFade, float speed)
    {
        float t = 0;
        while (t < 1)
        { 
            t += speed * Time.deltaTime;

            Color currentFade = imageToFade.color;
            float currentAlpha = Mathf.Lerp(currentFade.a, 1f, t);
            currentFade.a = currentAlpha;

            imageToFade.color = currentFade;

            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
}
