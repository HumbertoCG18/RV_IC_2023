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
    [SerializeField] private PizzaController _pizzaControllerPrefab;
    [SerializeField] private float _espacoEntreObjetos = 0.01f;

    [SerializeField] private AcertoErroUIController _acertoErroUIController;

    [Header("Parametros")]
    [SerializeField] private float _larguraPizzaController = 0.6f;

    [SerializeField] private List<PizzaController> _pizzaControllersLadoEsquerdo = new List<PizzaController>();
    [SerializeField] private List<PizzaController> _pizzaControllersLadoDireito = new List<PizzaController>();

    [SerializeField] private List<Fracao> _fracoesEsperadasLadoEsquerdo;
    [SerializeField] private List<Fracao> _fracoesEsperadasLadoDireito;

    [Header("Prefabs")]
    [SerializeField] private GameObject _sinalMaisPrefab; 
    [SerializeField] private GameObject _sinalIgualPrefab; 

    public void ValidaSolucao()
    {
        bool ladoEsquerdoOk = ValidaResposta(_fracoesEsperadasLadoEsquerdo, _pizzaControllersLadoEsquerdo.Select(c => c.Fracao).ToList());
        bool ladoDireitoOk = ValidaResposta(_fracoesEsperadasLadoDireito, _pizzaControllersLadoDireito.Select(c => c.Fracao).ToList());

        if (ladoEsquerdoOk && ladoEsquerdoOk)
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
        }
        else
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Errou);
        }
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

    private void InstanciaPizzas(List<ElementoAtividadePizza> ladoEsquerdo, List<ElementoAtividadePizza> ladoDireito)
    {
        _containerPizzas.DestroyChildren();

        _pizzaControllersLadoEsquerdo.Clear();
        _pizzaControllersLadoDireito.Clear();

        int total = ladoEsquerdo.Count + ladoDireito.Count;

        Vector3 posicaoInicial = Vector3.forward * ((total * _larguraPizzaController + _espacoEntreObjetos * (total- 1)) * 0.5f - _larguraPizzaController/2f);
        Vector3 passo = Vector3.forward * (-_larguraPizzaController - _espacoEntreObjetos);


        for (int i = 0; i < total; i++)
        {
            var controller = Instantiate(_pizzaControllerPrefab, _containerPizzas);
            controller.transform.localPosition = posicaoInicial + passo * i;
            controller.transform.localRotation = Quaternion.identity;

            controller.Reseta();

            if (i < ladoEsquerdo.Count)
            {
                _pizzaControllersLadoEsquerdo.Add(controller);
                controller.SetFormatoDeExibicao(ladoEsquerdo[i]._mostrarPedacosComidos);

                if (i < ladoEsquerdo.Count - 1)
                {
                    var sinalMais = Instantiate(_sinalMaisPrefab, _containerPizzas);
                    sinalMais.transform.localPosition = controller.transform.localPosition + passo * 0.5f;
                }
            }
            else
            {
                _pizzaControllersLadoDireito.Add(controller);
                controller.SetFormatoDeExibicao(ladoDireito[i - ladoEsquerdo.Count]._mostrarPedacosComidos);

            }

            if (i == ladoEsquerdo.Count - 1 && ladoDireito.Count != 0)
            {
                var sinalIgual = Instantiate(_sinalIgualPrefab, _containerPizzas);
                sinalIgual.transform.localPosition = controller.transform.localPosition + passo * 0.5f;
            }
        }
    }

    public override void CarregaAtividade(ScriptableObject atividade)
    {
        var ativiadePizzaSO = atividade as AtividadePizzaSO;

        _fracoesEsperadasLadoEsquerdo = ativiadePizzaSO._fracoesDesejadasLadoEsquerdo.Select(f => f._fracao).ToList();
        _fracoesEsperadasLadoDireito = ativiadePizzaSO._fracoesDesejadasLadoDireito.Select(f => f._fracao).ToList();

        InstanciaPizzas(ativiadePizzaSO._fracoesDesejadasLadoEsquerdo, ativiadePizzaSO._fracoesDesejadasLadoDireito);
    }

    internal void SetView(bool v3)
    {
        _view.SetActive(v3);
    }
}
