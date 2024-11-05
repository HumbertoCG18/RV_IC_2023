using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PortaController : MonoBehaviour
{
    [SerializeField] private List<Transform> _portaPivots;
    [SerializeField] private float _velocidadeAnimacao = 0.6f;
    [SerializeField] private bool _alteraEixoX = false;
    [SerializeField] private bool _alteraEixoY = false;
    [SerializeField] private bool _alteraEixoZ = true;

    [SerializeField] private bool _estaAberta = false;

    public UnityEvent OnPortaSelecionada;

    private void Update()
    {
        if (Keyboard.current[Key.P].wasPressedThisFrame)
        {
            TogglePorta();
        }
    }

    public void AbrirPorta(Action callback=null)
    {
        StartCoroutine(AnimaPorta(_velocidadeAnimacao, GetEscalaAlvo(true), callback));
        _estaAberta = true;
    }

    public void FechaPorta(Action callback = null)
    {
        StartCoroutine(AnimaPorta(_velocidadeAnimacao, GetEscalaAlvo(false), callback));
        _estaAberta = false;
    }

    public void TogglePorta()
    {
        if (_estaAberta)
        {
            FechaPorta();
        }
        else
        {
            AbrirPorta();
        }
    }

    private Vector3 GetEscalaAlvo(bool paraAbrir)
    {
        Vector3 escalaDesejada = Vector3.one;

        if (paraAbrir)
        {
            escalaDesejada.x = _alteraEixoX ? 0 : 1;
            escalaDesejada.y = _alteraEixoY ? 0 : 1;
            escalaDesejada.z = _alteraEixoZ ? 0 : 1;

            return escalaDesejada;
        }
        else
        {
            return escalaDesejada;
        }
    }

    private IEnumerator AnimaPorta(float tempoDeAnimacao, Vector3 escalaAlvo, Action callback)
    {
        OnPortaSelecionada?.Invoke();

        float t = 0f;

        List<Vector3> _escalasOriginais = new List<Vector3>();
        _portaPivots.ForEach(p => _escalasOriginais.Add(p.localScale));

        while (t < 1f)
        {
            int i = 0;
            foreach (Transform porta in _portaPivots)
            {
                porta.localScale = Vector3.Lerp(_escalasOriginais[i], escalaAlvo, t);
            }

            yield return null;

            t += Time.deltaTime / tempoDeAnimacao;
        
        }

        foreach (Transform porta in _portaPivots)
        {
            porta.localScale = escalaAlvo;
        }

        callback?.Invoke();
    }
}
