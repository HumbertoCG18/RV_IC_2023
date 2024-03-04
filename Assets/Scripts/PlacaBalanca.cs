using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaBalanca : MonoBehaviour
{
    public float Peso { get; private set; } = 0f;
    public float PesoAtivarMin = 0f;
    public float PesoAtivarMax = 0f;
    public GameObject Ativar;

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
        Peso += massa;
        VerificarAtivacao();
    }

    private void RemoverPeso(float massa)
    {
        Peso -= massa;
        VerificarAtivacao();
    }

    private void VerificarAtivacao()
    {
        if (Peso < PesoAtivarMin || Peso > PesoAtivarMax)
        {
            Ativar.SetActive(true);
        }
    }
}
