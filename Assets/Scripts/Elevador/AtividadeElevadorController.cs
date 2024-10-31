using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtividadeElevadorController : AbstractAtividadeController
{
    [SerializeField] private GameObject _atividadeView;
    [SerializeField] private AreaDosObjetosController _areaAlvo;
    [SerializeField] private ElevadorController _elevadorController;

    [SerializeField] private GameObject _uiTelaResultado2;

    private void Awake()
    {
        _areaAlvo.OnMudancaDeEstado += OnEstadoDaAreaMudou;
    }

    private void OnEstadoDaAreaMudou()
    {
        if (_areaAlvo.QuantidadeDeObjetos == 3)
        {
            AtividadeConcluida();

            _uiTelaResultado2.SetActive(true);
            _elevadorController.SetPrecisaValidarAreas(false);
        }
    }

    public void AtividadeConcluida()
    {
        OnAtividadeConcluida?.Invoke();
    }

    public override void CarregaAtividade(ScriptableObject atividade)
    {
        _atividadeView.SetActive(true);
        _uiTelaResultado2.SetActive(false);
        _elevadorController.SetPrecisaValidarAreas(true);
    }

    public void SetOnAtividadeConcluida(Action onAtividadeConcluida)
    {
        OnAtividadeConcluida += onAtividadeConcluida;
    }
}
