using System;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class PontoReferenciaController : MonoBehaviour
{
    [SerializeField] private PontoReferenciaManager _pontoReferenciaManager;

    private XRSocketInteractor _socketInteractor;

    private void Awake()
    {
        _socketInteractor = GetComponent<XRSocketInteractor>();
        _socketInteractor.selectEntered.AddListener(args => ObjetoEntrouNoEncaixe());
        _socketInteractor.selectExited.AddListener(args => ObjetoSaiuDoEncaixe());

        EstaPreenchido = false;
    }

    private void ObjetoSaiuDoEncaixe()
    {
        EstaPreenchido = false;
    }

    private void ObjetoEntrouNoEncaixe()
    {
        EstaPreenchido = true;

        _pontoReferenciaManager.AtualizaEstado();
    }

    public void DesativaEncaixce()
    {
        _socketInteractor.interactionLayers = 0;
    }

    public bool EstaPreenchido { get; private set; }
}
