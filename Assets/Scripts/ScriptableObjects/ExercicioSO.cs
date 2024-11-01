using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Exercicio", menuName = "ScriptableObjects/ExercicioSO", order=0)]
public class ExercicioSO : ScriptableObject
{
    public AudioDescricao _audioDescricao;
    public List<AtividadeSO> _atividades;
}
