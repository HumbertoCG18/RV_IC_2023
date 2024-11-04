using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Atividade_BarraChocolate", menuName = "ScriptableObjects/Atividade Barra Chocolate")]
public class AtividadeBarraChocolateSO : AtividadeSO
{
    public List<ElementoAtividadeChocolate> _ladoEsquerdo;
    public List<ElementoAtividadeChocolate> _ladoDireito;
}
