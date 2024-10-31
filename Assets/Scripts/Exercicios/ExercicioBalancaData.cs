using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExercicioBalancaData
{
    public int _nivel;
    public List<ExercicioBalanca> _exercicios;

    public ExercicioBalancaData(int nivel, List<ExercicioBalanca> exercicios)
    {
        _nivel = nivel;
        _exercicios = exercicios;
    }
}

