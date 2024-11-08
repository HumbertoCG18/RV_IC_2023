using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfiguracoesUIController : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private bool _comecarVisivel = false;

    private void Awake()
    {
        _view.SetActive(_comecarVisivel);
    }

    public void SetView(bool value)
    {
        _view.SetActive(value);
    }
}
