using UnityEngine;

public class PlacaBalanca : MonoBehaviour
{
    public enum PratoSelecionado { Esquerda, Direita }

    public PratoSelecionado prato;
    public float pesoPrato;
    public float pesoAtivarMin = 0f;
    public float pesoAtivarMax = 0f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peso"))
        {
            Rigidbody rb = other.attachedRigidbody;
            if (prato == PratoSelecionado.Esquerda && transform.CompareTag("PratoEsquerda"))
            {
                CalcularPeso(rb.mass, true);
            }
            else if (prato == PratoSelecionado.Direita && transform.CompareTag("PratoDireita"))
            {
                CalcularPeso(rb.mass, true);
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
                CalcularPeso(rb.mass, false);
            }
            else if (prato == PratoSelecionado.Direita && transform.CompareTag("PratoDireita"))
            {
                CalcularPeso(rb.mass, false);
            }
        }
    }

    void CalcularPeso(float massa, bool adicionar)
    {
        pesoPrato += adicionar ? massa : -massa;
        Debug.Log("Peso total detectado no prato da" + (prato == PratoSelecionado.Esquerda ? "Esquerda: " : "Direita: ") + pesoPrato);
    }

    public float GetPesoPrato(PratoSelecionado prato)
    {
        if (this.prato == prato)
        {
            return pesoPrato;
        }
        else
        {
            return 0f;
        }
    }
}
