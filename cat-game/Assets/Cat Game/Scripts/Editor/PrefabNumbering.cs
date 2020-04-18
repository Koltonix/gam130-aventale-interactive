using System.Linq;
using UnityEngine;

/// <summary>
/// Christopher Robertson 2020
/// Used to manually number new assets when dragged into a scene.
/// A prefab must be assigned to the Inspector for this to work.
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

            //A change from the previous amount of GameObjects in the scene.
            if (previousWorldCount != worldCount)
            {
                CheckNewItem();
            }
        } 
    }

    /// <summary>Checks the latest GameObject and ensures it doesn't exist already.</summary>
    /// <remarks>
    /// The newest GameObject in a scene will always be the 0th element
    /// when using FindObjectOfType<>().
    /// </remarks>
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

            //Renames the latest GameObject
            allPrefabs[0].name += extraText + allPrefabs.Length;
        }

        //Caching the previous GameObjects
        previousPrefabs = allPrefabs;
    }
    
    /// <summary>
    /// Renames all of the GameObjects of the Prefabs in order.
    /// </summary>
    /// <remarks>
    /// This does it so that 0 is the newest. To change this use a regular
    /// for loop rather than a reverse for loop.
    /// </remarks>
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
