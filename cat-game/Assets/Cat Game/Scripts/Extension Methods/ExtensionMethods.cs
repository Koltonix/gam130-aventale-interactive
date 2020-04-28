using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static void DisableBehaviours(this GameObject gameobject)
    {
        foreach (MonoBehaviour behaviour in gameobject.GetComponents<MonoBehaviour>()) behaviour.enabled = false;
    }

    public static void DisableBehaviours(this GameObject gameobject, MonoBehaviour[] ignoredBehaviours)
    {
        List<MonoBehaviour> ignore = new List<MonoBehaviour>();
        ignore.AddRange(ignoredBehaviours);

        foreach (MonoBehaviour behaviour in gameobject.GetComponents<MonoBehaviour>())
        {
            if (ignore.Contains(behaviour)) continue;
            else behaviour.enabled = false;
        }
    }
}
