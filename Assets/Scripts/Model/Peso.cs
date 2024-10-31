using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Peso
{
    public int _peso;
    public bool _isOculto;

    public Peso(int peso, bool isOculto)
    {
        _peso = peso;
        _isOculto = isOculto;
    }
}
