using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtividadeElevadorController : AbstractAtividadeController
{
    [SerializeField] private Transform _atividadeContainer;
    [SerializeField] private ElevadorController _elevadorController;
    [SerializeField] private GameObject _uiTelaResultado2;
    [SerializeField] private List<AreaDosObjetosController> _areasDeObjetosFixas;

    public override void IniciaAtividadeController(ScriptableObject atividade)
    {
        _atividadeContainer.DestroyChildren();

        var atividadeSO = atividade as AtividadeElevadorSO;

        var instanciaAtividade = Instantiate(atividadeSO._nivelPrefab, _atividadeContainer);

        instanciaAtividade.OnNivelConcluido += () => {
            TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
            _uiTelaResultado2.SetActive(true);
            _elevadorController.SetPrecisaValidarAreas(false);
            _atividadeContainer.gameObject.SetActive(false);
        };

        instanciaAtividade.ConfiguraNivel();
        _areasDeObjetosFixas.ForEach(a => a.ResetArea());
        _uiTelaResultado2.SetActive(false);
        _elevadorController.SetPrecisaValidarAreas(true);
        _atividadeContainer.gameObject.SetActive(true);
    }
}
