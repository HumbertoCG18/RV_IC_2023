using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuncaoBalanca : MonoBehaviour
{
    public GameObject pratoEsquerdo;
    public GameObject pratoDireito;
    public GameObject haste;

    void Update()
    {
        float diferencaPeso = Mathf.Abs(pratoEsquerdo.GetComponent<Rigidbody>().mass - pratoDireito.GetComponent<Rigidbody>().mass);
        Vector3 direcaoMovimento = diferencaPeso > 0 ? Vector3.up : Vector3.zero;
        haste.transform.localPosition += direcaoMovimento * Time.deltaTime;
    }
}

