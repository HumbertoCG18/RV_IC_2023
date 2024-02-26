using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceArm : MonoBehaviour
{
    // Refer�ncias para os pratos esquerdo e direito
    public Transform leftPlate;
    public Transform rightPlate;

    // Atributos para ajustar a velocidade e suavidade do movimento
    public float rotationSpeed = 5f;
    public float rotationSmoothing = 5f;

    private Quaternion initialRotation; // Rota��o inicial da aste da balan�a

    private void Start()
    {
        // Salva a rota��o inicial da aste da balan�a
        initialRotation = transform.rotation;
    }

    private void Update()
    {
        // Calcula a diferen�a de altura entre os pratos
        float heightDifference = leftPlate.position.y - rightPlate.position.y;

        // Calcula a rota��o baseada na diferen�a de altura
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, heightDifference * rotationSpeed) * initialRotation;

        // Suaviza a rota��o
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSmoothing);
    }
}

