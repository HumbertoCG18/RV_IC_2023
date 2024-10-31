using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AtividadePitagorasController : AbstractAtividadeController
{
    [SerializeField] private TextMeshProUGUI _txtDescricaoAtividade;
    [SerializeField] private PerguntaERespostaUIManager _perguntaERespostaUIManager;

    [SerializeField] private AcertoErroUIController _certoErroUIController;

    [SerializeField] private Transform _nivelControllerParent;
    private IteratorController<PerguntaERespostaSO> _iteradorPerguntaERespostaSO;
    private PitagorasNivelController _nivelAtualController;

    private void OnRespostaDoUsuario(bool acertou)
    {
        if (!acertou) return;

        if (_iteradorPerguntaERespostaSO.IsLast)
        {
            _perguntaERespostaUIManager.SetVisibilidade(false);

            TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
        }
        else
        {
            _iteradorPerguntaERespostaSO.Next();
            IniciaPergunta(_iteradorPerguntaERespostaSO.Current);
        }
    }

    private void OnNivelConcluido()
    {
        if (_iteradorPerguntaERespostaSO.Count != 0)
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Acertou, () => IniciaPergunta(_iteradorPerguntaERespostaSO.Current), false);
        }
        else
        {
            TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
        }
    }

    private void IniciaPergunta(PerguntaERespostaSO pergunta)
    {
        _perguntaERespostaUIManager.SetVisibilidade(true);
        _perguntaERespostaUIManager.CarregaPergunta(pergunta);
    }

    private void OnEnable()
    {
        _perguntaERespostaUIManager.OnRespostaDoUsuario += OnRespostaDoUsuario;
    }

    private void OnDisable()
    {
        _perguntaERespostaUIManager.OnRespostaDoUsuario -= OnRespostaDoUsuario;
    }

    public override void CarregaAtividade(ScriptableObject atividade)
    {
        var atividadePitagoras = atividade as AtividadePitagorasSO;

        _iteradorPerguntaERespostaSO = new IteratorController<PerguntaERespostaSO>(atividadePitagoras._perguntas);

        _txtDescricaoAtividade.text = atividadePitagoras._descricao;

        StartCoroutine(IniciaNivelCoroutine(atividadePitagoras._nivelControllerPrefab));
    }

    private IEnumerator IniciaNivelCoroutine(PitagorasNivelController controller)
    {
        if (_nivelAtualController != null)
        {
            _nivelAtualController.DesativaEncaixes();

            yield return null;

            Destroy(_nivelAtualController.gameObject);
        }

        _nivelAtualController = Instantiate(controller, _nivelControllerParent);
        _nivelAtualController.transform.ResetTransformation();
        _nivelAtualController.OnNivelConcluido += OnNivelConcluido;
    }
}
