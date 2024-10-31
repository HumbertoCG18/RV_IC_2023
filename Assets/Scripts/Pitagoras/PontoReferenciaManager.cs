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
    public UnityEvent<PontoReferenciaManager> OnPontoSelecionado;

    private void Awake()
    {
        _pontoDeReferenciaControllers = _pontosDeReferencia.GetComponentsInChildren<PontoReferenciaController>().ToList();
        _pontoDeReferenciaControllers.ForEach(p => p.SetManager(this));
    }

    public void AtualizaEstado()
    {
        OnPontoSelecionado?.Invoke(this);

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
        _pontoDeReferenciaControllers.ForEach(p => p.DesativaSocket());
    }

    public int QtdPontosSelecionados => _pontoDeReferenciaControllers.Count(p => p.EstaPreenchido);
    public bool GrupoCompleto => _pontoDeReferenciaControllers.All(pdr => pdr.EstaPreenchido);
}
