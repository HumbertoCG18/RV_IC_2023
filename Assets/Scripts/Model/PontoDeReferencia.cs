using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PontoDeReferencia
{
    public Vector3 _position;
    public Quaternion _rotation;
    public Transform _instance;

    public PontoDeReferencia(Vector3 position, Quaternion rotation, Transform instance)
    {
        _position = position;
        _rotation = rotation;
        _instance = instance;
    }   
}
