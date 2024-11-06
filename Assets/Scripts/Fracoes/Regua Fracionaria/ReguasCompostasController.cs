using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReguasCompostasController : MonoBehaviour
{
    [SerializeField] private List<Transform> _posicoesPreDefinidasVertical;
    [SerializeField] private List<Transform> _posicoesPreDefinidasHorizontal;
    [SerializeField] private Transform _instanciasVerticalParent;
    [SerializeField] private Transform _instanciasHorizontalParent;
    [SerializeField] private float _alturaRegua = 0.07f;
    [SerializeField] private float _escalaRegua = 0.5f;

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

        reguaComposta.transform.position = _posicoesPreDefinidasVertical[indexPosicao % _posicoesPreDefinidasVertical.Count].position;
        reguaComposta.transform.rotation = _posicoesPreDefinidasVertical[indexPosicao % _posicoesPreDefinidasVertical.Count].rotation;
        reguaComposta.transform.localScale = Vector3.one * _escalaRegua;

        reguaComposta.transform.SetParent(_instanciasVerticalParent);

        var copia = Instantiate(reguaComposta, _instanciasHorizontalParent);
        copia.transform.position = _posicoesPreDefinidasHorizontal[indexPosicao % _posicoesPreDefinidasHorizontal.Count].position;
        copia.transform.rotation = _posicoesPreDefinidasHorizontal[indexPosicao % _posicoesPreDefinidasHorizontal.Count].rotation;
        copia.transform.localScale = Vector3.one * _escalaRegua;

        _reguasArmazenadas.Add(reguaComposta);
    }


}
