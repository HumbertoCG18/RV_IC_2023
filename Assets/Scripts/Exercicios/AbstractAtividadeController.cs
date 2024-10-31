using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AcertoErroUIController;

public abstract class AbstractAtividadeController : MonoBehaviour
{
    private const float TEMPO_DE_ANIMACAO = 3f;

    [Header("Parametros Resultado UI")]
    [SerializeField] protected AcertoErroUIController _acertouErrouUIController;

    public Action OnAtividadeConcluida;
    public Action<bool> OnValidaResposta;

    private bool _atividadeConcluida = false;

    public abstract void CarregaAtividade(ScriptableObject atividade);

    public virtual void SetOnAtividadeConcluida(Action onAtividadeConcluida, Action<bool> onValidaResposta=null)
    {
        Debug.Log($"[AbstractAtividadeController][SetOnAtividadeConcluida]", gameObject);
        OnAtividadeConcluida += onAtividadeConcluida;
        OnValidaResposta += onValidaResposta;
    }

    public void TrataSolucao(TipoResultado resultado, Action callback = null, bool confirmarAtividade = true, float tempoDeAnimacao=TEMPO_DE_ANIMACAO, bool tocarSom=true)
    {
        if (_atividadeConcluida) return;

        Debug.Log("TrataSolucao");
        switch (resultado)
        {
            case TipoResultado.Acertou:
                _atividadeConcluida = confirmarAtividade;

                if (_atividadeConcluida)
                {
                    _acertouErrouUIController.ExibeAcerto(() => {
                        _atividadeConcluida = false;
                        callback?.Invoke(); 
                        OnAtividadeConcluida?.Invoke();
                        Debug.Log($"Atividade concluida    {OnAtividadeConcluida}");

                    }, tocarSom, tempoDeAnimacao);
                }
                else
                {
                    _acertouErrouUIController.ExibeAcerto(() => callback?.Invoke(), tocarSom, tempoDeAnimacao);
                }

                break;

            case TipoResultado.Errou: _acertouErrouUIController.ExibeErro(callback, tocarSom, tempoDeAnimacao); break;
            case TipoResultado.Aviso: _acertouErrouUIController.ExibeAviso(callback, tocarSom, tempoDeAnimacao); break;
        }
    }
}
