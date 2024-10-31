using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ControlaEixoYController : MonoBehaviour
{
    [SerializeField] private ElevadorController _elevadorController;
    [SerializeField] private Transform _cameraRig;

    private Vector3 _offset;
    private Vector3 _cameraRigPosicaoInicial;

    public void OnIniciaMovimentoElevador(Vector3 _posicaoInicial)
    {
        _offset = _posicaoInicial;
        _cameraRigPosicaoInicial = _cameraRig.position;
    }

    public void OnElevadorEmMovimento(Vector3 _posicaoPlataforma)
    {
        float deslocamentoEmY = _posicaoPlataforma.y - _offset.y;

        var posicaoCameraRig = _cameraRig.position;
        posicaoCameraRig.y = _cameraRigPosicaoInicial.y + deslocamentoEmY;

        _cameraRig.position = posicaoCameraRig;
    }

    public void OnFinalizaMovimentoElevador(Vector3 _posicaoFinal)
    {
        OnElevadorEmMovimento(_posicaoFinal);
    }

    private void AtivaEventos()
    {
        _elevadorController.OnIniciaMovimentoElevador += OnIniciaMovimentoElevador;
        _elevadorController.OnElevadorEmMovimento += OnElevadorEmMovimento;
        _elevadorController.OnFinalizaMovimentoElevador += OnFinalizaMovimentoElevador;
    }

    private void DesativaEventos()
    {
        _elevadorController.OnIniciaMovimentoElevador -= OnIniciaMovimentoElevador;
        _elevadorController.OnElevadorEmMovimento -= OnElevadorEmMovimento;
        _elevadorController.OnFinalizaMovimentoElevador -= OnFinalizaMovimentoElevador;
    }

    public void CustomTriggerEnter(Collider objeto)
    {
        var xrOrigin = objeto.GetComponentInChildren<XROrigin>();

        if (xrOrigin == null) return;

        Debug.Log("Adiciona evento");
        AtivaEventos();
    }

    public void CustomTriggerExit(Collider objeto)
    {
        var xrOrigin = objeto.GetComponentInChildren<XROrigin>();

        if (xrOrigin == null) return;

        Debug.Log("Remove evento");
        DesativaEventos();
    }
}
