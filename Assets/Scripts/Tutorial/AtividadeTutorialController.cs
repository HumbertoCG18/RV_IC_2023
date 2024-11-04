using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtividadeTutorialController : AbstractAtividadeController
{
    [SerializeField] private Transform _nivelParent;

    private NivelTutorialController _instanciaController;

    public override void IniciaAtividadeController(ScriptableObject atividade)
    {
        StartCoroutine(IniciaAtividadeCoroutine(atividade as AtividadeTutorialSO));
    }

    private IEnumerator IniciaAtividadeCoroutine(AtividadeTutorialSO atividadeTutorialSO)
    {
        if (_instanciaController != null)
        {
            _instanciaController.FinalizaProcessos();
        }

        yield return null;

        _nivelParent.DestroyChildren();

        _instanciaController = Instantiate(atividadeTutorialSO._nivelController, _nivelParent);
        _instanciaController.OnNivelConcluido += () => TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
    }
}
