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

public class BarraChocolateController : GeradorFracaoController
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
        CustomUtils.ClearChilds(_gridLayout.transform);

        GameObject prefabBase = _mostrarPedacosComidos ? _imagemPrefab : _basePrefab;
        GameObject prefabImage = _mostrarPedacosComidos ? _basePrefab : _imagemPrefab;

        float larguraBarra = _barraRectTransform.rect.width;
        float alturaBarra = _barraRectTransform.rect.height;

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

        Vector2 tamanhoPedaco = new Vector2(larguraBarra / colunas, alturaBarra / linhas);

        _gridLayout.cellSize = tamanhoPedaco;
        _gridLayout.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        _gridLayout.constraintCount = linhas;




        for (int i = 0; i < _fracao._denominador; i++)
        {
            var instancia = Instantiate(i < _fracao._numerador ? prefabImage : prefabBase, _gridLayout.transform);
            instancia.SetActive(true);

            try
            {
                var bordaEsquerda = instancia.GetChildByName("BordaEsquerda");
                var bordaDireita = instancia.GetChildByName("BordaDireita");
                var bordaCima = instancia.GetChildByName("BordaCima");
                var boardaBaixo = instancia.GetChildByName("BordaBaixo");
                //var centro = instancia.GetChildByName("Centro");

                float tamanhoBorda = tamanhoPedaco.x < tamanhoPedaco.y ? tamanhoPedaco.x * _porcentagemBorda : tamanhoPedaco.y * _porcentagemBorda;

                Vector2 tamanhoBordaHorizontal = new Vector2(tamanhoBorda, 0f);
                Vector2 tamanhoBordaVertical = new Vector2(0f, tamanhoBorda);

                bordaCima.GetComponent<RectTransform>().sizeDelta = tamanhoBordaVertical;
                boardaBaixo.GetComponent<RectTransform>().sizeDelta = tamanhoBordaVertical;

                bordaEsquerda.GetComponent<RectTransform>().sizeDelta = tamanhoBordaHorizontal;
                bordaDireita.GetComponent<RectTransform>().sizeDelta = tamanhoBordaHorizontal;

                //float tamanhoBorda = borda.GetComponent<RectTransform>().rect.width;

                //centro.GetComponent<RectTransform>().sizeDelta = new Vector2(tamanhoPedaco.x - tamanhoBorda * 2, 0f);

                //Debug.Log($"{larguraBarra} {alturaBarra} {tamanhoPedaco} {tamanhoBorda} {linhas} {colunas}");
            }
            catch (Exception e) 
            {
                Debug.LogError($"[BarraChocolateController][AtualizaFracoesUI] Borda ou centro do prefab chocolate nao encontrado: {e}");
            }
        }

        AtualizaFracao();
        OnValorMudou?.Invoke(_fracao); 
    }
}
