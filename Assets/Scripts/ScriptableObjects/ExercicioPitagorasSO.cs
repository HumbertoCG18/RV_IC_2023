using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Atividade Pitagoras", menuName = "ScriptableObjects/Atividade Pitagoras")]
public class AtividadePitagorasSO : AtividadeSO
{
    public PitagorasNivelController _nivelControllerPrefab;
    public List<PerguntaERespostaSO> _perguntas;
}
