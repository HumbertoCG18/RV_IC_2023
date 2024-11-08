using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PerguntaERespostaUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _txtDescricaoPergunta;
    [SerializeField] private GameObject _campoAlternativasView;
    [SerializeField] private GameObject _campoRespostaUsuarioView;
    [SerializeField] private Transform _campoAlternativas;
    [SerializeField] private GameObject _alternativaPrefab;
    [SerializeField] private List<AudioDescricao> _audioAlternativas;

    [Header("Referencias Tela Resultado")]
    [SerializeField] private AcertoErroUIController _acertoErroUIController;
    
    public Action<bool> OnRespostaDoUsuario;
    private TouchScreenKeyboard _keyboard;
    private TouchScreenKeyboardType _keyboardType = TouchScreenKeyboardType.Default;

    private PerguntaERespostaSO _perguntaERespostaSO;

    private void Update()
    {
        if (Keyboard.current[Key.T].wasPressedThisFrame)
        {
            AlternativaEscolhida(_perguntaERespostaSO._respostaEsperada);
        }

        if (Keyboard.current[Key.Y].wasPressedThisFrame)
        {
            AlternativaEscolhida("");
        }
    }

    public void SetVisibilidade(bool visibilidade)
    {
        _view.SetActive(visibilidade);
    }

    public void CarregaPergunta(PerguntaERespostaSO pergunta)
    {
        _perguntaERespostaSO = pergunta;
        _txtDescricaoPergunta.text = pergunta._audioDescricao.Descricao;

        if (pergunta._alternativas.Count == 0)
        {
            CarregaCampoRespostaUsuario(pergunta._requerRespostaNumerica);
        }
        else
        {
            CarregaCampoAlternativas(pergunta._alternativas);
        }

        PlayAudio();
    }

    public void PlayAudio()
    {
        AudioManager.Instance.PlayDescricao(_perguntaERespostaSO._audioDescricao);
    }

    private void CarregaCampoRespostaUsuario(bool requerRespostaNumerica)
    {
        _campoAlternativasView.SetActive(false);
        _campoRespostaUsuarioView.SetActive(true);

        _keyboardType = requerRespostaNumerica ? TouchScreenKeyboardType.NumberPad : TouchScreenKeyboardType.Default;
    }

    public void AtivaKeyboard()
    {
        _keyboard = TouchScreenKeyboard.Open("", _keyboardType);

        StartCoroutine(EsperaRespostaDoUsuario(resultado => AlternativaEscolhida(resultado)));
    }

    private void CarregaCampoAlternativas(List<AudioDescricao> alternativas)
    {
        _campoRespostaUsuarioView.SetActive(false);
        _campoAlternativasView.SetActive(true);

        _campoAlternativas.DestroyChildren();

        for (int i = 0; i < alternativas.Count; i++)
        {
            var controller = Instantiate(_alternativaPrefab, _campoAlternativas).GetComponent<AlternativaUIController>();

            controller.IniciaAlternativa(this, i + 1, alternativas[i]);
        }
    }

    public void AlternativaEscolhida(string alternativa)
    {
        bool acertou = alternativa == _perguntaERespostaSO._respostaEsperada;

        if (acertou)
        {
            _acertoErroUIController.ExibeAcerto(() => OnRespostaDoUsuario?.Invoke(acertou));
        }
        else
        {
            _acertoErroUIController.ExibeErro(null, false);
        }
    }

    public void  AlternativaEscolhida(TextMeshProUGUI txt) => AlternativaEscolhida(txt.text);

    private IEnumerator EsperaRespostaDoUsuario(Action<string> callback)
    {
        yield return new WaitUntil(() => _keyboard.status != TouchScreenKeyboard.Status.Visible);

        callback?.Invoke(_keyboard.text);
    }
}
