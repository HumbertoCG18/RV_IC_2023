using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PitagorasNivelController : MonoBehaviour
{
    [SerializeField] private Transform _preConfiguracaoTransform;
    [SerializeField] private List<PontoReferenciaManager> _gruposDosEncaixes;
 
    private List<PontoDeReferencia> _pontosDeReferencia;
    private PitagorasController _pitagorasController;

    private void Awake()
    {
        _pontosDeReferencia = new List<PontoDeReferencia>();

        foreach (Transform objeto in _preConfiguracaoTransform)
        {
            var pontoDeReferencia = new PontoDeReferencia(objeto.localPosition, objeto.localRotation, objeto);
            _pontosDeReferencia.Add(pontoDeReferencia);
        }
    }

    public void ResetaEstado()
    {
        foreach (var pontoDeReferencia in _pontosDeReferencia)
        {
            pontoDeReferencia._instance.localPosition = pontoDeReferencia._position;
            pontoDeReferencia._instance.localRotation = pontoDeReferencia._rotation;
        }
    }

    public void GrupoCompleto()
    {
        bool todosGruposCompletos = _gruposDosEncaixes.All(g => g.GrupoCompleto);

        if (todosGruposCompletos)
        {
            _pitagorasController.NivelConcluido();
        }
    }

    public void SetInfo(PitagorasController pitagorasController)
    {
        _pitagorasController = pitagorasController;
    }

    public void DesativaEncaixes()
    {
        _gruposDosEncaixes.ForEach(g => g.DesativaEncaixes());
    }
}
