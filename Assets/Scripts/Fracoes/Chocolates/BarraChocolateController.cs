using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.AI;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BarraChocolateController: GeradorFracaoController
{
    [Header("Referencias Barra Chocolate Controller")]
    [SerializeField] private RectTransform _barraRectTransform;
    [SerializeField] private GridLayoutGroup _gridLayout;
    [SerializeField] private GameObject _basePrefab;
    [SerializeField] private GameObject _imagemPrefab;

    [SerializeField] private bool _mostrarPedacosComidos = false;

    [SerializeField] private float _porcentagemBorda = 0.2f;

    private void Update()
    {
        if (Keyboard.current[Key.P].wasReleasedThisFrame)
        {
            AtualizaFracaoUI();
        }
    }

    public override void AtualizaFracaoUI()
    {
        GameObject prefabBase = _mostrarPedacosComidos ? _imagemPrefab : _basePrefab;
        GameObject prefabImage = _mostrarPedacosComidos ? _basePrefab : _imagemPrefab;

        // Chat-GPT
        int linhas = 1;
        int colunas = _fracao._denominador;

        // Procurar o par de divisores mais próximo da raiz quadrada para definir o grid
        for (int i = 1; i <= Math.Sqrt(_fracao._denominador); i++)
        {
            if (_fracao._denominador % i == 0)
            {
                linhas = i;
                colunas = _fracao._denominador / i;
            }
        }


        float larguraBarra = _barraRectTransform.rect.width;
        float alturaBarra = _barraRectTransform.rect.height;
        Vector2 tamanhoPedaco = new Vector2(larguraBarra / colunas, alturaBarra / linhas);

        _gridLayout.cellSize = tamanhoPedaco;
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _gridLayout.constraintCount = linhas;

        // Altera tamanho do canvas original
        float valorRealFracao = _fracao.ValorReal;
        int novoTamanhoCanvas = Mathf.Max(1, Mathf.CeilToInt(valorRealFracao)) * _larguraCanvas;

        if (_atualizarPosicaoQuandoCanvasMudar && novoTamanhoCanvas != _mainCanvasRectTransform.rect.width)
        {
            float diferencaLargura = (novoTamanhoCanvas - _mainCanvasRectTransform.rect.width) / 2f;

            transform.localPosition += Vector3.back * diferencaLargura * _mainCanvasRectTransform.lossyScale.x;
        }

        _mainCanvasRectTransform.sizeDelta = new Vector2(novoTamanhoCanvas, _mainCanvasRectTransform.sizeDelta.y);
        OnCanvasMudouDeTamanho?.Invoke(_mainCanvasRectTransform.sizeDelta);
        _itensContainerRectTransform.DestroyChildren();

        // Preenche grid pai com instancia equivalente ao excesso da fracao

        int numerador = _fracao._numerador;

        do
        {
            var instanciaParent = Instantiate(_gridLayout, _itensContainerRectTransform).GetComponent<RectTransform>();

            for (int i = 0; i < _fracao._denominador; i++)
            {
                var instancia = Instantiate(i < numerador ? prefabImage : prefabBase, instanciaParent);
                instancia.SetActive(true);

                FormataCelula(instancia, tamanhoPedaco);
            }

            numerador -= _fracao._denominador;

            if (numerador > 0) Instantiate(_sinalMaisPrefab, _itensContainerRectTransform);

        } while (numerador > 0);

        /*
        for (int i = 0; i < valorRealFracao - 1; i++)
        {
            var instanciaParent = Instantiate(_gridLayout, _itensContainerRectTransform);

            for (int j = 0; j < _fracao._denominador; j++)
            {
                var instancia = Instantiate(prefabImage, instanciaParent.transform);
                instancia.SetActive(true);

                FormataCelula(instancia, tamanhoPedaco);
            }
        }


        var viewPrincipalParent = Instantiate(_gridLayout, _itensContainerRectTransform).GetComponent<RectTransform>();

        // Altera grid filho principal
        int sobreDoNumerador = Mathf.Max(1, _fracao._numerador % _fracao._denominador);

        for (int i = 0; i < _fracao._denominador; i++)
        {
            var instancia = Instantiate(i < sobreDoNumerador ? prefabImage : prefabBase, viewPrincipalParent);
            instancia.SetActive(true);

            FormataCelula(instancia, tamanhoPedaco);
        }
        */
        AtualizaFracao();
        OnValorMudou?.Invoke(_fracao); 
    }

    private void FormataCelula(GameObject instancia, Vector2 tamanhoPedaco)
    {
        try
        {
            var bordaEsquerda = instancia.GetChildByName("BordaEsquerda");
            var bordaDireita = instancia.GetChildByName("BordaDireita");
            var bordaCima = instancia.GetChildByName("BordaCima");
            var boardaBaixo = instancia.GetChildByName("BordaBaixo");

            float tamanhoBorda = tamanhoPedaco.x < tamanhoPedaco.y ? tamanhoPedaco.x * _porcentagemBorda : tamanhoPedaco.y * _porcentagemBorda;

            Vector2 tamanhoBordaHorizontal = new Vector2(tamanhoBorda, 0f);
            Vector2 tamanhoBordaVertical = new Vector2(0f, tamanhoBorda);

            bordaCima.GetComponent<RectTransform>().sizeDelta = tamanhoBordaVertical;
            boardaBaixo.GetComponent<RectTransform>().sizeDelta = tamanhoBordaVertical;

            bordaEsquerda.GetComponent<RectTransform>().sizeDelta = tamanhoBordaHorizontal;
            bordaDireita.GetComponent<RectTransform>().sizeDelta = tamanhoBordaHorizontal;
        }
        catch (Exception e)
        {
            Debug.LogError($"[BarraChocolateController][AtualizaFracoesUI] Borda ou centro do prefab chocolate nao encontrado: {e}");
        }
    }
}
