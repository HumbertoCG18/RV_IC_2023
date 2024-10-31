using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PitagorasController : MonoBehaviour
{
    [SerializeField] private List<PitagorasNivelController> _niveis;
    [SerializeField] private Transform _nivelParent;

    private PitagorasNivelController _nivelAtual;
    private int _indexNivelAtual = 0;

    public Action OnNivelConcluido;

    private void Update()
    {
        
        if (Keyboard.current[Key.C].wasPressedThisFrame)
        {
            OnNivelConcluido();
        }
        
    }

    public void CarregaNivel(PitagorasNivelController nivel)
    {
        StartCoroutine(InstanciaNivel(nivel));
    }

    public IEnumerator InstanciaNivel(PitagorasNivelController nivel)
    {
        if (_nivelAtual != null)
        {
            _nivelAtual.DesativaEncaixes();

            yield return null;

            Destroy(_nivelAtual.gameObject);
        }

        _nivelAtual = Instantiate(nivel, _nivelParent);

        _nivelAtual.SetInfo(this);
    }

    public void NivelConcluido()
    {
        OnNivelConcluido?.Invoke();
    }

    public void ProximoNivel()
    {
        _indexNivelAtual = Mathf.Min(_indexNivelAtual + 1, _niveis.Count - 1);

        StartCoroutine(InstanciaNivel(_niveis[_indexNivelAtual]));
    }

    public void VoltarNivel()
    {
        _indexNivelAtual = Mathf.Max(0, _indexNivelAtual - 1);

        CarregaNivel(_niveis[_indexNivelAtual]);
    }

    public void IniciaNivel1()
    {
        _indexNivelAtual = -1;
        ProximoNivel();
    }

    public void IniciaNivel2()
    {
        _indexNivelAtual = 0;
        ProximoNivel();
    }

    public void IniciaNivel3()
    {
        _indexNivelAtual = 1;
        ProximoNivel();
    }

    public void IniciaNivel4()
    {
        _indexNivelAtual = 2;
        ProximoNivel();
    }
}
