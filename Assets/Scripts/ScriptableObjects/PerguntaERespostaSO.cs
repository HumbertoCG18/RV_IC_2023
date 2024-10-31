using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Exercicio", menuName = "ScriptableObjects/Pergunta e Resposta", order = 3)]
public class PerguntaERespostaSO : ScriptableObject
{
    public string _pergunta;
    public List<string> _alternativas;
    public string _respostaEsperada;
    public bool _requerRespostaNumerica;
    public bool _requerRespostaIgual;
}
