using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomCollisionController : MonoBehaviour
{
    public UnityEvent<Collider> OnTriggerEnterEvent;
    public UnityEvent<Collider> OnTriggerExitEvent;

    private void OnTriggerEnter(Collider other) => OnTriggerEnterEvent?.Invoke(other);

    private void OnTriggerExit(Collider other) => OnTriggerExitEvent?.Invoke(other);
}
