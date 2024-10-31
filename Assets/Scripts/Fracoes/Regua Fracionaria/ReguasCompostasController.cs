using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReguasCompostasController : MonoBehaviour
{
    [SerializeField] private List<Transform> _posicoesPreDefinidas;
    [SerializeField] private Transform _instanciasParent;
    [SerializeField] private float _alturaRegua = 0.07f;

    private List<GameObject> _reguasArmazenadas = new List<GameObject>();

    public void AdicionaReguas(List<Transform> reguas)
    {
        GameObject reguaComposta = new GameObject();

        Vector3 passo = Vector3.up * _alturaRegua;

        int i = 0;
        foreach (Transform regua in reguas)
        {
            regua.SetParent(reguaComposta.transform);
            regua.localPosition = passo * i;

            i++;
        }

        int indexPosicao = _reguasArmazenadas.Count;

        reguaComposta.transform.position = _posicoesPreDefinidas[indexPosicao % _posicoesPreDefinidas.Count].position;
        reguaComposta.transform.rotation = _posicoesPreDefinidas[indexPosicao % _posicoesPreDefinidas.Count].rotation;
        reguaComposta.transform.localScale = Vector3.one * 0.6f;

        reguaComposta.transform.SetParent(_instanciasParent);

        _reguasArmazenadas.Add(reguaComposta);
    }
}
