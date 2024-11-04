using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjetoDivisivelController : MonoBehaviour, IFracao
{
    [SerializeField] private ExibidorDeFracoesController _exibidorDeFracoesController;
    [SerializeField] private Transform _modelo3D;

    private static float ESPACO_ENTRE_DIVISOES = 0.05f;

    private int _numerador = 1;
    private int _denominador = 1;

    private void Start()
    {
        _exibidorDeFracoesController.AdicionaFracao(this);
    }

    public void DefineRazao(int numerador, int denominador)
    {
        _numerador = numerador;
        _denominador = Mathf.Max(0, denominador);

        Vector3 novaEscala = new Vector3(1, 1, 1 / (float)_denominador);
        _modelo3D.localScale = novaEscala;
    }

    public void DivideObjeto(int quantidadePartes)
    {
        MeshRenderer renderer = _modelo3D.GetComponentInChildren<MeshRenderer>();
        Bounds bounds = renderer.bounds;

        float tamanhoFilho = bounds.size.z / (float)quantidadePartes;
        float larguraTotal = bounds.size.z + ESPACO_ENTRE_DIVISOES * (quantidadePartes - 1);
        Vector3 posicaoInicial = transform.localPosition - transform.forward * (larguraTotal * 0.5f) + transform.forward * tamanhoFilho * 0.5f;

        Vector3 passo = transform.forward * ESPACO_ENTRE_DIVISOES + transform.forward * tamanhoFilho;

        //Debug.Log($"{quantidadePartes} {larguraTotal} {passo} {posicaoInicial}  {transform.forward} {transform.localPosition}");

        for (int i = 0; i < quantidadePartes; i++)
        {
            GameObject instancia = Instantiate(gameObject, transform.parent);

            instancia.transform.localPosition = posicaoInicial + passo * i;
            instancia.GetComponent<ObjetoDivisivelController>().DefineRazao(1, _denominador * quantidadePartes);
        }

        Destroy(gameObject);
    }

    public int Numerador() => _numerador;

    public int Denominador() => _denominador;

    public GameObject Instancia() => gameObject;
}
