using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class GeradorDePesosController : MonoBehaviour
{
    [SerializeField] private Transform _pontoDeReferencia;
    [SerializeField] private GameObject _pesoPrefab;

    [SerializeField] private float _velocidadeDaRotacao = 30f;
    [SerializeField, Range(1, 99)] private int _valorPeso = 1;
    [SerializeField] private Color _corDoPeso = Color.green;

    private BalancaPesoController _instanciaAtual;

    private void Awake()
    {
        CriaPeso();
    }

    private void Update()
    {
        _instanciaAtual.transform.Rotate(Vector3.up * _velocidadeDaRotacao * Time.deltaTime);


        if (Keyboard.current[Key.Digit9].wasPressedThisFrame)
        {
            DiminuiPeso();
        }

        if (Keyboard.current[Key.Digit0].wasPressedThisFrame)
        {
            AumentaPeso();
        }
    }

    public void CriaPeso()
    {
        _instanciaAtual = Instantiate(_pesoPrefab, _pontoDeReferencia).GetComponent<BalancaPesoController>();

        _instanciaAtual.transform.localPosition = Vector3.zero;
        _instanciaAtual.transform.rotation = Quaternion.identity;
        _instanciaAtual.SetInfo(_valorPeso, false, _corDoPeso, true);
        _instanciaAtual.GetComponent<Rigidbody>().isKinematic = true;

        var grabInteractor = _instanciaAtual.GetComponent<XRGrabInteractable>();
        grabInteractor.selectExited.AddListener(args => AjustaParametrosNoNovoPeso(grabInteractor));
    }

    private void AjustaParametrosNoNovoPeso(XRGrabInteractable interactor)
    {
        interactor.selectExited.RemoveAllListeners();

        interactor.GetComponent<Rigidbody>().isKinematic = false;
    }

    private void OnTriggerExit(Collider other)
    {
        var controller = other.GetComponent<BalancaPesoController>();

        if (controller == null) return;

        CriaPeso();
    }

    public void AumentaPeso()
    {
        _valorPeso = Mathf.Min(_valorPeso + 1, 99);

        _instanciaAtual.SetInfo(_valorPeso, false, _corDoPeso, true);
    }

    public void DiminuiPeso()
    {
        _valorPeso = Mathf.Max(_valorPeso - 1, 1);
        _instanciaAtual.SetInfo(_valorPeso, false, _corDoPeso, true);
    }

    public void LimpaInstancias()
    {
        foreach (Transform child in _pontoDeReferencia) Destroy(child.gameObject);

        CriaPeso();
    }
}
