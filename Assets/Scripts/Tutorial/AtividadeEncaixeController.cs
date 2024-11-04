using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtividadeEncaixeController : MonoBehaviour
{
    [SerializeField] private PontoReferenciaManager _pontoReferenciaManager;

    public void RemoveEncaixes()
    {
        _pontoReferenciaManager.DesativaEncaixes();
    }
}
