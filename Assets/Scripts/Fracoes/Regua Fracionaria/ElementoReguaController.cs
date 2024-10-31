using System;
using UnityEngine;

public class ElementoReguaController : MonoBehaviour
{
    [SerializeField] private MeshRenderer _meshRenderer;

    [SerializeField] private Color _corLigado = Color.green;
    [SerializeField] private Color _corDesligado = Color.white;

    public void Activate()
    {
        _meshRenderer.material.color = _corLigado;
    }

    public void Deactivate()
    {
        _meshRenderer.material.color = _corDesligado;
    }

    public void SetActivate(bool valor)
    {
        if (valor)
        {
            Activate();
        }
        else
        {
            Deactivate();
        }
    }

    public void SetCores(Color corLigado, Color corDesligado)
    {
        _corLigado = corLigado;
        _corDesligado = corDesligado;
    }
}