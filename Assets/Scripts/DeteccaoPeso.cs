using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetecaoPeso : MonoBehaviour
{
    public float peso { get; private set; } = 0f;
    public float pesoAtivarMin = 0f;
    public float pesoAtivarMax = 0f;
    public GameObject ativar;

    void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            AdicionarPeso(otherRigidbody.mass);
        }
    }

    void OnTriggerExit(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            RemoverPeso(otherRigidbody.mass);
        }
    }

    void AdicionarPeso(float massa)
    {
        peso += massa;
        VerificarAtivacao();
    }

    void RemoverPeso(float massa)
    {
        peso -= massa;
        VerificarAtivacao();
    }

    void VerificarAtivacao()
    {
        if (peso < pesoAtivarMin || peso > pesoAtivarMax)
        {
            ativar.SetActive(true);
        }
    }
}

