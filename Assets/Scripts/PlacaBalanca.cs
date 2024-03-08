using UnityEngine;

public class PlacaBalanca : MonoBehaviour
{
    public float pesoPratoEsquerda;
    public float pesoPratoDireita;
    public float pesoAtivarMin = 0f;
    public float pesoAtivarMax = 0f;
    public GameObject ativar;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PratoEsquerda"))
        {
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody != null)
            {
                pesoPratoEsquerda += otherRigidbody.mass;
                VerificarAtivacao();
            }
        }
        else if (other.CompareTag("PratoDireita"))
        {
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody != null)
            {
                pesoPratoDireita += otherRigidbody.mass;
                VerificarAtivacao();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PratoEsquerda"))
        {
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody != null)
            {
                pesoPratoEsquerda -= otherRigidbody.mass;
                VerificarAtivacao();
            }
        }
        else if (other.CompareTag("PratoDireita"))
        {
            Rigidbody otherRigidbody = other.attachedRigidbody;
            if (otherRigidbody != null)
            {
                pesoPratoDireita -= otherRigidbody.mass;
                VerificarAtivacao();
            }
        }
    }

    private void VerificarAtivacao()
    {
        if (pesoPratoEsquerda < pesoAtivarMin || pesoPratoEsquerda > pesoAtivarMax ||
            pesoPratoDireita < pesoAtivarMin || pesoPratoDireita > pesoAtivarMax)
        {
            ativar.SetActive(true);
        }
        else
        {
            ativar.SetActive(false); // Desativar se estiver dentro do intervalo
        }
    }
}
