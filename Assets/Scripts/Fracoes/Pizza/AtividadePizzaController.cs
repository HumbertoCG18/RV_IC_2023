using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtividadePizzaController : AbstractAtividadeController
{
    [SerializeField] private GameObject _view;
    [SerializeField] private Transform _containerPizzas;
    [SerializeField] private float _espacoEntreObjetos = 0.01f;

    [SerializeField] private AcertoErroUIController _acertoErroUIController;

    [Header("Parametros")]
    [SerializeField] private float _larguraPizzaController = 0.3f;

    [SerializeField] private List<GeradorFracaoController> _pizzaControllersLadoEsquerdo = new List<GeradorFracaoController>();
    [SerializeField] private List<GeradorFracaoController> _pizzaControllersLadoDireito = new List<GeradorFracaoController>();

    [SerializeField] private List<Fracao> _fracoesEsperadasLadoEsquerdo;
    [SerializeField] private List<Fracao> _fracoesEsperadasLadoDireito;

    [Header("Prefabs")]
    [SerializeField] private GeradorFracaoController _pizzaControllerPrefab;
    [SerializeField] private GeradorFracaoController _barraChocolateControllerPrefab;
    [SerializeField] private GameObject _sinalMaisPrefab;
    [SerializeField] private GameObject _sinalMenosPrefab;
    [SerializeField] private GameObject _sinalIgualPrefab; 

    public void ValidaSolucao()
    {
        var todasFracoesEsperadas = (new List<Fracao>()).Concat(_fracoesEsperadasLadoEsquerdo).Concat(_fracoesEsperadasLadoDireito).ToList();
        var todasFracoesInformadas = (new List<Fracao>()).Concat(_pizzaControllersLadoEsquerdo.Select(g => g.Fracao)).Concat(_pizzaControllersLadoDireito.Select(g => g.Fracao)).ToList();

        if (todasFracoesEsperadas.Count != todasFracoesInformadas.Count) return; // ERRO

        for (var i = 0; i < todasFracoesEsperadas.Count; i++)
        {
            if (!todasFracoesEsperadas[i].SaoEquivalentes(todasFracoesInformadas[i]))
            {
                TrataSolucao(AcertoErroUIController.TipoResultado.Errou);
                return;
            }
        }

        TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
    }

    private bool ValidaResposta(List<Fracao> fracoesInformadas, List<Fracao> fracoesEsperadas)
    {
        foreach (var fracaoInformada in fracoesInformadas)
        {
            if (!fracoesEsperadas.Any(f => f.SaoEquivalentes(fracaoInformada))) return false;
        }

        return true;
    }

    public void SetOnAtividadeConcluida(Action callback)
    {
        OnAtividadeConcluida += callback;
    }

    private void InstanciaPizzas(GeradorFracaoController prefab, List<ElementoAtividadePizza> ladoEsquerdo, List<ElementoAtividadePizza> ladoDireito)
    {
        _containerPizzas.DestroyChildren();

        _pizzaControllersLadoEsquerdo.Clear();
        _pizzaControllersLadoDireito.Clear();

        int total = ladoEsquerdo.Count + ladoDireito.Count;

        Vector3 posicaoInicial = Vector3.forward * ((total * _larguraPizzaController + _espacoEntreObjetos * (total- 1)) * 0.5f - _larguraPizzaController/2f);
        Vector3 passo = Vector3.forward * (-_larguraPizzaController - _espacoEntreObjetos);

        for (int i = 0; i < total; i++)
        {
            var controller = Instantiate(prefab, _containerPizzas);
            controller.transform.localPosition = posicaoInicial + passo * i;
            controller.transform.localRotation = Quaternion.identity;

            controller.Reseta();

            if (i < ladoEsquerdo.Count)
            {
                _pizzaControllersLadoEsquerdo.Add(controller);
                controller.SetParametrosGerador(ladoEsquerdo[i]._mostrarPedacosComidos, ladoEsquerdo[i]._permitirNumeradorMaiorDenominador, ladoEsquerdo[i]._atualizarPosicao);

                GameObject sinal = null;

                if (ladoEsquerdo[i]._subtrairProximo)
                {
                    sinal = Instantiate(_sinalMenosPrefab, _containerPizzas);
                }
                else if (ladoEsquerdo[i]._adicionarProximo)
                {
                    sinal = Instantiate(_sinalMaisPrefab, _containerPizzas);
                }
                else if (ladoDireito.Count != 0)
                {
                    sinal = Instantiate(_sinalIgualPrefab, _containerPizzas);
                }

                if (sinal != null) sinal.transform.localPosition = controller.transform.localPosition + passo * 0.5f;
            }
            else
            {
                _pizzaControllersLadoDireito.Add(controller);
                var elemento = ladoDireito[i - ladoEsquerdo.Count];

                controller.SetParametrosGerador(elemento._mostrarPedacosComidos, elemento._permitirNumeradorMaiorDenominador, elemento._atualizarPosicao);

                GameObject sinal = null;

                if (ladoDireito[i - ladoEsquerdo.Count]._subtrairProximo)
                {
                    sinal = Instantiate(_sinalMenosPrefab, _containerPizzas);
                }
                else if (ladoDireito[i - ladoEsquerdo.Count]._adicionarProximo)
                {
                    sinal = Instantiate(_sinalMaisPrefab, _containerPizzas);
                }

                if (sinal != null) sinal.transform.localPosition = controller.transform.localPosition + passo * 0.5f;
            }
        }
    }

    public override void IniciaAtividadeController(ScriptableObject atividade)
    {
        var ativiadePizzaSO = atividade as AtividadePizzaSO;

        _fracoesEsperadasLadoEsquerdo = ativiadePizzaSO._fracoesDesejadasLadoEsquerdo.Select(f => f._fracao).ToList();
        _fracoesEsperadasLadoDireito = ativiadePizzaSO._fracoesDesejadasLadoDireito.Select(f => f._fracao).ToList();

        var prefab = ativiadePizzaSO._formatoDeExibicao == AtividadePizzaSO.FormatoDeExibicao.Pizza ? _pizzaControllerPrefab : _barraChocolateControllerPrefab;

        InstanciaPizzas(prefab, ativiadePizzaSO._fracoesDesejadasLadoEsquerdo, ativiadePizzaSO._fracoesDesejadasLadoDireito);
    }

    internal void SetView(bool v3)
    {
        _view.SetActive(v3);
    }
}
