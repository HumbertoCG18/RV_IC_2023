using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.XR.CoreUtils;
using UnityEngine;

public class BalancaController : MonoBehaviour
{
    [Header("Componentes")]
    [SerializeField] private Transform _aste;
    [SerializeField] private Transform _pratoEsquerdo;
    [SerializeField] private Transform _pratoDireito;
    [SerializeField] private Transform _pivotEsquerdoAste;
    [SerializeField] private Transform _pivotDireitoAste;
    [SerializeField] private BalancaPlacaController _placaEsquerdaController;
    [SerializeField] private BalancaPlacaController _placaDireitaController;

    [Header("Componentes UI")]
    [SerializeField] private TextMeshProUGUI _labelPlacaEsquerda;
    [SerializeField] private RectTransform _canvasPlacaEsquerda;
    [SerializeField] private TextMeshProUGUI _labelPlacaDireita;
    [SerializeField] private RectTransform _canvasPlacaDireita;

    [Header("Configuração de parametros")]
    [SerializeField] private float _anguloMaximoAste = 15;
    [SerializeField] private float _sensibilidade = 10f;
    [SerializeField] private float _pesoAtualEsquerdo = 0f;
    [SerializeField] private float _pesoAtualDireito = 0f;

    [Header("Paramentros da Animacao")]
    [SerializeField] private float _tempoDeAnimacao = 0.4f;

    private float _anguloAntigo = 0f;
    private bool _precisaAtualizarBalanca = false;
    private Coroutine _coroutineAtual = null;

    private void Start()
    {
        _labelPlacaEsquerda.text = ((int)_pesoAtualEsquerdo).ToString();
        _labelPlacaDireita.text = ((int)_pesoAtualDireito).ToString();
    }

    private void LateUpdate()
    {
        if (_precisaAtualizarBalanca)
        {
            AtualizaBalanca();
            _precisaAtualizarBalanca = false;
        }
    }

    public void AtualizaBalanca()
    {
        float diferenaDePeso = _pesoAtualEsquerdo - _pesoAtualDireito;
        float magnitudeDaDiferenca = Mathf.Abs(diferenaDePeso);

        float novoAnguloAste = Mathf.Min(1f, magnitudeDaDiferenca / _sensibilidade) * _anguloMaximoAste;

        if ((_pesoAtualEsquerdo == 0 || _pesoAtualDireito == 0) && _pesoAtualDireito != _pesoAtualEsquerdo) novoAnguloAste = _anguloMaximoAste;

        if (diferenaDePeso > 0)
        {
            novoAnguloAste *= -1;
        }

        if (_coroutineAtual != null)
            StopCoroutine(_coroutineAtual);

        _coroutineAtual = StartCoroutine(AnimaRotacaoCoroutine(_anguloAntigo, novoAnguloAste));

        ExibePesoNaPlaca(_placaEsquerdaController, _labelPlacaEsquerda, _canvasPlacaEsquerda);
        ExibePesoNaPlaca(_placaDireitaController, _labelPlacaDireita, _canvasPlacaDireita);
    }

    private void ExibePesoNaPlaca(BalancaPlacaController placa, TextMeshProUGUI txtPeso, RectTransform _canvas)
    {
        var variaveis = placa.Pesos.Where(p => p.IsOculto).ToList();
        var valores = placa.Pesos.Where(p => !p.IsOculto).ToList();
        int tamanhoCanvas = variaveis.Count == 0 ? 100 : 100 * (variaveis.Count + 1);

        txtPeso.text = "";
        _canvas.sizeDelta = new Vector2(tamanhoCanvas, 100);

        if (variaveis.Count > 0)
        {
            for (var i = 0; i < variaveis.Count; i++)
            {
                txtPeso.text += "X";

                if (i < variaveis.Count - 1 || valores.Count != 0)
                {
                    txtPeso.text += " + ";
                }
            }

            if (valores.Count > 0)
            {
                int peso = valores.Sum(p => p.Peso);
                txtPeso.text += peso.ToString();
            }
        }
        else
        {
            txtPeso.text = placa.TotalPeso.ToString();
        }
    }

    public void AtualizaPesos()
    {
        _pesoAtualEsquerdo = _placaEsquerdaController.TotalPeso;
        _pesoAtualDireito = _placaDireitaController.TotalPeso;

        _precisaAtualizarBalanca = true;
    }

    private IEnumerator AnimaRotacaoCoroutine(float anguloAntigo, float anguloAlvo)
    {
        float t = 0f;

        Vector3 anguloAntigoEuler = new Vector3(0, 0, anguloAntigo);
        Vector3 anguloAlvoEuler = new Vector3(0, 0, anguloAlvo);


        while (t < _tempoDeAnimacao)
        {
            Vector3 novoAngulo = Vector3.Lerp(anguloAntigoEuler, anguloAlvoEuler, t / _tempoDeAnimacao);

            _aste.localEulerAngles = novoAngulo;
            _anguloAntigo = novoAngulo.z;

            AtualizaPosicoes();

            yield return null;

            t += Time.deltaTime;
        }

        _aste.localEulerAngles = anguloAlvoEuler;
        AtualizaPosicoes();

        _coroutineAtual = null;
    }

    private void AtualizaPosicoes()
    {
        _pratoEsquerdo.position = _pivotEsquerdoAste.position;
        _pratoDireito.position = _pivotDireitoAste.position;
    }
}
