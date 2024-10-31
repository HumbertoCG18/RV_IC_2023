using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomUtils
{
    public static Bounds GetBoundRecursive(GameObject gameObject)
    {
        Bounds bounds = new Bounds();

        foreach (var collider in gameObject.GetComponentsInChildren<Collider>())
        {
            bounds.Encapsulate(collider.bounds);
        }

        return bounds;
    }

    public static void ClearChilds(Transform parent)
    {
        foreach (Transform child in parent) Object.Destroy(child.gameObject);
    }

    public static List<Transform> GetChilds(Transform parent)
    {
        List<Transform> childs = new List<Transform>();

        foreach (Transform child in parent)
        {
            childs.Add(child);
        }

        return childs;
    }
}
