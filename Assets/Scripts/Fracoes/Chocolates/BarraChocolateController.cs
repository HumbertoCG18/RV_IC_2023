using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BarraChocolateController : GeradorFracaoController
{
    [Header("Referencias Barra Chocolate Controller")]
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private GameObject _basePrefab;
    [SerializeField] private GameObject _imagemPrefab;

    [SerializeField] private int _numeroColunasGridLayout = -1;
    [SerializeField] private int _numeroLinhasGridLayout = -1;
    [SerializeField] private bool _mostrarPedacosComidos = false;

    private void Update()
    {
        if (Keyboard.current[Key.P].wasReleasedThisFrame)
        {
            AtualizaFracaoUI();
        }
    }

    public void SetParametros(int numeroColunas, int numeroLinhas)
    {
        _numeroColunasGridLayout = numeroColunas;
        _numeroLinhasGridLayout = numeroLinhas;
    }

    public override void AtualizaFracaoUI()
    {
        CustomUtils.ClearChilds(_gridLayout.transform);

        if (_numeroColunasGridLayout != -1)
        {
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            _gridLayout.constraintCount = _numeroColunasGridLayout;

        }else if (_numeroLinhasGridLayout != -1)
        {
            _gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            _gridLayout.constraintCount = _numeroLinhasGridLayout;
        }

        GameObject prefabBase = _mostrarPedacosComidos ? _basePrefab : _imagemPrefab;
        GameObject prefabImage = _mostrarPedacosComidos ? _imagemPrefab : _basePrefab;

        for (int i = 0; i < _fracao._denominador; i++)
        {
            Instantiate(i < _fracao._numerador ? prefabImage : prefabBase, _gridLayout.transform);
        }

        AtualizaFracao();
        OnValorMudou?.Invoke(_fracao); 
    }
}
