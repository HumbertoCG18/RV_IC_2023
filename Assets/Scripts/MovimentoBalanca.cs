using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoBalanca : MonoBehaviour
{
    public GameObject pratoEsquerdo;
    public GameObject pratoDireito;
    public GameObject haste;
    public float forcaTorque = 10f;

    Rigidbody rbPratoEsquerdo;
    Rigidbody rbPratoDireito;
    Rigidbody rbHaste;

    void Start()
    {
        rbPratoEsquerdo = pratoEsquerdo.GetComponent<Rigidbody>();
        rbPratoDireito = pratoDireito.GetComponent<Rigidbody>();
        rbHaste = haste.GetComponent<Rigidbody>();
    }

    void Update()
    {
        float diferencaPeso = Mathf.Abs(rbPratoEsquerdo.mass - rbPratoDireito.mass);
        Vector3 torque = diferencaPeso * forcaTorque * Vector3.up;
        rbHaste.AddTorque(torque);
    }
}
