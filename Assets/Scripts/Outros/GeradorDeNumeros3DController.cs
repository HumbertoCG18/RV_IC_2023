using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GeradorDeNumeros3DController : MonoBehaviour
{
    [SerializeField] private Transform _containerNumeros;
    [SerializeField] private List<ModeloNumero> _numerosPrefabs;

    [SerializeField] private float _espacoEntreNumeros;

    public void AtualizaVisual(int numero)
    {
        _containerNumeros.DestroyChildren();

        string numeroEmString = numero.ToString();
        int qtdNumeros = numeroEmString.Length;
        float larguraTotal = _espacoEntreNumeros * (qtdNumeros - 1);
        List<GameObject> instancias = new List<GameObject>();

        foreach (var algarismo in numeroEmString)
        {
            int algarismoInt = int.Parse(algarismo.ToString());

            var instancia = Instantiate(_numerosPrefabs[algarismoInt]._prefab, _containerNumeros);

            instancias.Add(instancia);
            larguraTotal += _numerosPrefabs[algarismoInt]._largura;
        }

        float passo = larguraTotal/2f;
        for (int i = 0; i < instancias.Count; i++)
        {
            passo += _numerosPrefabs[i]._largura / -2f;
            instancias[i].transform.localPosition = new Vector3(passo, 0f, 0f);
            passo += -_espacoEntreNumeros - _numerosPrefabs[i]._largura / 2f;
        }
    }

    [Serializable]
    private class ModeloNumero
    {
        public GameObject _prefab;
        public float _largura;
    }
}
