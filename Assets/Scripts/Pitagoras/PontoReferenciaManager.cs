using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PontoReferenciaManager : MonoBehaviour
{
    [SerializeField] private Transform _pontosDeReferencia;

    private List<PontoReferenciaController> _pontoDeReferenciaControllers;

    public UnityEvent OnGrupoCompleto;

    private void Awake()
    {
        _pontoDeReferenciaControllers = _pontosDeReferencia.GetComponentsInChildren<PontoReferenciaController>().ToList();
    }

    public void AtualizaEstado()
    {
        if (GrupoCompleto)
        {
            OnGrupoCompleto?.Invoke();
        }
    }

    private void OnDestroy()
    {
        _pontoDeReferenciaControllers.ForEach(p => Destroy(p.gameObject));
    }

    public void DesativaEncaixes()
    {
        _pontoDeReferenciaControllers.ForEach(p => p.DesativaEncaixce());
    }

    public bool GrupoCompleto => _pontoDeReferenciaControllers.All(pdr => pdr.EstaPreenchido);
}
