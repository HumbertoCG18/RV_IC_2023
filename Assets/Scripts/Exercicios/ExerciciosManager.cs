using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class ExerciciosManager : MonoBehaviour
{
    [SerializeField] private IteratorController<ExercicioSO> _listaDeExerciciosController;
    [SerializeField] private AbstractAtividadeController _atividadeControllerInstance;

    [Header("Parametros")]
    [SerializeField] private bool _passarAtividadesAutomaticamente = false;

    [Header("UI Exercicios")]
    [SerializeField] private TextMeshProUGUI _txtDescricaoExercicio;
    [SerializeField] private UIContainerController _containerExercicios;
    [SerializeField] private GameObject _telaExerciciosConcluidos;

    [Header("UI Atividades")]
    [SerializeField] private TextMeshProUGUI _txtDescricaoAtividade;
    [SerializeField] private UIContainerController _containerAtividades;
    [SerializeField] private GameObject _audioDescricaoView;
    [SerializeField] private GameObject _btnProximaAtividade;
    [SerializeField] private GameObject _btnProximoExercicio;

    public UnityEvent<ExerciciosManager> OnExerciciosFinalizados;
    [SerializeField] private bool _exerciciosFinalizados = false;
    private IteratorController<AtividadeSO> _listaDeAtividadesController;

    private List<List<bool>> _historicoDeExerciciosCompletados;

    private void Awake()
    {
        _atividadeControllerInstance.SetOnAtividadeConcluida(OnAtividadeConcluida);

        _historicoDeExerciciosCompletados = new List<List<bool>>();
        for (int i = 0; i < _listaDeExerciciosController.Count; i++)
        {
            _historicoDeExerciciosCompletados.Add(new List<bool>());

            for (int j = 0; j < _listaDeExerciciosController.Values[i]._atividades.Count; j++)
            {
                _historicoDeExerciciosCompletados[i].Add(false);
            }
        }
    }

    public void IniciaExercicios()
    {
        Debug.Log($"[ExerciciosManager][IniciaExercicios]");
        if (_telaExerciciosConcluidos != null) _telaExerciciosConcluidos.SetActive(false);

        _listaDeExerciciosController.Reset();

        CarregaExercicio(0);
    }

    public void CarregaExercicio(int index)
    {
        Debug.Log($"[ExerciciosManager][CarregaExercicio]");

        _listaDeExerciciosController.SetIndex(index);

        var exercicioSO = _listaDeExerciciosController.Current;
        _listaDeAtividadesController = new IteratorController<AtividadeSO>(exercicioSO._atividades);

        if (_listaDeAtividadesController.Count != 0 )
        {
            CarregaAtividades(_listaDeAtividadesController.Index);
        }
        else
        {
            AtualizaInterface(exercicioSO);
        }

    }

    public void CarregaAtividades(int index)
    {
        Debug.Log($"[ExerciciosManager][CarregaAtividade] Carrega Atividade {index}");
        _listaDeAtividadesController.SetIndex(index);

        _atividadeControllerInstance.CarregaAtividade(_listaDeAtividadesController.Current);

        AtualizaInterface(_listaDeExerciciosController.Current);
    }

    public void AtualizaInterface(ExercicioSO exercicioSO)
    {
        Debug.Log($"[ExerciciosManager][AtualizaInterface]");

        if (_txtDescricaoExercicio != null)
        {
            _txtDescricaoExercicio.text = exercicioSO._audioDescricao.Descricao;
        }

        if (_txtDescricaoAtividade != null)
        {
            _txtDescricaoAtividade.text = _listaDeAtividadesController.Current._audioDescricao.Descricao;
        }

        if (_containerExercicios != null)
        {
            _containerExercicios.UpdateContainer(_listaDeExerciciosController.Values, PreprocessamentoExercicios, CarregaExercicio);
        }

        if (_containerAtividades != null)
        {
            _containerAtividades.UpdateContainer(_listaDeAtividadesController.Values, PreprocessamentoAtividades, CarregaAtividades);
        }

        if (_listaDeAtividadesController.Current._audioDescricao.Descricao != null && _audioDescricaoView != null)
        {
            _audioDescricaoView.SetActive(true);
            PlayDescricaoEmAudio();
        }
        else
        {
            _audioDescricaoView.SetActive(false);
        }

        if (_btnProximaAtividade != null) _btnProximaAtividade.SetActive(false);
        if (_btnProximoExercicio != null) _btnProximoExercicio.SetActive(false);
    }

    private void PreprocessamentoExercicios(GameObject obj, int indexExercicio)
    {
        // Preprocessing
        var uiButtonController = obj.GetComponentInChildren<UIConainerElementController>();

        uiButtonController.Interactable = IsExercicioDisponivel(indexExercicio) || _exerciciosFinalizados;

        var txt = obj.GetComponentInChildren<TextMeshProUGUI>().text = $"Nivel {indexExercicio + 1}";

        if (indexExercicio == _listaDeExerciciosController.Index) uiButtonController.SetFocus();
    }

    private void PreprocessamentoAtividades(GameObject obj, int indexAtividade)
    {
        // Preprocessing
        var uiButtonController = obj.GetComponentInChildren<UIConainerElementController>();

        uiButtonController.Interactable = IsAtividadeDisponivel(indexAtividade) || _exerciciosFinalizados;

        var txt = obj.GetComponentInChildren<TextMeshProUGUI>().text = (indexAtividade + 1).ToString();

        if (indexAtividade == _listaDeAtividadesController.Index) uiButtonController.SetFocus();
    }

    private void OnAtividadeConcluida()
    {
        Debug.Log($"[ExerciciosManager][OnAtividadeConcluida] Atividade concluida");
        _historicoDeExerciciosCompletados[_listaDeExerciciosController.Index][_listaDeAtividadesController.Index] = true;

        if (_listaDeAtividadesController.IsLast)
        {
            ExercicioConcluido();
            Debug.Log($"[ExerciciosManager]\tProximo exercicio");
        }
        else
        {
            if (_passarAtividadesAutomaticamente)
            {
                ProximaAtividade();
            }


            if (_btnProximaAtividade != null) _btnProximaAtividade.SetActive(!_passarAtividadesAutomaticamente);
            if (_btnProximoExercicio != null) _btnProximoExercicio.SetActive(false);
        }

    }

    public void ProximaAtividade()
    {
        _listaDeAtividadesController.Next();
        Debug.Log($"[ExerciciosManager][ProximaAtividade]");

        CarregaAtividades(_listaDeAtividadesController.Index);
    }

    private void ExercicioConcluido()
    {
        Debug.Log($"[ExerciciosManager][ExercicioConcluido]");
        if (_listaDeExerciciosController.IsLast)
        {
            OnExerciciosFinalizados?.Invoke(this);
            _exerciciosFinalizados = true;

            _telaExerciciosConcluidos.SetActive(true);

            Debug.Log($"[ExerciciosManager]\tTodos os exercicios concluidos");
        }
        else
        {
            if (_passarAtividadesAutomaticamente)
            {
                Debug.Log($"[ExerciciosManager]\tProximo Exercicio");
                ProximoExercicio();
            }
            else
            {
                Debug.Log($"[ExerciciosManager]\tEspera usuario passar Exercicio");
            }


            if (_btnProximaAtividade != null) _btnProximaAtividade.SetActive(false);
            if (_btnProximoExercicio != null) _btnProximoExercicio.SetActive(!_passarAtividadesAutomaticamente);
        }
    }

    public void PlayDescricaoEmAudio()
    {
        if (_listaDeAtividadesController.Current._audioDescricao.Descricao != null)
        {
            AudioManager.Instance.PlayDescricao(_listaDeAtividadesController.Current._audioDescricao);
        }
    }

    public void ProximoExercicio()
    {
        _listaDeExerciciosController.Next();

        CarregaExercicio(_listaDeExerciciosController.Index);
    }

    public void VoltaExercicio()
    {
        _listaDeExerciciosController.Previous();

        CarregaExercicio(_listaDeExerciciosController.Index);
    }

    private bool IsAtividadeDisponivel(int indexAtividade)
    {
        if (indexAtividade == 0) return true;

        int indexExercicioAtual = _listaDeExerciciosController.Index;

        return _historicoDeExerciciosCompletados[indexExercicioAtual][indexAtividade - 1] || _historicoDeExerciciosCompletados[indexExercicioAtual][indexAtividade];
    }

    private bool IsExercicioDisponivel(int indexExercicio)
    {
        if (indexExercicio == 0) return true;

        var atividadeAnterior = _listaDeExerciciosController.Values[indexExercicio - 1]._atividades;

        return _historicoDeExerciciosCompletados[indexExercicio - 1][atividadeAnterior.Count - 1];
    }
}
