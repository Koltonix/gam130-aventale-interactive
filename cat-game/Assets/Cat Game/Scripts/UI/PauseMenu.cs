using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public void PauseGame()
    {
        isPaused = !isPaused;
        pauseUI.SetActive(isPaused);

        if (isPaused) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }

    //Manual Override just in case
    public void PauseGame(bool pauseState)
    {
        isPaused = pauseState;
        pauseUI.SetActive(isPaused);

        if (isPaused) Time.timeScale = 0.0f;
        else Time.timeScale = 1.0f;
    }

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
