using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaBalanca : MonoBehaviour
{
    public float peso { get; private set; } = 0f;
    public float pesoAtivarMin = 0f;
    public float pesoAtivarMax = 0f;
    public GameObject ativar;

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            AdicionarPeso(otherRigidbody.mass);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody otherRigidbody = other.GetComponent<Rigidbody>();
        if (otherRigidbody != null)
        {
            RemoverPeso(otherRigidbody.mass);
        }
    }

    private void AdicionarPeso(float massa)
    {
        peso += massa;
        VerificarAtivacao();
    }

    private void RemoverPeso(float massa)
    {
        peso -= massa;
        VerificarAtivacao();
    }

    private void VerificarAtivacao()
    {
        if (peso < pesoAtivarMin || peso > pesoAtivarMax)
        {
            ativar.SetActive(true);
        }
    }
}
