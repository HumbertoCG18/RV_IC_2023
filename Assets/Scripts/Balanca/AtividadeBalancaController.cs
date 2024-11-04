using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AtividadeBalancaController : AbstractAtividadeController
{
    [Header("Componentes UI")]
    [SerializeField] private TextMeshProUGUI _txtDescricaoExercicio;
    [SerializeField] private TextMeshProUGUI _txtprogressaoCombinacoes;
    [SerializeField] private TextMeshProUGUI _txtDescricaoErro;
    [SerializeField] private GameObject _telaDeErro;

    [Header("Componentes")]
    [SerializeField] private List<Transform> _pivosLadoEsquerdo;
    [SerializeField] private List<Transform> _pivosLadoDireito;
    [SerializeField] private BalancaPlacaController _placaEsquerdaController;
    [SerializeField] private BalancaPlacaController _placaDireitaController;
    [SerializeField] private Transform _objetosDoExercicioParent;
    [SerializeField] private List<PrateleiraController> _prateleirasControllers;
    [SerializeField] private GeradorDePesosController _geradorDePesosController;

    [Header("Prefabs")]
    [SerializeField] private GameObject _pesoEsquerdoPrefab;
    [SerializeField] private GameObject _pesoDireitoPrefab;

    [Header("Parametros")]
    [SerializeField] private Color _corPesosDaEsquerda;
    [SerializeField] private Color _corPesosDaDireita;

    [Header("SFX")]
    [SerializeField] private float _tempoDeAnimacaoSFX = 2f;
    [SerializeField] private AudioSource _audioSourcer;
    [SerializeField] private AudioClip _sfxAcertou;
    [SerializeField] private AudioClip _sfxErrou;
    [SerializeField] private AudioClip _sfxAviso;

    [Header("Managers")]
    [SerializeField] private RegistradorDeTentativasManager _registradorDeTentativasManager;

    private int _nivelExercicio = 1;
    private int _combinacoesCorretas;
    private int _combinacoesMinimas;
    private GameObject _instanciaDosPesosPreDefinidos;

    private void InstanciaPesos(List<AtividadeBalancaSO.PesoBalanca> pesos, GameObject prefab, List<Transform> pivos, Color cor, bool podeAlterarPesos)
    {
        int i = 0;
        foreach (var peso in pesos)
        {
            BalancaPesoController instanciaPesoController = Instantiate(prefab, _objetosDoExercicioParent).GetComponent<BalancaPesoController>();
            instanciaPesoController.SetInfo(peso._valorPeso, peso._isOculto, cor, podeAlterarPesos);

            instanciaPesoController.transform.position = pivos[i].position;
            instanciaPesoController.transform.rotation = pivos[i].rotation;

            i = (i + 1) % pivos.Count;
        }
    }

    private void AtualizaCombinacoes()
    {
        _txtprogressaoCombinacoes.text = $"Você acerto {_combinacoesCorretas} de {_combinacoesMinimas} combinacões até agora.";
    }

    public void ExibeErro(string mensagem)
    {
        _txtDescricaoErro.text = mensagem;
        _telaDeErro.SetActive(true);
    }

    public void AplicaCorrecao()
    {
        if (_atividadeConcluida) return;

        bool acertou = false;
        bool tentativaJaUsada = false;

        if (_nivelExercicio == 1)
        {
            int pesoEsquerdo = _placaEsquerdaController.TotalPeso;
            int pesoDireito = _placaDireitaController.TotalPeso;

            acertou = pesoEsquerdo == pesoDireito;

        }
        else if (_nivelExercicio == 2 || _nivelExercicio == 3)
        {
            if (_placaDireitaController.ContemPesoOculto)
            {
                acertou = VerificaResposta(_placaDireitaController, _placaEsquerdaController);
            }
            else
            {
                acertou = VerificaResposta(_placaEsquerdaController, _placaDireitaController);
            }
        }

        if (acertou)
        {
            /*
            if (_placaDireitaController.ContemPesoOculto)
            {
                tentativaJaUsada = !_registradorDeTentativasManager.AtualizaTentativas(_placaEsquerdaController.Pesos);
            }
            else
            {

                tentativaJaUsada = !_registradorDeTentativasManager.AtualizaTentativas(_placaDireitaController.Pesos);
            }*/

            tentativaJaUsada = !_registradorDeTentativasManager.AtualizaTentativas(_placaEsquerdaController.Pesos, _placaDireitaController.Pesos, _nivelExercicio != 1);

            if (!tentativaJaUsada)
            {
                _combinacoesCorretas += 1;

                PlayEffeitoSonoro(_sfxAcertou);
                if (_combinacoesCorretas == _combinacoesMinimas)
                {
                    TrataSolucao(AcertoErroUIController.TipoResultado.Acertou);
                }
                else
                {
                    TrataSolucao(AcertoErroUIController.TipoResultado.Acertou, _tempoDeAnimacaoSFX, () =>
                    {
                        AtualizaCombinacoes();
                        ResetaPesosPreDefinidos();
                    }, false, false);
                }
            }
            else
            {
                PlayEffeitoSonoro(_sfxAviso);
                TrataSolucao(AcertoErroUIController.TipoResultado.Aviso, _tempoDeAnimacaoSFX, null, false);
            }
        }
        else
        {
            PlayEffeitoSonoro(_sfxErrou);
            TrataSolucao(AcertoErroUIController.TipoResultado.Errou, _tempoDeAnimacaoSFX, null, false, false);
        }
    }

    private bool VerificaResposta(BalancaPlacaController placaEsquerdaController, BalancaPlacaController placaDireitaController)
    {
        int pesoEsquerdo = placaEsquerdaController.TotalPeso;
        int pesoDireito = placaDireitaController.TotalPeso;

        var pesosEsquerdo = placaEsquerdaController.Pesos;
        var pesosDireito = placaDireitaController.Pesos;

        bool ladoEsquerdoOk = pesosEsquerdo.Count == 1 && pesosEsquerdo[0].IsOculto;

        bool ladoDireitoOk = true;
        foreach (var peso in pesosDireito)
        {
            if (peso.IsOculto)
            {
                ladoDireitoOk = false;
                break;
            }
        }

        return (pesoEsquerdo == pesoDireito) && ladoEsquerdoOk && ladoDireitoOk;
    }

    private void PlayEffeitoSonoro(AudioClip effeitoSonoro)
    {
        _audioSourcer.clip = effeitoSonoro;
        _audioSourcer.Play();
    }

    private void ResetaPesosPreDefinidos()
    {
        _prateleirasControllers.ForEach(p => p.ResetarPosicoes());
        _geradorDePesosController.LimpaInstancias();

        _placaDireitaController.ResetarPesos();
    }

    public override void IniciaAtividadeController(ScriptableObject atividade)
    {
        var atividadeBalancaSO = atividade as AtividadeBalancaSO;

        // Destroi objetos instanciados no exercicio anterior
        foreach (Transform objeto in _objetosDoExercicioParent) Destroy(objeto.gameObject);

        // Reseta posicao dos pesos pre definidos
        ResetaPesosPreDefinidos();

        // Zera contador de combinacoes corretas
        _combinacoesCorretas = 0;

        // Atualiza combinacoes
        AtualizaCombinacoes();

        // Reseta Pesos da placa esquerda
        _placaEsquerdaController.ResetarPesos();

        // Instancia pesos nas placas da balanca
        InstanciaPesos(atividadeBalancaSO._pesosLadoEsquerdo, _pesoEsquerdoPrefab, _pivosLadoEsquerdo, _corPesosDaEsquerda, atividadeBalancaSO._podeAlterarLadoEsquerdo);
        InstanciaPesos(atividadeBalancaSO._pesosLadoDireito, _pesoDireitoPrefab, _pivosLadoDireito, _corPesosDaDireita, atividadeBalancaSO._podeAlterarLadoDireito);

        // Seta nivel da atividade
        _nivelExercicio = atividadeBalancaSO._nivel;

        // Seta combinacoes minimas
        _combinacoesMinimas = atividadeBalancaSO._minimoTentativas;

        // Limpa registro de tentativas
        _registradorDeTentativasManager.LimpaTentativas();
    }
}
