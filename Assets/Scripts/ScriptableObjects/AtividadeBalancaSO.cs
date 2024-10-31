using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Atvidade_Balanca", menuName = "ScriptableObjects/Atividade Balanca")]
public class AtividadeBalancaSO : AtividadeSO
{
    public int _nivel;
    public int _minimoTentativas;
    public List<PesoBalanca> _pesosLadoEsquerdo;
    public bool _podeAlterarLadoEsquerdo = true;
    public List<PesoBalanca> _pesosLadoDireito;
    public bool _podeAlterarLadoDireito = true;

    [Serializable]
    public class PesoBalanca
    {
        public int _valorPeso;
        public bool _isOculto;
    }
}
