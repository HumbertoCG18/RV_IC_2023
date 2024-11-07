using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Fracao
{
    public int _numerador;
    public int _denominador;

    public Fracao(int numerador, int denominador)
    {
        _numerador = numerador;
        _denominador = denominador;
    }

    public bool SaoEquivalentes(Fracao fracao) => (_numerador * fracao._denominador - fracao._numerador * _denominador) == 0;
    public float ValorReal => _numerador / (float)_denominador;
}
