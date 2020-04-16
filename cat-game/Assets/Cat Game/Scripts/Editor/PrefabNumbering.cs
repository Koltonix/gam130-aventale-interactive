using System.Linq;
using UnityEngine;

/// <summary>
/// Created by Christopher Robertson
/// </summary>
[ExecuteInEditMode]
public class PrefabNumbering : MonoBehaviour
{
    public GameObject prefab;
    public string extraText = "#";

    private GameObject[] allPrefabs;
    private GameObject[] previousPrefabs;

    private int worldCount = 0;
    private int previousWorldCount = 0;

    private void Update()
    {
        if (prefab)
        {
            worldCount = FindObjectsOfType<GameObject>().Length;

            if (previousWorldCount != worldCount)
            {
                CheckNewItem();
            }
        } 
    }

    private void CheckNewItem()
    {
        allPrefabs = GameObject.FindObjectsOfType<GameObject>().Where(obj => UnityEditor.PrefabUtility.GetPrefabParent(obj) == prefab).ToArray();
        if (allPrefabs.Length > 0 && previousPrefabs != null)
        {
            for (int i = 0; i < previousPrefabs.Length; i++)
            {
                //Already existed
                if (allPrefabs[0] == previousPrefabs[i])
                {
                    return;
                }
            }

            allPrefabs[0].name += extraText + allPrefabs.Length;
        }

        previousPrefabs = allPrefabs;
    }

    public void RenameAllGameObjects()
    {
        GameObject[] allObjects = allPrefabs = GameObject.FindObjectsOfType<GameObject>().Where(obj => UnityEditor.PrefabUtility.GetPrefabParent(obj) == prefab).ToArray();

        //Reversed as the 0th element is the latest instance
        for (int i = allObjects.Length - 1; i >= 0; i--)
        {
            allPrefabs[i].name = prefab.name + extraText + (allObjects.Length - i - 1);
        }
    }
}
