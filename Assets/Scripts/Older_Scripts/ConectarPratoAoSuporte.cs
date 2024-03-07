using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarPratoAoSuporte : MonoBehaviour
{
    [HideInInspector]
    public GameObject suporteConectado; // Referência para o suporte conectado

    void Start()
    {
        // Encontra o prato associado ao suporte
        GameObject prato = transform.parent.gameObject; // O prato é o pai do suporte

        // Conecta o suporte ao prato
        transform.SetParent(prato.transform); // Torna o prato o pai do suporte
        transform.localPosition = Vector3.zero; // Reposiciona o suporte para a origem local do prato

        // Conecta o suporte ao ponto de conexão na haste
        ConnectToJoint();

        // Armazena uma referência para o suporte conectado
        suporteConectado = gameObject;
    }

    private void ConnectToJoint()
    {
        // Obtém o Rigidbody do prato
        Rigidbody pratoRigidbody = transform.parent.GetComponent<Rigidbody>();
        if (pratoRigidbody == null)
        {
            Debug.LogError("Erro: Rigidbody não encontrado no prato.");
            return;
        }

        // Obtém o ponto de conexão na haste (PontoEsquerda ou PontoDireita)
        string nomePontoConexao = transform.name.Equals("Suporte prato esquerda") ? "PontoEsquerda" : "PontoDireita";
        GameObject pontoConexao = transform.parent.Find(nomePontoConexao).gameObject;
        if (pontoConexao == null)
        {
            Debug.LogError("Erro: Ponto de conexão não encontrado.");
            return;
        }

        // Obtém o Rigidbody do ponto de conexão na haste
        Rigidbody pontoRigidbody = pontoConexao.GetComponent<Rigidbody>();
        if (pontoRigidbody == null)
        {
            Debug.LogError("Erro: Rigidbody não encontrado no ponto de conexão.");
            return;
        }

        // Configura a conexão do suporte ao ponto de conexão na haste
        ConfigurarJoint(pratoRigidbody, pontoRigidbody);
    }

    private void ConfigurarJoint(Rigidbody pratoRigidbody, Rigidbody pontoRigidbody)
    {
        // Calcula a posição relativa do ponto de conexão na haste em relação ao prato
        Vector3 conexaoLocalPosition = pratoRigidbody.transform.InverseTransformPoint(pontoRigidbody.position);

        // Configura uma junta fixa entre o suporte e o ponto de conexão na haste
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = pontoRigidbody;
        joint.anchor = conexaoLocalPosition;
    }
}



