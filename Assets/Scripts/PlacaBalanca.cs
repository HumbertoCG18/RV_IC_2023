using System;
using UnityEngine;

public class PlacaBalanca : MonoBehaviour
{
    public enum PratoSelecionado { Esquerda, Direita }

    public PratoSelecionado prato;
    public float pesoPrato;
    public float pesoAtivarMin = 0f;
    public float pesoAtivarMax = 0f;
    //public GameObject objetoAtivacao;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peso"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (prato == PratoSelecionado.Esquerda && transform.CompareTag("PratoEsquerda"))
            {
                CalcularPeso(rb, true);
            }
            else if (prato == PratoSelecionado.Direita && transform.CompareTag("PratoDireita"))
            {
                CalcularPeso(rb, true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Peso"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (prato == PratoSelecionado.Esquerda && transform.CompareTag("PratoEsquerda"))
            {
                CalcularPeso(rb, false);
            }
            else if (prato == PratoSelecionado.Direita && transform.CompareTag("PratoDireita"))
            {
                CalcularPeso(rb, false);
            }
        }
    }

    void CalcularPeso(Rigidbody rb, bool adicionar)
    {
        if (rb == null) return;

        float massa = rb.mass;
        pesoPrato += adicionar ? massa : -massa;
        Debug.Log("Peso detectado no prato " + (prato == PratoSelecionado.Esquerda ? "Esquerda: " : "Direita: ") + massa);
        /*
         
        if (pesoPrato < pesoAtivarMin || pesoPrato > pesoAtivarMax)
        {
            objetoAtivacao.SetActive(true);
        }
        else
        {
            objetoAtivacao.SetActive(false);
        }
        */
    }

    internal float GetPesoPrato(PratoSelecionado prato)
    {
        throw new NotImplementedException();
    }
}
