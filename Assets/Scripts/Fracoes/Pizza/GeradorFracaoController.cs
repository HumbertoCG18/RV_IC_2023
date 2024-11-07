using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GeradorFracaoController : MonoBehaviour, IFracao
{
    private static int DENOMINADOR_MAXIMO = 20;

    [SerializeField] protected GameObject _linhaPrefab;

    [SerializeField] protected RectTransform _mainCanvasRectTransform;
    [SerializeField] protected int _larguraCanvas = 300;
    [SerializeField] protected RectTransform _itensContainerRectTransform;

    [SerializeField] protected FracaoUIController _fracaoUIController;
    [SerializeField] protected GameObject _viewOriginalPrefab;
    [SerializeField] protected GameObject _sinalMaisPrefab;
                     
    [Header("UI")]   
    [SerializeField] protected Button _btnAumentaNumerador;
    [SerializeField] protected Button _btnDiminuiNumerador;
    [SerializeField] protected Button _btnAumentaDenominador;
    [SerializeField] protected Button _btnDiminuiDenominador;

    protected Fracao _fracao = new Fracao(1,1);
    public Action<Fracao> OnValorMudou;
    protected bool _inverterExibicaoDeUnidades = false;
    protected bool _permitirNumeradorMaiorDenominador = false;
    protected bool _atualizarPosicaoQuandoCanvasMudar = false;

    public UnityEvent<Vector2> OnCanvasMudouDeTamanho;


    private void Update()
    {
        if (Keyboard.current[Key.P].wasReleasedThisFrame)
        {
            AtualizaFracaoUI();
        }
    }

    public virtual void AtualizaFracaoUI()
    {
        _itensContainerRectTransform.DestroyChildren();

        // Altera tamanho do canvas original
        int qtdItens = Mathf.Max(1, Mathf.CeilToInt(_fracao.ValorReal));
        int novoTamanhoCanvas = qtdItens * _larguraCanvas;

        if (_atualizarPosicaoQuandoCanvasMudar && novoTamanhoCanvas != _mainCanvasRectTransform.rect.width)
        {
            float diferencaLargura = (novoTamanhoCanvas - _mainCanvasRectTransform.rect.width)/2f;

            transform.localPosition += Vector3.back * diferencaLargura * _mainCanvasRectTransform.lossyScale.x;
        }

        _mainCanvasRectTransform.sizeDelta = new Vector2(novoTamanhoCanvas, _mainCanvasRectTransform.sizeDelta.y);
        OnCanvasMudouDeTamanho?.Invoke(_mainCanvasRectTransform.sizeDelta);

        int numerador = _fracao._numerador;

        do
        {
            var instanciaParent = Instantiate(_viewOriginalPrefab, _itensContainerRectTransform);

            float percPreenchimento = Mathf.Min(1f, numerador / (float)_fracao._denominador);

            var containerLinhas = instanciaParent.GetChildByName("ContainerLinhas").GetComponent<RectTransform>();
            var imagemPizza = instanciaParent.GetChildByName("IMG Pizza").GetComponent<Image>();

            FormataCelula(instanciaParent, containerLinhas, imagemPizza, percPreenchimento);

            numerador -= _fracao._denominador;

            if (numerador > 0) Instantiate(_sinalMaisPrefab, _itensContainerRectTransform);

        } while (numerador > 0);

        AtualizaFracao();
        OnValorMudou?.Invoke(_fracao);
    }

    private void FormataCelula(GameObject instancia, RectTransform containerLinhas, Image imagePizza, float percPreenchimento)
    {
        if (_fracao._denominador != 1)
        {
            float anguloPorPedaco = 360f / (float)_fracao._denominador;

            for (int i = 0; i < _fracao._denominador; i++)
            {
                var instanciaLinha = Instantiate(_linhaPrefab, containerLinhas).transform;

                instanciaLinha.localPosition = Vector3.zero;
                instanciaLinha.Rotate(Vector3.forward, anguloPorPedaco * i);
                instanciaLinha.gameObject.SetActive(true);
            }
        }

        imagePizza.fillAmount = percPreenchimento;
    }

    public void SetParametrosGerador(bool pedacaoComidos, bool permitirNumeradorMaiorDenominador, bool atulizarPosicao)
    {
        _inverterExibicaoDeUnidades = pedacaoComidos;
        _permitirNumeradorMaiorDenominador = permitirNumeradorMaiorDenominador;
        _atualizarPosicaoQuandoCanvasMudar = atulizarPosicao;

        AtualizaFracaoUI();
    }

    public void AtualizaFracao()
    {
        _fracaoUIController.AtualizaUI();

        _btnAumentaNumerador.gameObject.SetActive(_fracao._numerador < LimiteNumerador);
        _btnDiminuiNumerador.gameObject.SetActive(_fracao._numerador > 0);
        _btnAumentaDenominador.gameObject.SetActive(_fracao._denominador < DENOMINADOR_MAXIMO);
        _btnDiminuiDenominador.gameObject.SetActive(_fracao._denominador > 1);
    }

    public void AumentaNumerador()
    {
        //_fracao._numerador = Mathf.Min(_fracao._denominador, _fracao._numerador + 1);
        _fracao._numerador = Mathf.Min(LimiteNumerador, _fracao._numerador + 1);

        AtualizaFracaoUI();
    }

    public void DiminuiNumerador()
    {
        _fracao._numerador = Mathf.Max(0, _fracao._numerador - 1);

        AtualizaFracaoUI();
    }

    public void AumentarDenominador()
    {
        _fracao._denominador = Mathf.Min(_fracao._denominador + 1, DENOMINADOR_MAXIMO);

        AtualizaFracaoUI();
    }


    public void DiminuiDenominador()
    {
        _fracao._denominador = Mathf.Max(1, _fracao._denominador - 1);

        if (!_permitirNumeradorMaiorDenominador)
        {
            _fracao._numerador = Mathf.Min(_fracao._numerador, _fracao._denominador);
        }

        AtualizaFracaoUI();
    }

    public int Numerador()
    {
        return _fracao._numerador;
    }

    public int Denominador()
    {
        return _fracao._denominador;
    }

    public GameObject Instancia()
    {
        return gameObject;
    }

    public void Reseta()
    {
        _fracaoUIController.SetFracao(this);
        _fracao = new Fracao(1, 1);
        AtualizaFracaoUI();
    }

    private int LimiteNumerador => _permitirNumeradorMaiorDenominador ? DENOMINADOR_MAXIMO : _fracao._denominador;

    public Fracao Fracao => _fracao;
}
