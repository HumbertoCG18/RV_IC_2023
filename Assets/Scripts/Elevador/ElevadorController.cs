using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static ObjetoElevadorController;

public class ElevadorController : MonoBehaviour
{
    [SerializeField] private Transform _platadoraElevador;
    [SerializeField] private Transform _referenciaAndarDeBaixo;
    [SerializeField] private Transform _referenciaAndarDeCima;
    [SerializeField] private float _tempoDeAnimacao;
    [SerializeField] private List<AreaDosObjetosController> _areasDeObjetosControllers;

    [Header("Portas")]
    [SerializeField] private PortaController _portaAndarDeBaixo; 
    [SerializeField] private PortaController _portaAndarDeCima1; 
    [SerializeField] private PortaController _portaAndarDeCima2; 

    private List<ObjetoElevadorController> _objetoNoElevador = new List<ObjetoElevadorController>();
    private bool _emMovimento = false;

    public UnityEvent<Vector3> OnIniciaMovimentoElevador;
    public UnityEvent<Vector3> OnElevadorEmMovimento;
    public UnityEvent<Vector3> OnFinalizaMovimentoElevador;

    private bool _precisaValidarAreas = false;

    public void SubirElevador()
    {
        if (_emMovimento || _platadoraElevador.position == _referenciaAndarDeCima.position) return;

        if (_precisaValidarAreas && !AreasValidas()) return;

        _portaAndarDeBaixo.FechaPorta(() =>
        {
            StartCoroutine(AnimaElevadorCoroutine(_tempoDeAnimacao, _referenciaAndarDeBaixo.position, _referenciaAndarDeCima.position, () => 
            {
                _portaAndarDeCima1.AbrirPorta();
                _portaAndarDeCima2.AbrirPorta();
            }));
        });

    }

    public void DescerElevador()
    {
        if (_emMovimento || _platadoraElevador.position == _referenciaAndarDeBaixo.position) return;

        if (_precisaValidarAreas && !AreasValidas()) return;

        _portaAndarDeCima1.FechaPorta();
        _portaAndarDeCima2.FechaPorta(() =>
        {
            StartCoroutine(AnimaElevadorCoroutine(_tempoDeAnimacao, _referenciaAndarDeCima.position, _referenciaAndarDeBaixo.position, () =>
            {
                _portaAndarDeBaixo.AbrirPorta();
            }));
        });
        
    }

    private IEnumerator AnimaElevadorCoroutine(float tempoDeAnimacao, Vector3 pontoInicial, Vector3 pontoFinal, Action callback)
    {
        float t = 0;
        _emMovimento = true;

        OnIniciaMovimentoElevador?.Invoke(_platadoraElevador.position);

        while (t < 1)
        {
            Vector3 novoPonto = Vector3.Lerp(pontoInicial, pontoFinal, t);
            _platadoraElevador.position = novoPonto;

            OnElevadorEmMovimento?.Invoke(novoPonto);

            yield return null;

            t += Time.deltaTime / tempoDeAnimacao;
        }

        _platadoraElevador.position = pontoFinal;

        OnFinalizaMovimentoElevador?.Invoke(_platadoraElevador.position);

        _emMovimento = false;
        callback?.Invoke();
    }

    public void SetPrecisaValidarAreas(bool precisaValidarAreas) => _precisaValidarAreas = precisaValidarAreas;

    public bool AreasValidas()
    {
        return _areasDeObjetosControllers.All(a => a.AreaValida);
    }
}
