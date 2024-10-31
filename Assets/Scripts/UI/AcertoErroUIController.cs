using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcertoErroUIController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private GameObject _imgAcertou;
    [SerializeField] private GameObject _imgErrou;
    [SerializeField] private GameObject _imgAviso;

    [SerializeField] private AudioDescricao _audioAcertou;
    [SerializeField] private AudioDescricao _audioErrou;
    [SerializeField] private AudioDescricao _audioTentativa;

    public enum TipoResultado { Acertou, Errou, Aviso };

    private Coroutine _coroutineAtual;

    private void Awake()
    {
        _view.SetActive(false);
    }

    public void ExibeAcerto(Action callback=null, bool tocarSom = true, float tempoAnimacao = 2.5f)
    {
        IniciaCoroutine(AnimaExibicaoResultadoCoroutine(TipoResultado.Acertou, tempoAnimacao, callback, tocarSom));
    }

    public void ExibeAviso(Action callback = null, bool tocarSom = false, float tempoAnimacao = 2.5f)
    {
        IniciaCoroutine(AnimaExibicaoResultadoCoroutine(TipoResultado.Aviso, tempoAnimacao, callback, tocarSom));
    }

    public void ExibeErro(Action callback, bool tocarSom=false, float tempoAnimacao = 2.5f)
    {
        IniciaCoroutine(AnimaExibicaoResultadoCoroutine(TipoResultado.Errou, tempoAnimacao, callback, tocarSom));
    }

    private void IniciaCoroutine(IEnumerator coroutine)
    {
        if (_coroutineAtual != null)
        {
            StopCoroutine(_coroutineAtual);
        }

        _coroutineAtual = StartCoroutine(coroutine);
    }

    public IEnumerator AnimaExibicaoResultadoCoroutine(TipoResultado resultado, float tempoDeAnimacao, Action callback, bool tocarSom)
    {
        _imgAcertou.SetActive(resultado == TipoResultado.Acertou);
        _imgErrou.SetActive(resultado == TipoResultado.Errou);
        _imgAviso.SetActive(resultado == TipoResultado.Aviso);

        _view.SetActive(true);

        if (tocarSom)
        {
            switch (resultado)
            {
                case TipoResultado.Acertou: AudioManager.Instance.PlayDescricao(_audioAcertou); break;
                case TipoResultado.Errou: AudioManager.Instance.PlayDescricao(_audioErrou); break;
                case TipoResultado.Aviso: AudioManager.Instance.PlayDescricao(_audioTentativa); break;
            }
        }

        yield return new WaitForSeconds(tempoDeAnimacao);

        _view.SetActive(false);
        callback?.Invoke();
    }
}
