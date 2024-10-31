using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIContainerController: MonoBehaviour
{
    [SerializeField] private UIConainerElementController _containerElementPrefab;
    [SerializeField] private Transform _containerTransform;

    public void UpdateContainer<T>(List<T> values, Action<GameObject, int> preprocessingElement, Action<int> callback)
    {
        ClearChilds();

        for (int i = 0; i < values.Count; i++)
        {
            var instance = Instantiate(_containerElementPrefab, _containerTransform);

            preprocessingElement?.Invoke(instance.gameObject, i);

            instance.SetTrigger(callback, i);
        }
    }

    public void ClearChilds()
    {
        foreach (Transform child in _containerTransform) Destroy(child.gameObject);
    }
}
