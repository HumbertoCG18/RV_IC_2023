using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtividadeFracoesHelper : AbstractAtividadeController
{
    [SerializeField] private AtividadeChocolateController _atividadeChocolateController;
    [SerializeField] private AtividadeReguaFracionariaController _atividadeReguaFracionariaController;
    [SerializeField] private AtividadePizzaController _atividadePizzaController;

    [SerializeField] private GameObject _historicoDeReguasView;

    public override void IniciaAtividadeController(ScriptableObject atividade)
    {
        if (atividade is AtividadeChocolateSO)
        {
            SetViews(true, false, false);
            _historicoDeReguasView.SetActive(false);
            _atividadeChocolateController.CarregaAtividade(atividade);
        }
        else if (atividade is AtividaderReguaFracionariaSO)
        {
            SetViews(false, true, false);
            _historicoDeReguasView.SetActive(true);
            _atividadeReguaFracionariaController.CarregaAtividade(atividade);
        }
        else if (atividade is AtividadePizzaSO)
        {
            SetViews(false, false, true);
            _historicoDeReguasView.SetActive(true);
            _atividadePizzaController.CarregaAtividade(atividade);
        }
        else
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
        }
    }

    public void SetViews(bool v1, bool v2, bool v3)
    {
        _atividadeChocolateController.SetView(v1);
        _atividadeReguaFracionariaController.SetView(v2);
        _atividadePizzaController.SetView(v3);
    }

    public override void SetOnAtividadeConcluida(Action callback, Action<bool> OnValidaSolucao=null)
    {
        Debug.Log($"[AtividadeFracoesHelper][SetOnAtividadeConcluida]");
        _atividadeChocolateController.SetOnAtividadeConcluida(callback);
        _atividadeReguaFracionariaController.SetOnAtividadeConcluida(callback);
        _atividadePizzaController.SetOnAtividadeConcluida(callback);
    }
}
