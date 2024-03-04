using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectToSupport : MonoBehaviour
{
    // Referências para os suportes da direita e esquerda
    public Transform rightSupport;
    public Transform leftSupport;

    // Referências para os pontos de conexão da direita e esquerda
    public Transform rightConnectionPoint;
    public Transform leftConnectionPoint;

    private void Start()
    {
        // Verifica se as referências estão atribuídas corretamente
        if (rightSupport == null || leftSupport == null || rightConnectionPoint == null || leftConnectionPoint == null)
        {
            Debug.LogError("Certifique-se de atribuir todas as referências nos pontos de conexão e suportes.");
            return;
        }

        // Conecta o ponto de conexão da direita ao suporte da direita
        Connect(rightConnectionPoint, rightSupport);

        // Conecta o ponto de conexão da esquerda ao suporte da esquerda
        Connect(leftConnectionPoint, leftSupport);
    }

    private void Connect(Transform connectionPoint, Transform support)
    {
        // Calcula a posição e rotação corretas para conectar o ponto de conexão ao suporte
        Vector3 offset = support.position - connectionPoint.position;
        Quaternion rotationOffset = Quaternion.Inverse(connectionPoint.rotation) * support.rotation;

        // Conecta o ponto de conexão ao suporte
        connectionPoint.parent = support;
        connectionPoint.localPosition = offset;
        connectionPoint.localRotation = rotationOffset;
    }
}
