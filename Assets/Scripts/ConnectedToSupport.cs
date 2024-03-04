using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToSupport : MonoBehaviour
{
    // Refer�ncias para os suportes da direita e esquerda
    public Transform rightSupport;
    public Transform leftSupport;

    // Refer�ncias para os pontos de conex�o da direita e esquerda
    public Transform rightConnectionPoint;
    public Transform leftConnectionPoint;

    private void Start()
    {
        // Verifica se as refer�ncias est�o atribu�das corretamente
        if (rightSupport == null || leftSupport == null || rightConnectionPoint == null || leftConnectionPoint == null)
        {
            Debug.LogError("Certifique-se de atribuir todas as refer�ncias nos pontos de conex�o e suportes.");
            return;
        }

        // Conecta o ponto de conex�o da direita ao suporte da direita
        Connect(rightConnectionPoint, rightSupport);

        // Conecta o ponto de conex�o da esquerda ao suporte da esquerda
        Connect(leftConnectionPoint, leftSupport);
    }

    private void Connect(Transform connectionPoint, Transform support)
    {
        // Calcula a posi��o e rota��o corretas para conectar o ponto de conex�o ao suporte
        Vector3 offset = support.position - connectionPoint.position;
        Quaternion rotationOffset = Quaternion.Inverse(connectionPoint.rotation) * support.rotation;

        // Conecta o ponto de conex�o ao suporte
        connectionPoint.parent = support;
        connectionPoint.localPosition = offset;
        connectionPoint.localRotation = rotationOffset;
    }
}
