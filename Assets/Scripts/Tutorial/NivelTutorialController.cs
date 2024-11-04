using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NivelTutorialController : MonoBehaviour
{
    public Action OnNivelConcluido;
    public UnityEvent OnFinalizaProcesso;

    public void NivelConcluido() => OnNivelConcluido?.Invoke();

    public void FinalizaProcessos()
    {
        OnFinalizaProcesso?.Invoke();
    }
}
