using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentoHaste : MonoBehaviour
{
    public float anguloMaximo = 90f;
    public float anguloMinimo = -90f;

    void Update()
    {
        float input = Input.GetAxis("Vertical");
        float angulo = Mathf.Clamp(input, anguloMinimo, anguloMaximo);

        transform.localRotation = Quaternion.Euler(angulo, 0f, 0f);
    }
}

