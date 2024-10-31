using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FracaoUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txtNumerador;
    [SerializeField] private TextMeshProUGUI _txtDenominador;
    [SerializeField] private int _width = 150;
    [SerializeField] private int _height = 150;
    [SerializeField] private float _scale = 0.001f;

    private IFracao _fracao;

    public void SetFracao(IFracao fracao)
    {
        _fracao = fracao;

        AtualizaUI();
    }

    public void AtualizaUI()
    {
        _txtNumerador.text = _fracao.Numerador().ToString();
        _txtDenominador.text = _fracao.Denominador().ToString();
    }

    public IFracao Fracao => _fracao;

    public int Width => _width;
    public int Height => _height;
    public float Scale => _scale;
}
