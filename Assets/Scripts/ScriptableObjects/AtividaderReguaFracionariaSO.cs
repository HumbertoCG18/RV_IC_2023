using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Atividade_ReguaFracionaria", menuName = "ScriptableObjects/Atividade Regua Fracionaria")]
public class AtividaderReguaFracionariaSO : AtividadeSO
{
    public Fracao _fracaoEquivalenteDesejada;
    public int _quantidadeDeVariacoesNecessarias;
}
