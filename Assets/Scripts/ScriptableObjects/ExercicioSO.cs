using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Exercicio", menuName = "ScriptableObjects/ExercicioSO", order=0)]
public class ExercicioSO : ScriptableObject
{
    public string _descricao;
    public List<AudioClip> _descricoesEmAudios;
    public List<AtividadeSO> _atividades;
}
