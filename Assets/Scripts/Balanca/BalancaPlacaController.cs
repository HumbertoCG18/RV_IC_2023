using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalancaPlacaController : MonoBehaviour
{
    [SerializeField] private BalancaController _balancaController;
    [SerializeField] private bool _isPlacaEsquerda = false;

    private List<BalancaPesoController> _pesosNaPlaca;

    private void Awake()
    {
        _pesosNaPlaca = new List<BalancaPesoController>();
    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        BalancaPesoController pesoController = collision.gameObject.GetComponent<BalancaPesoController>();

        if (pesoController == null) return;

        _pesosNaPlaca.Add(pesoController);

        _balancaController.AtualizaPesos();
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        BalancaPesoController pesoController = other.gameObject.GetComponent<BalancaPesoController>();

        if (pesoController == null) return;

        _pesosNaPlaca.Add(pesoController);

        _balancaController.AtualizaPesos();
    }

    private void OnTriggerExit(Collider other)
    {
        BalancaPesoController pesoController = other.gameObject.GetComponent<BalancaPesoController>();

        if (pesoController == null) return;

        _pesosNaPlaca.Remove(pesoController);

        _balancaController.AtualizaPesos();
    }

    public void ResetarPesos()
    {
        _pesosNaPlaca.Clear();
        _balancaController.AtualizaPesos();
    }

    /*
    private void OnCollisionExit(Collision collision)
    {
        BalancaPesoController pesoController = collision.gameObject.GetComponent<BalancaPesoController>();

        if (pesoController == null) return;

        _pesosNaPlaca.Remove(pesoController);

        _balancaController.TrataPesoNaPlaca(this);
    }
    */

    public List<BalancaPesoController> Pesos { get => _pesosNaPlaca; }
    public int TotalPeso => _pesosNaPlaca.Sum(p => p.Peso);
    public bool IsPlacaEsquerda { get => _isPlacaEsquerda; }
    public bool ContemPesoOculto => _pesosNaPlaca.Any(p => p.IsOculto);
}
