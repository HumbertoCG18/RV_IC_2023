using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtividadeChocolateController : AbstractAtividadeController
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Transform _coletoresParent;
    [SerializeField] private Transform _chocolatesParent;

    [SerializeField] private List<GameObject> _coletorPrefab;
    [SerializeField] private GameObject _objetosPrefabs;
    [SerializeField] private float _espacoEntreObjetos = 0.05f;
    [SerializeField] private float _espacoEntreColetores = 0.1f;

    [SerializeField] private float _tamanhoChocolate = 0.2f;
    [SerializeField] private float _tamanhoColetor = 0.4f;

    [SerializeField] private TextMeshProUGUI _txtFracaoResultante;

    private List<ColetorController> _coletoresControllers;
    private bool _respostaExibida = false;

    private Fracao _fracaoEsperada;

    private void Awake()
    {
        _coletoresControllers = new List<ColetorController>();
    }

    private List<GameObject> InstanciaObjetos(List<GameObject> prefabs, Transform parent, int quantidade, float espacoEntreObjetos, float larguraObjeto)
    {
        List<GameObject> instancias = new List<GameObject>();

        CustomUtils.ClearChilds(parent);

        float larguraTotal = larguraObjeto * quantidade + espacoEntreObjetos * (quantidade - 1);
        Vector3 posicaoInicial = (Vector3.forward * larguraTotal * -0.5f + Vector3.forward * larguraObjeto * 0.5f);
        Vector3 passo = Vector3.forward * (larguraObjeto + espacoEntreObjetos);

        for (int i = 0; i < quantidade; i++)
        {
            GameObject instancia = Instantiate(prefabs[i % prefabs.Count], parent);
            instancia.transform.localPosition = posicaoInicial + passo * i;

            instancias.Add(instancia);
        }

        return instancias;
    }

    public void ObjetoColetado(Transform objeto)
    {
        int totalDeChocolates = _chocolatesParent.childCount;
        int totalDeObjetoColetados = _coletoresControllers.Sum(c => c.QuantidadeDeFracoes);

        if (totalDeChocolates != totalDeObjetoColetados) return;

        float valorMaximo = _coletoresControllers.Max(c => c.ValorDaFracao());
        bool atividadeConcluido = true;

        foreach (var coletorController in _coletoresControllers)
        {
            float valor = coletorController.ValorDaFracao();
            coletorController.ExibeResultado(valor < valorMaximo);

            atividadeConcluido &= valor == valorMaximo;
        }

        _respostaExibida = true;


        if (atividadeConcluido)
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
        }
        else
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Errou);
        }
    }

    public void ObjetoRemovido(Transform objeto)
    {
        if (!_respostaExibida) return;

        _coletoresControllers.ForEach(c => c.ResetaVisual());
        _respostaExibida = false;
    }

    public void SetOnAtividadeConcluida(Action callback)
    {
        OnAtividadeConcluida += callback;
    }

    public override void CarregaAtividade(ScriptableObject atividade)
    {
        var atividadeChocolateSO = atividade as AtividadeChocolateSO;


        InstanciaObjetos(new List<GameObject>() { _objetosPrefabs }, _chocolatesParent, atividadeChocolateSO._quantidadeChocolates, _espacoEntreObjetos, _tamanhoChocolate);
        var instanciasColedores = InstanciaObjetos(_coletorPrefab, _coletoresParent, atividadeChocolateSO._quantidadeCriancas, _espacoEntreColetores, _tamanhoColetor);

        _coletoresControllers = new List<ColetorController>();

        instanciasColedores.ForEach(i => _coletoresControllers.Add(i.GetComponentInChildren<ColetorController>()));

        _coletoresControllers.ForEach(c => c.OnObjetoColetado += ObjetoColetado);
        _coletoresControllers.ForEach(c => c.OnObjetoRemovido += ObjetoRemovido);
    }

    public void SetView(bool newValue) => _view.SetActive(newValue);
}
