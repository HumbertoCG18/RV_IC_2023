using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class ReguaBuilderController : MonoBehaviour
{
    [SerializeField] private float _largura;
    [SerializeField] private float _altura;
    [SerializeField] private float _profundidade;

    [SerializeField] private float _grossuraBorda;

    [SerializeField] private GameObject _bordaPrefab;
    [SerializeField] private GameObject _elementPrefab;

    private float _offset = 0.002f;
    [SerializeField] private Color _cor = Color.white;
    [SerializeField] private Fracao _fracao;

    [SerializeField] private GameObject _argolaPrefab;

    public float Altura => _altura;

    private void Update()
    {
        if (Keyboard.current[Key.P].wasReleasedThisFrame)
        {
            AtualizaVisual();
        }
    }

    public void AtualizaVisual()
    {
        transform.DestroyChildren();

        // Parte interna
        GameObject elemento = Instantiate(_elementPrefab, transform);

        elemento.transform.ResetTransformation();
        elemento.transform.localScale = new Vector3(_profundidade - _offset/2f, _altura - _offset/2f, _largura * (_fracao.ValorReal) - _offset/2f);
        elemento.transform.localPosition = Vector3.forward * (_largura/2f - elemento.transform.localScale.z/2f + _offset);
        elemento.GetComponentInChildren<MeshRenderer>().material.color = _cor;

        elemento = Instantiate(_elementPrefab, transform);

        elemento.transform.ResetTransformation();
        elemento.transform.localScale = new Vector3(_profundidade - _offset, _altura - _offset, _largura - _offset);
        elemento.transform.localPosition = Vector3.zero;

        // Argola
        Vector3 passo = new Vector3(0f, 0f, _largura / _fracao._denominador);
        Vector3 posicaoInicial = new Vector3(0f, 0f, _largura / -2f);

        for (int i = 0; i < _fracao._denominador + 1; i++)
        {
            GameObject argola = Instantiate(_argolaPrefab, transform);

            argola.transform.ResetTransformation();
            argola.transform.localPosition = posicaoInicial + passo * i;
            argola.transform.localEulerAngles = Vector3.one;
        }

        // Bordas
        GameObject borda = Instantiate(_bordaPrefab, transform);

        borda.transform.ResetTransformation();
        borda.transform.localScale = new Vector3(_grossuraBorda, _grossuraBorda, _largura) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(_profundidade / -2 + _grossuraBorda/2f, _altura/2f - _grossuraBorda/2f, 0f);

        borda = Instantiate(_bordaPrefab, transform);

        borda.transform.ResetTransformation();
        borda.transform.localScale = new Vector3(_grossuraBorda, _grossuraBorda, _largura) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(_profundidade / 2 - _grossuraBorda / 2f, _altura / 2f - _grossuraBorda / 2f, 0f);

        borda = Instantiate(_bordaPrefab, transform);

        borda.transform.ResetTransformation();
        borda.transform.localScale = new Vector3(_grossuraBorda, _grossuraBorda, _largura) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(_profundidade / -2 + _grossuraBorda / 2f, _altura / -2f + _grossuraBorda / 2f, 0f);

        borda = Instantiate(_bordaPrefab, transform);

        borda.transform.ResetTransformation();
        borda.transform.localScale = new Vector3(_grossuraBorda, _grossuraBorda, _largura) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(_profundidade / 2 - _grossuraBorda / 2f, _altura / -2f + _grossuraBorda / 2f, 0f);
    }

    /*
    public void AtualizaVisual()
    {
        foreach (Transform t in transform) Destroy(t.gameObject);

        GameObject elemento = Instantiate(_elementPrefab, transform);
        // 0.5 0.3 0.3
        elemento.transform.localScale = new Vector3(_largura, _altura, _profundidade) - Vector3.one * _offset;
        elemento.transform.localPosition = new Vector3(0, 0, 0);

        float fracao = _fracao._numerador / (float)_fracao._denominador;

        elemento = Instantiate(_elementPrefab, transform);
        elemento.transform.localScale = new Vector3(_largura* fracao, _altura, _profundidade);
        elemento.transform.localPosition = new Vector3(_largura /2f - elemento.transform.localScale.x/2f, 0, 0);
        elemento.GetComponentInChildren<MeshRenderer>().material.color = _cor;

        GameObject borda = Instantiate(_bordaPrefab, transform);

        borda.transform.localScale = new Vector3(_largura, _grossuraBorda, _grossuraBorda) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(0, elemento.transform.localScale.y / 2f - _grossuraBorda / 2f, elemento.transform.localScale.y / 2f - _grossuraBorda / 2f);

        borda = Instantiate(_bordaPrefab, transform);

        borda.transform.localScale = new Vector3(_largura, _grossuraBorda, _grossuraBorda) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(0, elemento.transform.localScale.y / 2f - _grossuraBorda / 2f, elemento.transform.localScale.y / -2f + _grossuraBorda / 2f);


        borda = Instantiate(_bordaPrefab, transform);

        borda.transform.localScale = new Vector3(_largura, _grossuraBorda, _grossuraBorda) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(0, elemento.transform.localScale.y / -2f + _grossuraBorda / 2f, elemento.transform.localScale.y / 2f - _grossuraBorda / 2f);

        borda = Instantiate(_bordaPrefab, transform);

        borda.transform.localScale = new Vector3(_largura, _grossuraBorda, _grossuraBorda) + Vector3.one * _offset;
        borda.transform.localPosition = new Vector3(0, elemento.transform.localScale.y / -2f + _grossuraBorda / 2f, elemento.transform.localScale.y / -2f + _grossuraBorda / 2f);


        Vector3 posicaoInicialCima = new Vector3(_largura / -2f + _grossuraBorda / 2, _altura / 2f - _grossuraBorda / 2f, 0f);
        Vector3 posicaoInicialBaixo = new Vector3(_largura / -2f + _grossuraBorda / 2, _altura / -2f + _grossuraBorda / 2f, 0f);
        Vector3 passo = new Vector3(_largura / _fracao._denominador, 0, 0);
        for (int i = 0; i < _fracao._denominador + 1; i++)
        {
            borda = Instantiate(_bordaPrefab, transform);

            borda.transform.localScale = new Vector3(_grossuraBorda, _grossuraBorda, _profundidade) + Vector3.one * _offset;
            borda.transform.localPosition = posicaoInicialCima + passo * i;


            borda = Instantiate(_bordaPrefab, transform);

            borda.transform.localScale = new Vector3(_grossuraBorda, _grossuraBorda, _profundidade) + Vector3.one * _offset;
            borda.transform.localPosition = posicaoInicialBaixo + passo * i;
        }

        Vector3 posicaoInicialEsquerda = new Vector3(_largura / -2f + _grossuraBorda / 2, 0f, _profundidade / 2f - _grossuraBorda / 2f);
        Vector3 posicaoInicialDireita = new Vector3(_largura / -2f + _grossuraBorda / 2, 0f, _profundidade / -2f + _grossuraBorda / 2f);
        passo = new Vector3(_largura / _fracao._denominador, 0, 0);
        for (int i = 0; i < _fracao._denominador + 1; i++)
        {
            borda = Instantiate(_bordaPrefab, transform);

            borda.transform.localScale = new Vector3(_grossuraBorda, _altura, _grossuraBorda) + Vector3.one * _offset;
            borda.transform.localPosition = posicaoInicialEsquerda + passo * i;

            borda = Instantiate(_bordaPrefab, transform);

            borda.transform.localScale = new Vector3(_grossuraBorda, _altura, _grossuraBorda) + Vector3.one * _offset;
            borda.transform.localPosition = posicaoInicialDireita + passo * i;
        }
    }
    public void ControiRegua2(int qtd)
    {
        GameObject elemento = Instantiate(_bordaPrefab, transform);

        elemento.transform.localScale = new Vector3(_largura + _grossuraBorda*2, _altura, _profundidade) - Vector3.one * _offset;
        elemento.transform.localPosition = Vector3.zero;

        GameObject borda = Instantiate(_elementPrefab, transform);

        borda.transform.localScale = new Vector3(_largura + _grossuraBorda*2, _altura - _grossuraBorda * 2, _profundidade - _grossuraBorda * 2);
        borda.transform.localPosition = Vector3.zero;


        float _larguraCubo = _largura / qtd - _grossuraBorda;
        float _alturaCubo = _altura - _grossuraBorda * 2;

        Vector3 posicaoInicial = new Vector3(-_largura/2f + _larguraCubo/2f + _grossuraBorda/2f, 0f, 0f);
        Vector3 passo = new Vector3(_larguraCubo + _grossuraBorda, 0f, 0f);

        for (int i = 0; i < qtd; i++)
        {
            borda = Instantiate(_elementPrefab, transform);

            borda.transform.localScale = new Vector3(_larguraCubo, _alturaCubo, _profundidade);
            borda.transform.localPosition = posicaoInicial + passo * i;
        }
    }
    */

    public void SetCorDosElementos(Color corDaRegua)
    {
        _cor = corDaRegua;
    }

    public void SetFracao(Fracao novaFracao, float tamanhoDaRegua)
    {
        _fracao = novaFracao;
        _largura = tamanhoDaRegua;

        AtualizaVisual();
    }
}
