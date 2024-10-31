using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColisorModelo3DController : MonoBehaviour
{
    [SerializeField] private GameObject _controller;

    private void Awake()
    {
        if (_controller == null) _controller = gameObject;
    }

    public GameObject Controller => _controller;
}
