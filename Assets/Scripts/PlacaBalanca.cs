using UnityEngine;

public class PlacaBalanca : MonoBehaviour
{
    public enum PratoSelecionado { Esquerda, Direita }

    public PratoSelecionado prato;
    public float pesoPrato;
    public float pesoAtivarMin = 0f;
    public float pesoAtivarMax = 0f;
    public GameObject objetoAtivacao;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Peso"))
        {
            Rigidbody rb = other.attachedRigidbody;
            Transform parent = other.transform.parent;
            while (parent != null)
            {
                if (prato == PratoSelecionado.Esquerda && parent.CompareTag("PratoEsquerda"))
                {
                    CalcularPeso(rb, true);
                    break;
                }
                else if (prato == PratoSelecionado.Direita && parent.CompareTag("PratoDireita"))
                {
                    CalcularPeso(rb, true);
                    break;
                }
                parent = parent.parent;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Peso"))
        {
            Rigidbody rb = other.attachedRigidbody;
            Transform parent = other.transform.parent;
            while (parent != null)
            {
                if (prato == PratoSelecionado.Esquerda && parent.CompareTag("PratoEsquerda"))
                {
                    CalcularPeso(rb, false);
                    break;
                }
                else if (prato == PratoSelecionado.Direita && parent.CompareTag("PratoDireita"))
                {
                    CalcularPeso(rb, false);
                    break;
                }
                parent = parent.parent;
            }
        }
    }

    void CalcularPeso(Rigidbody rb, bool adicionar)
    {
        if (rb == null) return;

        float massa = rb.mass;
        pesoPrato += adicionar ? massa : -massa;

        Debug.Log("Peso detectado no prato " + prato + ": " + pesoPrato);

        if (pesoPrato < pesoAtivarMin || pesoPrato > pesoAtivarMax)
        {
            objetoAtivacao.SetActive(true);
        }
        else
        {
            objetoAtivacao.SetActive(false);
        }
    }
}
