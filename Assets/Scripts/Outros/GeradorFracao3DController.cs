using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorFracao3DController : MonoBehaviour, IFracao
{
    [SerializeField] private GeradorDeNumeros3DController _numeradorController;
    [SerializeField] private GeradorDeNumeros3DController _denominadorController;

    private Fracao _fracao;

    public int Denominador()
    {
        return _fracao._denominador;
    }

    public GameObject Instancia()
    {
        return gameObject;
    }

    public int Numerador()
    {
        return _fracao._numerador;
    }

    public void SetFracao(Fracao fracao)
    {
        _fracao = fracao;

        AtualizaUI();
    }

    internal void AtualizaUI()
    {
        _numeradorController.AtualizaVisual(_fracao._numerador);
        _denominadorController.AtualizaVisual(_fracao._denominador);
    }
}
