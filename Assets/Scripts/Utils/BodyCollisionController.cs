using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fonte: https://www.youtube.com/watch?v=L2jCO7g_18w
public class BodyCollisionController : MonoBehaviour
{
    [SerializeField] private Transform _cameraRig;
    [SerializeField] private Transform _feet;

    private void Start()
    {
        _feet.position = _cameraRig.position;
    }

    private void Update()
    {
        _cameraRig.position = _feet.position = new Vector3(_cameraRig.position.x, _feet.position.y, _cameraRig.position.z);
    }
}
