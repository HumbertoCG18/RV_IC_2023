using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.Oculus;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PizzaController : MonoBehaviour, IFracao
{
    private static int DENOMINADOR_MAXIMO = 20;

    [SerializeField] private Image _imgPizza;
    [SerializeField] private RectTransform _containerLinhas;
    [SerializeField] private GameObject _linhaPrefab;

    [SerializeField] private FracaoUIController _fracaoUIController;

    [SerializeField] private GameObject _viewFormato1;
    [SerializeField] private GameObject _viewFormato2;
    [SerializeField] private Image _imgPizzaFormato1;
    [SerializeField] private Image _imgPizzaFormato2;

    [Header("UI")]
    [SerializeField] private Button _btnAumentaNumerador;
    [SerializeField] private Button _btnDiminuiNumerador;
    [SerializeField] private Button _btnAumentaDenominador;
    [SerializeField] private Button _btnDiminuiDenominador;



    private Fracao _fracao;
    public Action<Fracao> OnValorMudou;

    private void Awake()
    {
        _fracao = new Fracao(0, 1);
        _fracaoUIController.SetFracao(this);
        SetFormatoDeExibicao(false);
    }

    public void AtualizaPizza()
    {
        CustomUtils.ClearChilds(_containerLinhas);

        if (_fracao._denominador == 1)
        {
            _imgPizza.fillAmount = 1 - _fracao._numerador;
        }
        else
        {
            float anguloPorPedaco = 360f / (float)_fracao._denominador;

            for (int i = 0; i < _fracao._denominador; i++)
            {
                var instancia = Instantiate(_linhaPrefab, _containerLinhas).transform;

                instancia.Rotate(Vector3.forward, anguloPorPedaco * i);
                instancia.gameObject.SetActive(true);
            }

            _imgPizza.fillAmount = 1 - _fracao._numerador / (float)_fracao._denominador;
        }

        AtualizaFracao();
        OnValorMudou?.Invoke(_fracao);
    }

    public void SetFormatoDeExibicao(bool pedacaoComidos)
    {
        _viewFormato1.SetActive(!pedacaoComidos);
        _viewFormato2.SetActive(pedacaoComidos);

        _imgPizza = pedacaoComidos ? _imgPizzaFormato2 : _imgPizzaFormato1;
        AtualizaPizza();
    }

    public void AtualizaFracao()
    {
        _fracaoUIController.AtualizaUI();

        _btnAumentaNumerador.gameObject.SetActive(_fracao._numerador < DENOMINADOR_MAXIMO);
        _btnDiminuiNumerador.gameObject.SetActive(_fracao._numerador > 0);
        _btnAumentaDenominador.gameObject.SetActive(_fracao._denominador < DENOMINADOR_MAXIMO);
        _btnDiminuiDenominador.gameObject.SetActive(_fracao._denominador > 1);
    }

    public void AumentaNumerador()
    {
        _fracao._numerador = Mathf.Min(_fracao._denominador, _fracao._numerador + 1);

        AtualizaPizza();
    }

    public void DiminuiNumerador()
    {
        _fracao._numerador = Mathf.Max(0, _fracao._numerador - 1);

        AtualizaPizza();
    }

    public void AumentarDenominador()
    {
        _fracao._denominador = Mathf.Min(_fracao._denominador + 1, DENOMINADOR_MAXIMO);

        AtualizaPizza();
    }


    public void DiminuiDenominador()
    {
        _fracao._denominador = Mathf.Max(1, _fracao._denominador - 1);
        _fracao._numerador = Mathf.Min(_fracao._numerador, _fracao._denominador);

        AtualizaPizza();
    }

    public int Numerador()
    {
        return _fracao._numerador;
    }

    public int Denominador()
    {
        return _fracao._denominador;
    }

    public GameObject Instancia()
    {
        return gameObject;
    }

    public void Reseta()
    {
        _fracao = new Fracao(1, 1);
        AtualizaPizza();
    }

    public Fracao Fracao => _fracao;
}
