using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Automatically Instantiates the PauseMenu prefab if it is not referenced.
/// It also automatically assigns UnityEvents to the SceneController using the
/// text of the button.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private KeyCode pauseButton = KeyCode.Escape;
    private bool isPaused;

    [SerializeField]
    private GameObject pauseUIPrefab;
    [SerializeField]
    private GameObject pauseUI;


    [SerializeField]
    private int mainMenu = 0;
    private List<Transform> searchList = new List<Transform>();

    private void Start()
    {
        isPaused = false;
        if (!pauseUI) CreatePauseMenu();
        AssignEventsToButton(pauseUI.transform);
    }

    private void Update()
    {
        if (Input.GetKeyDown(pauseButton)) PauseGame();
    }

    /// <summary>Spawns the PauseMenu prefab at the centre of the screen.</summary>
    private void CreatePauseMenu()
    {
        pauseUI = Instantiate(pauseUIPrefab, Vector3.zero, Quaternion.identity);
        pauseUI.transform.SetParent(FindObjectOfType<Canvas>().transform);
        pauseUI.SetActive(isPaused);

        pauseUI.transform.localScale = Vector3.one;

        RectTransform rect = pauseUI.GetComponent<RectTransform>();
        rect.anchorMin = new Vector2(0.0f, 0.0f);
        rect.anchorMax = new Vector2(1.0f, 1.0f);

        rect.anchoredPosition = Vector3.zero;
        rect.sizeDelta = Vector3.zero;
    }

    /// <summary>Pauses the Game.</summary>
    /// <remarks>Be sure to reset the time back to 1.0f on a new scene</remarks>
    public void PauseGame()
    {
        isPaused = !isPaused;
        pauseUI.SetActive(isPaused);

        if (isPaused) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }

    /// <summary>Pauses the Game with a manual override</summary>
    /// <param name="pauseState">Overriding Pause Setting</param>
    /// <remarks>Be sure to reset the time back to 1.0f on a new scene</remarks>
    public void PauseGame(bool pauseState)
    {
        isPaused = pauseState;
        pauseUI.SetActive(isPaused);

        if (isPaused) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }

    /// <summary>
    /// Automatically assigns an event to the button's UnityEvent based on 
    /// the text of the button.
    /// </summary>
    /// <param name="parent">Root Transform</param>
    private void AssignEventsToButton(Transform parent)
    {
        searchList = new List<Transform>();
        DepthSearch(new Transform[1] { pauseUI.transform });

        for (int i = 0; i < searchList.Count; i++)
        {
            Button button = searchList[i].GetComponent<Button>();
            if (button)
            {
                string childText = button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                childText = childText.ToUpper();

                if (childText == "RESUME") button.onClick.AddListener(delegate { PauseGame(); });
                else if (childText == "RESTART") button.onClick.AddListener(delegate { SceneController.Instance.RestartScene(); });
                else if (childText == "MAIN MENU") button.onClick.AddListener(delegate { SceneController.Instance.LoadScene(mainMenu); });
                else if (childText == "QUIT") button.onClick.AddListener(delegate { SceneController.Instance.QuitApplication(); });
            }
        }
    }


    /// <summary>
    /// A depth search that searches through all of the child objects of every object
    /// in the root transform.
    /// </summary>
    /// <param name="paths">The directories of all of the objects in the current root.</param>
    /// <returns>An array of path directories.</returns>
    private Transform[] DepthSearch(Transform[] paths)
    {
        foreach (Transform childPath in paths)
        {
            for (int i = 0; i < childPath.childCount; i++)
            {
                searchList.Add(childPath.GetChild(i));
            }

            DepthSearch(GetAllChildObjects(childPath));
        }

        return searchList.ToArray();
    }

    /// <summary>Gets all of the Child Objects of a root component.</summary>
    /// <param name="parent">Root Tranform.</param>
    /// <returns>All of the Child Transforms in the Root.</returns>
    /// <remarks>This only gets the Transforms of just the root component's children.</remarks>
    private Transform[] GetAllChildObjects(Transform parent)
    {
        Transform[] children = new Transform[parent.childCount];
        for (int i = 0; i < children.Length; i++)
        {
            children[i] = parent.GetChild(i);
        }

        return children;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1.0f;
    }
}
