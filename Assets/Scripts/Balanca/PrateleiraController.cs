using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrateleiraController : MonoBehaviour
{
    private Dictionary<string, PontoDeReferencia> _pontosDeReferencia;

    private void Awake()
    {
        _pontosDeReferencia = new Dictionary<string, PontoDeReferencia>();

        foreach (var peso in gameObject.GetComponentsInChildren<BalancaPesoController>())
        {
            var pontoDeReferencia = new PontoDeReferencia(peso.transform, peso.transform.localPosition, peso.transform.localRotation);
            _pontosDeReferencia.Add(peso.name, pontoDeReferencia);
        }
    }

    public void ResetarPosicoes()
    {
        foreach (var key in _pontosDeReferencia.Keys)
        {
            var pontoDeReferencia = _pontosDeReferencia[key];

            pontoDeReferencia._instancia.localPosition = pontoDeReferencia._position;
            pontoDeReferencia._instancia.localRotation = pontoDeReferencia._rotation;
        }
    }

    private class PontoDeReferencia
    {
        public Vector3 _position;
        public Quaternion _rotation;
        public Transform _instancia;

        public PontoDeReferencia(Transform instancia, Vector3 position, Quaternion rotation)
        {
            _instancia = instancia;
            _position = position;
            _rotation = rotation;
        }   
    }
}
