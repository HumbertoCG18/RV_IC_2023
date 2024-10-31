using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjetoElevadorController : MonoBehaviour
{
    [SerializeField] private TipoObjetoElevador _tipoObjetoElevador;
    [SerializeField] private List<TipoObjetoElevador> _restricoes;
    [SerializeField] private AreaDosObjetosController _areaController;

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
        Debug.Log($"Objeto solto {name} {_estaEmUmaArea}");
    }

    public void Temp(int i)
    {
        Debug.Log(i.ToString());
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

    public bool EstaEmUmaArea { get => _estaEmUmaArea; }
    public TipoObjetoElevador TipoObjeto => _tipoObjetoElevador;
}
