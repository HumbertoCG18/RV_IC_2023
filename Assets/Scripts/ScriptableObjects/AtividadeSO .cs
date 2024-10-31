using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Exercicio", menuName = "ScriptableObjects/Atividade", order=2)]
public class AtividadeSO : ScriptableObject
{
    public string _descricao;
    public AudioDescricao _descricaoEmAudio;
}
