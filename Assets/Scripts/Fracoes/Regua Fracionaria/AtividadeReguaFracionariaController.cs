using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class AtividadeReguaFracionariaController : AbstractAtividadeController
{
    [SerializeField] private GameObject _view;
    [SerializeField] private GeradorDeReguaFracionariaController _geradorDeReguaFracionariaController;
    [SerializeField] private AcertoErroUIController _acertoErroUIController;

    private int _variacoesMinimas = 0;
    private Fracao _fracaoEsperada;
    private List<Fracao> _historicoDeVariacoes;

    private void Awake()
    {
        _geradorDeReguaFracionariaController.OnFracaoConfirmada += OnFracaoConfirmada;
    }

    public void OnFracaoConfirmada(Fracao fracao)
    {
        if (_geradorDeReguaFracionariaController.QuantidadeReguasSalvas >= _variacoesMinimas) return;

        if (VariacaoJaUsada(fracao))
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Aviso);
            return;
        }

        if (_fracaoEsperada.SaoEquivalentes(fracao))
        {
            _geradorDeReguaFracionariaController.ArmazenaRegua();
            _historicoDeVariacoes.Add(fracao);

            if (_geradorDeReguaFracionariaController.QuantidadeReguasSalvas >= _variacoesMinimas)
            {
                _geradorDeReguaFracionariaController.SalvaReguasAtuais();

                TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
            }
        }
        else
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Errou);
        }
    }

    private bool VariacaoJaUsada(Fracao fracao)
    {
        return _historicoDeVariacoes.Any(f => f._numerador == fracao._numerador && f._denominador == fracao._denominador);
    }

    public override void CarregaAtividade(ScriptableObject atividade)
    {
        var atividadeReguaSO = atividade as AtividaderReguaFracionariaSO;

        _historicoDeVariacoes = new List<Fracao>();
        _fracaoEsperada = atividadeReguaSO._fracaoEquivalenteDesejada;
        _variacoesMinimas = atividadeReguaSO._quantidadeDeVariacoesNecessarias;

        _geradorDeReguaFracionariaController.IniciaGerador();
    }

    internal void SetView(bool v2)
    {
        _view.SetActive(v2);
    }
}
