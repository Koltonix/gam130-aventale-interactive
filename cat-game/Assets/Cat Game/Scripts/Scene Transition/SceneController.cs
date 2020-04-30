using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles Scene Transitioning for scripts and UnityEvents.
/// </summary>
public class SceneController : MonoBehaviour
{
    #region Singleton Reference
    public static SceneController Instance;
    private void Awake()
    {
        if (!Instance) Instance = this;
        else Destroy(this);
    }
    #endregion

    /// <summary>Loads the scene with a build index.</summary>
    /// <param name="buildIndex">Scene Build Index.</param>
    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>Loads the scene with a string</summary>
    /// <param name="sceneName">Scene Name.</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void PlayMainAfter(float delay)
    {
        StartCoroutine(DelayLoading(2, delay));
    }

    /// <summary>Reloads the current scene.</summary>
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>Quits the Application.</summary>
    public void QuitApplication()
    {
        Application.Quit();
    }

    private IEnumerator DelayLoading(int buildIndex, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(buildIndex);
    }
}
 