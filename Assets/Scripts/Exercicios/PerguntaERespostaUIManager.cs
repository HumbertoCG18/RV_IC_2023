using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PerguntaERespostaUIManager : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private TextMeshProUGUI _txtDescricaoPergunta;
    [SerializeField] private GameObject _campoAlternativasView;
    [SerializeField] private GameObject _campoRespostaUsuarioView;
    [SerializeField] private Transform _campoAlternativas;
    [SerializeField] private GameObject _alternativaPrefab;

    [Header("Referencias Tela Resultado")]
    [SerializeField] private AcertoErroUIController _acertoErroUIController;
    
    private string _respostaEsperada;
    public Action<bool> OnRespostaDoUsuario;
    private TouchScreenKeyboard _keyboard;
    private TouchScreenKeyboardType _keyboardType = TouchScreenKeyboardType.Default;


    private void Update()
    {
        
        if (Keyboard.current[Key.T].wasPressedThisFrame)
        {
            AlternativaEscolhida(_respostaEsperada);
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
        _txtDescricaoPergunta.text = pergunta._pergunta;
        _respostaEsperada = pergunta._respostaEsperada;

        if (pergunta._alternativas.Count == 0)
        {
            CarregaCampoRespostaUsuario(pergunta._requerRespostaNumerica);
        }
        else
        {
            CarregaCampoAlternativas(pergunta._alternativas);
        }
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
        Debug.Log($"AtivaKeyboard");
    }

    private void CarregaCampoAlternativas(List<string> alternativas)
    {
        _campoRespostaUsuarioView.SetActive(false);
        _campoAlternativasView.SetActive(true);

        int indexAlternativa = 0;
        foreach (Transform botao in _campoAlternativas)
        {
            if (indexAlternativa >= alternativas.Count)
            {
                botao.gameObject.SetActive(false);
            }
            else
            {
                botao.gameObject.SetActive(true);
                botao.GetComponentInChildren<TextMeshProUGUI>().text = alternativas[indexAlternativa];
            }

            indexAlternativa++;
        }
    }

    public void AlternativaEscolhida(string alternativa)
    {
        bool acertou = alternativa == _respostaEsperada;

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
        Debug.Log($"EsperarespostaDoUsuario {_keyboard.text}");
    }
}
