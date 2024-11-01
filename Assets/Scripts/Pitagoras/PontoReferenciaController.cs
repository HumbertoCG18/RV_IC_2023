using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(XRSocketInteractor))]
public class PontoReferenciaController : MonoBehaviour
{
    [SerializeField] private PontoReferenciaManager _pontoReferenciaManager;

    [Header("Parametros Encaixe Especifico")]
    [SerializeField] private Transform _transformReferencia;
    [SerializeField] private bool _matchRotacao;
    [SerializeField] private float _erroMaximoRotacao = 0.1f;
    [SerializeField] private bool _matchPosicao;
    [SerializeField] private float _distanciaMinimaPosicao = 0.3f;

    private XRSocketInteractor _socketInteractor;
    private Transform _objetoDentroDaArea;

    private void Awake()
    {
        _socketInteractor = GetComponent<XRSocketInteractor>();
        _socketInteractor.selectEntered.AddListener(args => ObjetoEntrouNoSocket());
        _socketInteractor.selectExited.AddListener(args => ObjetoSaiuDoSocket());

        if (_transformReferencia != null)
        {
            DesativaSocket();
        }

        foreach (CustomCollisionController customCollision in GetComponentsInChildren<CustomCollisionController>())
        {
            customCollision.OnTriggerEnterEvent.AddListener(CustomTriggerEnter);
            customCollision.OnTriggerExitEvent.AddListener(CustomTriggerExit);
        }
    }

    private void Update()
    {
        if (_objetoDentroDaArea != null && !EstaPreenchido)
        {
            if (ValidaEncaixeDoObjeto())
            {
                AtivaSocket();
            }
            else
            {
                DesativaSocket();
            }
        }
    }

    private bool ValidaEncaixeDoObjeto()
    {
        List<bool> requisitos = new List<bool>() { true };

        if (_matchPosicao)
        {
            float distancia = Vector3.Distance(_objetoDentroDaArea.position, _transformReferencia.position);

            requisitos.Add(distancia <= _distanciaMinimaPosicao);
        }

        if (_matchRotacao)
        {
            Vector3 diferencaRotacao = GetRotationDiff(_transformReferencia.rotation, _objetoDentroDaArea.rotation);

            requisitos.Add(diferencaRotacao.x <= _erroMaximoRotacao && diferencaRotacao.y <= _erroMaximoRotacao && diferencaRotacao.z <= _erroMaximoRotacao);
        }

        return requisitos.All(r => r);
    }

    private Vector3 GetRotationDiff(Quaternion r1, Quaternion r2)
    {
        Vector3 diff = new Vector3();

        diff.x = Mathf.Abs(Mathf.Abs(r1.x) - Mathf.Abs(r2.x));
        diff.y = Mathf.Abs(Mathf.Abs(r1.y) - Mathf.Abs(r2.y));
        diff.z = Mathf.Abs(Mathf.Abs(r1.z) - Mathf.Abs(r2.z));

        return diff;
    }

    private void ObjetoSaiuDoSocket()
    {
        if (_objetoDentroDaArea != null)
        {
            _objetoDentroDaArea.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        }
        
        DesativaSocket();
        _pontoReferenciaManager.AtualizaEstado();
    }

    private void ObjetoEntrouNoSocket()
    {
        if (_objetoDentroDaArea != null)
        {
            _objetoDentroDaArea.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }

        _pontoReferenciaManager.AtualizaEstado();
    }

    public void DesativaSocket()
    {
        if (_socketInteractor.socketActive) _socketInteractor.socketActive = false;
    }

    public void AtivaSocket()
    {
        if (!_socketInteractor.socketActive) _socketInteractor.socketActive = true;
    }

    public void SetManager(PontoReferenciaManager pontoReferenciaManager)
    {
        _pontoReferenciaManager = pontoReferenciaManager;
    }

    public void CustomTriggerEnter(Collider objeto)
    {
        if (!VerificaSeObjetoEDoSocket(objeto.gameObject)) return;

        _objetoDentroDaArea = objeto.transform;
    }

    public void CustomTriggerExit(Collider objeto)
    {
        if (!VerificaSeObjetoEDoSocket(objeto.gameObject)) return;

        _objetoDentroDaArea = null;

        if (!_socketInteractor.hasSelection)
        {
            DesativaSocket();
        }
    }

    private bool VerificaSeObjetoEDoSocket(GameObject objeto)
    {
        if (!objeto.CompareTag("Peca")) return false;

        var grabInteractor = objeto.GetComponentInChildren<XRGrabInteractable>();

        return grabInteractor.interactionLayers.Equals(grabInteractor.interactionLayers);
    }

    public bool EstaPreenchido => _socketInteractor.hasSelection;
}
