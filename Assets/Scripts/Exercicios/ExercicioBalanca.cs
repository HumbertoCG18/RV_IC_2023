using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExercicioBalanca
{
    public string _descricao;
    public List<Peso> _pesosLadoEsquerdo;
    public List<Peso> _pesosLadoDireito;
    public int _combinacoesMinimas;
    public string _audioDaDescricao;

    public ExercicioBalanca(string descricao, List<Peso> pesosLadoEsquerdo, List<Peso> pesosLadoDireito, int combinacoesMinimas, string audioDaDescricao)
    {
        _descricao = descricao;
        _pesosLadoEsquerdo = pesosLadoEsquerdo;
        _pesosLadoDireito = pesosLadoDireito;
        _combinacoesMinimas = combinacoesMinimas;
        _audioDaDescricao = audioDaDescricao;
    }
}
