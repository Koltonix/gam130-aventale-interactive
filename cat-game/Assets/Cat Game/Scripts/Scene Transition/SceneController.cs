using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    #region Singleton Reference
    public static SceneController Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }
    #endregion

    public void LoadScene(int buildIndex)
    {
        SceneManager.LoadScene(buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
 