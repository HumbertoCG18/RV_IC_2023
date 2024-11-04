using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeradorDeReguaFracionariaController : MonoBehaviour, IFracao
{
    private static int DENOMINADOR_MAXIMO = 30;

    //[SerializeField] private ReguaFracionariaController _reguaFracionariaPrefab;
    [SerializeField] private ReguaBuilderController _reguaFracionariaPrefab;
    [SerializeField] private Transform _reguaContainer;
    [SerializeField] private GeradorFracao3DController _fracaoUIController;
    [SerializeField] private float _tamanhoDaRegua;
    [SerializeField] private Transform _localParaArmazenarReguas;
    [SerializeField] private ReguasCompostasController _reguasCompostasController;
    [SerializeField] private List<Color> _coresDasBases;

    private Fracao _fracao;
    private ReguaBuilderController _reguaAtual;

    public Action<Fracao> OnFracaoConfirmada;

    public void IniciaGerador()
    {

        CustomUtils.ClearChilds(_localParaArmazenarReguas);
        CustomUtils.ClearChilds(_reguaContainer);

        _fracao = null;

        IniciaRegua();
    }

    public void AtualizaRegua()
    {
        Color corDaRegua = _fracao._denominador < _coresDasBases.Count ? _coresDasBases[_fracao._denominador] : Color.green;
        _reguaAtual.SetCorDosElementos(corDaRegua);

        _reguaAtual.AtualizaVisual();
        _fracaoUIController.AtualizaUI();
    }

    public void AumentaNumerador()
    {
        _fracao._numerador = Mathf.Min(_fracao._denominador, _fracao._numerador + 1);

        AtualizaRegua();
    }

    public void DiminuiNumerador()
    {
        _fracao._numerador = Mathf.Max(0, _fracao._numerador - 1);

        AtualizaRegua();
    }

    public void AumentarDenominador()
    {
        _fracao._denominador = Mathf.Min(_fracao._denominador + 1, DENOMINADOR_MAXIMO);

        AtualizaRegua();
    }


    public void DiminuiDenominador()
    {
        _fracao._denominador = Mathf.Max(1, _fracao._denominador - 1);
        _fracao._numerador = Mathf.Min(_fracao._numerador, _fracao._denominador);

        AtualizaRegua();
    }

    public void IniciaRegua()
    {
        if (_fracao != null)
            _fracao = new Fracao(_fracao._numerador, _fracao._denominador);
        else
            _fracao = new Fracao(1,1);

        Color corDaRegua = _fracao._denominador < _coresDasBases.Count ? _coresDasBases[_fracao._denominador] : Color.green;
        _reguaAtual = Instantiate(_reguaFracionariaPrefab, _reguaContainer);
        _reguaAtual.transform.ResetTransformation();
        _reguaAtual.SetCorDosElementos(corDaRegua);
        _reguaAtual.SetFracao(_fracao, _tamanhoDaRegua);
        _fracaoUIController.SetFracao(_fracao);
    }

    public void ArmazenaRegua()
    {
        _reguaAtual.transform.SetParent(_localParaArmazenarReguas);
        _reguaAtual.transform.localPosition = Vector3.zero;
        _reguaAtual.transform.localPosition += Vector3.up * _localParaArmazenarReguas.childCount * _reguaAtual.Altura;

        IniciaRegua();
    }

    public void SalvaReguasAtuais()
    {
        if (_localParaArmazenarReguas.childCount != 0)
        {
            _reguasCompostasController.AdicionaReguas(CustomUtils.GetChilds(_localParaArmazenarReguas));
        }
    }

    public int QuantidadeReguasSalvas => _localParaArmazenarReguas.childCount;

    public void ConfirmarRegua()
    {
        OnFracaoConfirmada?.Invoke(_fracao);
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
}
