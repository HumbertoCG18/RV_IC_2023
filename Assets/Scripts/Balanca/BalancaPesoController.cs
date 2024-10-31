using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BalancaPesoController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private List<TextMeshProUGUI> _txtPesos;
    [SerializeField] private Transform _pivotParaCores;

    [Header("Configuracao dos Parametros")]
    [SerializeField] private int _peso;
    [SerializeField] private Color _color;

    private bool _isOculto = false;
    private XRGrabInteractable _grabController;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _grabController = GetComponentInChildren<XRGrabInteractable>();
        _rigidbody = GetComponentInChildren<Rigidbody>();
    }

    private void Start()
    {
        AtualizaValor();
    }

    private void OnValidate()
    {
        AtualizaValor();
    }

    public void SetInfo(int peso, bool isOculto, Color color, bool podeAlterarPesos)
    {
        _peso = peso;
        _isOculto = isOculto;
        _color = color;

        if (_grabController != null) _grabController.enabled = podeAlterarPesos;
        if (podeAlterarPesos) gameObject.layer = LayerMask.NameToLayer("PesoBloqueado");

        AtualizaValor();
    }

    private void AtualizaValor()
    {
        if (_isOculto)
        {
            _txtPesos.ForEach(p => p.text = "X");
        }
        else
        {
            _txtPesos.ForEach(p => p.text = _peso.ToString());
        }

        foreach (var renderer in _pivotParaCores.GetComponentsInChildren<MeshRenderer>())
        {
            renderer.sharedMaterial.color = _color;
        }
    }

    public int Peso { get => _peso; }
    public bool IsOculto { get => _isOculto; }
}
