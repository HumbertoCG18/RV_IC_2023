using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;

public class ControlaEixoYController : MonoBehaviour
{
    [SerializeField] private Transform _cameraRig;

    private Vector3 _offset;
    private Vector3 _cameraRigPosicaoInicial;

    private ElevadorController _elevadorController;

    private void Start()
    {
        _elevadorController = ElevadorController.Instance;

        _elevadorController.AdicionaListenerNoElevador(this);
    }

    public void OnIniciaMovimentoElevador(Vector3 _posicaoInicial)
    {
        Debug.Log($"{name} Inicia elevador", gameObject);
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

    public void AtivaEventos()
    {
        _elevadorController.OnIniciaMovimentoElevador.AddListener(OnIniciaMovimentoElevador);
        _elevadorController.OnElevadorEmMovimento.AddListener(OnElevadorEmMovimento);
        _elevadorController.OnFinalizaMovimentoElevador.AddListener(OnFinalizaMovimentoElevador);
    }

    public void DesativaEventos()
    {
        _elevadorController.OnIniciaMovimentoElevador.RemoveListener(OnIniciaMovimentoElevador);
        _elevadorController.OnElevadorEmMovimento.RemoveListener(OnElevadorEmMovimento);
        _elevadorController.OnFinalizaMovimentoElevador.RemoveListener(OnFinalizaMovimentoElevador);
    }

    public void CustomTriggerEnter(Collider objeto)
    {
        if (objeto.transform != _cameraRig) return;

        AtivaEventos();
    }

    public void CustomTriggerExit(Collider objeto)
    {
        if (objeto.transform != _cameraRig) return;
        
        DesativaEventos();
    }

    private void OnDestroy()
    {
        _elevadorController.RemoveListenerDoElevador(this);
    }
}
