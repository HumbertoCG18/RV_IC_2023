using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEventAfterSecondsController : MonoBehaviour
{
    [SerializeField] private float _timeToTrigger = 2f;

    public UnityEvent OnTriggerEvent;

    public void WaitToTrigger()
    {
        StartCoroutine(WaitToTriggerCoroutine());
    }

    private IEnumerator WaitToTriggerCoroutine()
    {
        yield return new WaitForSeconds( _timeToTrigger );

        OnTriggerEvent?.Invoke();
    }
}
