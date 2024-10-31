using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Atividade_Pizza", menuName = "ScriptableObjects/Atividade Pizza")]
public class AtividadePizzaSO : AtividadeSO
{
    public List<ElementoAtividadePizza> _fracoesDesejadasLadoEsquerdo;
    public List<ElementoAtividadePizza> _fracoesDesejadasLadoDireito;


}
