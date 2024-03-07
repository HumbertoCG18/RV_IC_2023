using UnityEngine;

public class PlacaBalanca : MonoBehaviour
{
    public float PesoPrato { get { return pesoPrato; } }
    public float pesoPrato;
    public float pesoAtivarMin = 0f;
    public float pesoAtivarMax = 0f;
    public GameObject ativar;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.attachedRigidbody;
        if (otherRigidbody != null)
        {
            if (other.transform.Find("pratoEsquerda"))
            {
                AdicionarPesoPrato(otherRigidbody.mass);
            }
            else if (other.transform.Find("pratoDireito"))
            {
                AdicionarPesoPrato(otherRigidbody.mass);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody otherRigidbody = other.attachedRigidbody;
        if (otherRigidbody != null)
        {
            if (other.transform.Find("pratoEsquerda"))
            {
                RemoverPesoPrato(otherRigidbody.mass);
            }
            else if (other.transform.Find("pratoDireito"))
            {
                RemoverPesoPrato(otherRigidbody.mass);
            }
        }
    }

    private void AdicionarPesoPrato(float massa)
    {
        pesoPrato += massa;
        VerificarAtivacao();
    }

    private void RemoverPesoPrato(float massa)
    {
        pesoPrato -= massa;
        VerificarAtivacao();
    }

    private void VerificarAtivacao()
    {
        if (pesoPrato < pesoAtivarMin || pesoPrato > pesoAtivarMax)
        {
            ativar.SetActive(true);
        }
        else
        {
            ativar.SetActive(false); // Desativar se estiver dentro do intervalo
        }
    }
}
