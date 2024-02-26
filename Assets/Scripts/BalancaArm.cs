using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceArm : MonoBehaviour
{
    // Referências para os pratos esquerdo e direito
    public Transform leftPlate;
    public Transform rightPlate;

    // Atributos para ajustar a velocidade e suavidade do movimento
    public float rotationSpeed = 5f;
    public float rotationSmoothing = 5f;

    private Quaternion initialRotation; // Rotação inicial da aste da balança

    private void Start()
    {
        // Salva a rotação inicial da aste da balança
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Calcula a diferença de altura entre os pratos
        float heightDifference = leftPlate.position.y - rightPlate.position.y;

        // Calcula a rotação baseada na diferença de altura
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, heightDifference * rotationSpeed) * initialRotation;

        // Suaviza a rotação
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
    }
}

