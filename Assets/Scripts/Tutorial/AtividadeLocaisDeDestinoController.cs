using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AtividadeLocaisDeDestinoController : MonoBehaviour
{
    [SerializeField] private Transform _containerLocaisDeDestino;

    private int counter = 0;

    public UnityEvent OnTodosLocaisAtingidos;

    public void ChegouNoLocal(GameObject localDeDestino)
    {
        counter++;
        localDeDestino.SetActive(false);

        if (counter == _containerLocaisDeDestino.childCount) OnTodosLocaisAtingidos?.Invoke();
    }
}
