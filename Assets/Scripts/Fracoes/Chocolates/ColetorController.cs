using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit;

public class ColetorController : MonoBehaviour
{
    //[SerializeField] private ExibidorDeFracoesController _exibidorDeFracoes;
    [SerializeField] private float _espacoEntreObjetos = 0.1f;
    [SerializeField] private Transform _containerDeObjetos;
    [SerializeField] private GameObject _colisorDeExibicao;
    [SerializeField] private GameObject _sinalMaisPrefab;
    [SerializeField] private Transform _sinalParent;

    [Header("UI")]
    [SerializeField] private Image _imgResultado;
    [SerializeField] private Sprite _spriteEsperando;
    [SerializeField] private Sprite _spriteComChocolate;
    [SerializeField] private Sprite _spriteSemChocolate;
    [SerializeField] private Sprite _spriteAcertou;
    [SerializeField] private Material _materialEsperando;
    [SerializeField] private Material _materialCerto;
    [SerializeField] private Material _materialErrado;

    private enum EstadoColetor { Esperando, ComChocolate, SemChocolate, Acertou };
    private EstadoColetor _estadoColetor;

    private List<IFracao> _fracoes;

    public Action<Transform> OnObjetoColetado;
    public Action<Transform> OnObjetoRemovido;

    private void Awake()
    {
        _fracoes = new List<IFracao>();
        _estadoColetor = EstadoColetor.Esperando;
    }

    public void AtualizaPosicoes()
    {
        if (_fracoes.Count == 0) return;

        Bounds bounds = _fracoes[0].Instancia().GetComponentInChildren<MeshRenderer>().bounds;
        float width = bounds.size.z;

        float larguraTotal = width * _fracoes.Count + _espacoEntreObjetos * (_fracoes.Count - 1);
        Vector3 posicaoInicial = (Vector3.forward * larguraTotal * -0.5f + Vector3.forward * width * 0.5f);
        Vector3 passo = Vector3.forward * (width + _espacoEntreObjetos);

        AtualizaTamanhoColisorDeExibicao(larguraTotal * 1.1f);
        _sinalParent.DestroyChildren();

        int i = 0;
        foreach (var objeto in _fracoes)
        {
            var instancia = objeto.Instancia();
            var paiInstancia = instancia.transform.parent;

            instancia.transform.parent = _containerDeObjetos;
            instancia.transform.localPosition = (posicaoInicial + passo * i);
            instancia.transform.localRotation = Quaternion.identity;
            instancia.transform.parent = paiInstancia;

            if (i < _fracoes.Count - 1)
            {
                var sinal = Instantiate(_sinalMaisPrefab, _sinalParent);
                sinal.transform.localPosition = posicaoInicial + passo * i + Vector3.forward * ((width + _espacoEntreObjetos) / 2f);
            }

            i++;
        }

    }

    private void AtualizaTamanhoColisorDeExibicao(float novaEscalaZ)
    {
        Vector3 escala = _colisorDeExibicao.transform.localScale;

        escala.z = novaEscalaZ;

        _colisorDeExibicao.transform.localScale = escala;
    }

    private IEnumerator AdicionaObjetoCoroutine(IFracao fracao)
    {
        var grabController = fracao.Instancia().GetComponentInChildren<XRGrabInteractable>();

        yield return InteractionManager.Instance.DropObjectCoroutine(grabController);

        fracao.Instancia().layer = LayerMask.NameToLayer("ObjetoBloqueado");
        _fracoes.Add(fracao);
        AtualizaPosicoes();


        _estadoColetor = EstadoColetor.ComChocolate;
        AtualizaUI();

        OnObjetoColetado?.Invoke(fracao.Instancia().transform);
    }

    public void OnCustomTriggerEnter(Collider other)
    {
        var colisor = other.GetComponentInChildren<ColisorModelo3DController>();

        if (colisor == null) return;

        var fracao = colisor.Controller.GetComponentInChildren<IFracao>();

        if (fracao == null) return;

        StartCoroutine(AdicionaObjetoCoroutine(fracao));
    }

    public void RemoveFracao(IFracao fracao)
    {
        _fracoes.Remove(fracao);
        fracao.Instancia().layer = 0;
        AtualizaPosicoes();

        OnObjetoRemovido?.Invoke(fracao.Instancia().transform);

        if (_fracoes.Count == 0)
        {
            _estadoColetor = EstadoColetor.Esperando;
        }

        AtualizaUI();
    }

    public void OnCustomTriggerExit(Collider other)
    {
        var colisor = other.GetComponentInChildren<ColisorModelo3DController>();

        if (colisor == null) return;

        var fracao = colisor.Controller.GetComponentInChildren<IFracao>();

        if (fracao == null) return;

        RemoveFracao(fracao);
    }

    public float ValorDaFracao()
    {
        return _fracoes.Sum(f => f.Numerador() / (float)f.Denominador());
    }

    public void ExibeResultado(bool possuiMenos)
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();

        meshRenderer.material = possuiMenos ? _materialErrado : _materialCerto;

        _estadoColetor = possuiMenos ? EstadoColetor.SemChocolate : EstadoColetor.Acertou;
        AtualizaUI();
    }

    public void ResetaVisual()
    {
        var meshRenderer = GetComponentInChildren<MeshRenderer>();

        meshRenderer.material = _materialEsperando;

        _estadoColetor = _fracoes.Count > 0 ? EstadoColetor.ComChocolate : EstadoColetor.Esperando;
        AtualizaUI();
    }

    public void AtualizaUI()
    {
        switch (_estadoColetor)
        {
            case EstadoColetor.Esperando: _imgResultado.sprite = _spriteEsperando; break;
            case EstadoColetor.ComChocolate: _imgResultado.sprite = _spriteComChocolate; break;
            case EstadoColetor.SemChocolate: _imgResultado.sprite = _spriteSemChocolate; break;
            case EstadoColetor.Acertou: _imgResultado.sprite = _spriteAcertou; break;
        }
    }

    public int QuantidadeDeFracoes => _fracoes.Count;

    /*
    private void OnTriggerEnter(Collider other)
    {
        var colisor = other.GetComponentInChildren<ColisorModelo3DController>();

        if (colisor == null) return;

        var fracao = colisor.Controller.GetComponentInChildren<IFracao>();

        if (fracao == null) return;

        _fracoes.Add(fracao);
        _exibidorDeFracoes.AdicionaFracao(fracao);
    }

    private void OnTriggerExit(Collider other)
    {
        var colisor = other.GetComponentInChildren<ColisorModelo3DController>();

        if (colisor == null) return;

        var fracao = colisor.Controller.GetComponentInChildren<IFracao>();

        if (fracao == null) return;
        if (!_fracoes.Contains(fracao)) return;

        _fracoes.Remove(fracao);
        _exibidorDeFracoes.RemoveFracao(fracao);
    }
    */
}
