using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ObjetoElevadorController : MonoBehaviour
{
    [SerializeField] private TipoObjetoElevador _tipoObjetoElevador;
    [SerializeField] private List<TipoObjetoElevador> _restricoes;
    [SerializeField] private AreaDosObjetosController _areaController;
    [SerializeField] private Rigidbody _rigibody;

    public enum TipoObjetoElevador { Cachorro, Gato, Rato }

    private bool _estaEmUmaArea = false;


    public List<TipoObjetoElevador> ContemRestricao(List<TipoObjetoElevador> objetos)
    {
        var restricoesEncontradas = new List<TipoObjetoElevador>();

        foreach (var objeto in objetos)
        {
            if (_restricoes.Contains(objeto))
            {
                restricoesEncontradas.Add(objeto);
            }
        }

        return restricoesEncontradas;
    }

    public List<TipoObjetoElevador> ContemRestricao(List<ObjetoElevadorController> objetos)
    {
        var tipos = objetos.Select(o => o.TipoObjeto).ToList();

        return ContemRestricao(tipos);
    }

    public void OnObjetoSolto()
    {
        if (!_estaEmUmaArea)
        {
            _areaController.AdicionaObjeto(this);
        }
    }

    public void AdicionaEmArea(AreaDosObjetosController areaController)
    {
        _areaController = areaController;
        _estaEmUmaArea = true;
    }

    public void RemoveDaArea()
    {
        _estaEmUmaArea = false;
    }

    public void SoltaObjeto()
    {
        var xrGrabInteraction = GetComponentInChildren<XRGrabInteractable>();

        if (xrGrabInteraction == null) return;

        InteractionManager.Instance.DropObject(xrGrabInteraction, true);

        _rigibody.velocity = Vector3.zero;
    }

    public void DesabilitaColisoes()
    {
        _rigibody.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void AbilitaColisoes()
    {
        _rigibody.constraints = RigidbodyConstraints.FreezeRotation;
    }


    public bool EstaEmUmaArea { get => _estaEmUmaArea; }
    public TipoObjetoElevador TipoObjeto => _tipoObjetoElevador;
}
