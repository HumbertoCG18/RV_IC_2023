using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AreaDosObjetosController : MonoBehaviour
{
    [SerializeField] private List<Transform> _locaisDosObjetos;
    [SerializeField] private GameObject _uiRestricaoCachorroGato;
    [SerializeField] private GameObject _uiRestricaoGatoRato;
    [SerializeField] private int _quantidadeObjetosPermitidos;
    [SerializeField] private bool _usaPosicoesPreDefinidas = true;

    [SerializeField] private List<ObjetoElevadorController> _objetosNaArea = new List<ObjetoElevadorController>();


    public Action OnMudancaDeEstado;

    public void CustomOnTriggerEnter(Collider objeto)
    {
        var objetoElevador = objeto.GetComponentInChildren<ObjetoElevadorController>();

        if (objetoElevador == null) return;

        AdicionaObjeto(objetoElevador);
    }

    private void AtualizaPosicoes()
    {
        if (!_usaPosicoesPreDefinidas) return;

        foreach(var objeto in _objetosNaArea)
        {
            switch (objeto.TipoObjeto)
            {
                case ObjetoElevadorController.TipoObjetoElevador.Cachorro: PosicionaOBjeto(objeto.transform, _locaisDosObjetos[0]); break;
                case ObjetoElevadorController.TipoObjetoElevador.Gato: PosicionaOBjeto(objeto.transform, _locaisDosObjetos[1]); break;
                case ObjetoElevadorController.TipoObjetoElevador.Rato: PosicionaOBjeto(objeto.transform, _locaisDosObjetos[2]); break;
            }
        }
    }

    private void AtualizaUI()
    {
        _uiRestricaoCachorroGato.SetActive(false);
        _uiRestricaoGatoRato.SetActive(false);

        foreach (var obj in _objetosNaArea)
        {
            if (obj.ContemRestricao(_objetosNaArea).Count != 0)
            {
                if (obj.TipoObjeto == ObjetoElevadorController.TipoObjetoElevador.Cachorro) _uiRestricaoCachorroGato.SetActive(true);
                if (obj.TipoObjeto == ObjetoElevadorController.TipoObjetoElevador.Gato) _uiRestricaoGatoRato.SetActive(true);
            }
        }
    }

    public void CustomOnTriggerExit(Collider objeto)
    {
        var objetoElevador = objeto.GetComponentInChildren<ObjetoElevadorController>();

        if (objetoElevador == null) return;

        RemoveObjeto(objetoElevador);
        AtualizaUI();
    }

    public void AdicionaObjeto(ObjetoElevadorController objetoElevadorController)
    {
        if (!_objetosNaArea.Contains(objetoElevadorController))
            _objetosNaArea.Add(objetoElevadorController);

        objetoElevadorController.AdicionaEmArea(this);

        AtualizaPosicoes();
        AtualizaUI();
        objetoElevadorController.SoltaObjeto();

        OnMudancaDeEstado?.Invoke();
    }

    public void RemoveObjeto(ObjetoElevadorController objetoElevadorController)
    {
        _objetosNaArea.Remove(objetoElevadorController);
        objetoElevadorController.RemoveDaArea();

        OnMudancaDeEstado?.Invoke();
    }

    private void PosicionaOBjeto(Transform objeto, Transform posicao)
    {
        objeto.position = posicao.position;
        objeto.rotation = posicao.rotation;
    }

    public bool AreaValida => _objetosNaArea.All(o => o.ContemRestricao(_objetosNaArea).Count == 0) && _objetosNaArea.Count <= _quantidadeObjetosPermitidos;

    public int QuantidadeDeObjetos => _objetosNaArea.Count;
}
