using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIConainerElementController : MonoBehaviour
{
    [SerializeField] private Image _buttonVisual;

    [SerializeField] private Color _onIdle;
    [SerializeField] private Color _onFocus;
    [SerializeField] private Color _onNotInteractable;

    private int _index;
    private bool _interactable = true;
    private Action<int> OnObjectSelected;

    public bool Interactable { get => _interactable; set => SetInteractable(value); }

    public void SetInteractable(bool novoValor)
    {
        _interactable = novoValor;

        _buttonVisual.color = _interactable ? _onIdle : _onNotInteractable;
    }

    public void SetFocus()
    {
        _buttonVisual.color = _onFocus;
    }

    public void SetTrigger(Action<int> callback, int index)
    {
        OnObjectSelected = callback;
        _index = index;
    }

    public void OnSelected()
    {
        if (_interactable)
        {
            OnObjectSelected?.Invoke(_index);
        }
    }
}
