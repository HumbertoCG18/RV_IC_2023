using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private float _speed;

    public float Speed { get => _speed; }

    internal void SetSpeed(float speed)
    {
        _speed = speed;
    }

    private void Update()
    {
        transform.RotateAround(_target.position, _target.up, _speed * Time.deltaTime);
    }
}
