using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarPratoAoSuporte : MonoBehaviour
{
    [HideInInspector]
    public GameObject suporteConectado; // Refer�ncia para o suporte conectado

    void Start()
    {
        // Encontra o prato associado ao suporte
        GameObject prato = transform.parent.gameObject; // O prato � o pai do suporte

        // Conecta o suporte ao prato
        transform.SetParent(prato.transform); // Torna o prato o pai do suporte
        transform.localPosition = Vector3.zero; // Reposiciona o suporte para a origem local do prato

        // Conecta o suporte ao ponto de conex�o na haste
        ConnectToJoint();

        // Armazena uma refer�ncia para o suporte conectado
        suporteConectado = gameObject;
    }

    private void ConnectToJoint()
    {
        // Obt�m o Rigidbody do prato
        Rigidbody pratoRigidbody = transform.parent.GetComponent<Rigidbody>();
        if (pratoRigidbody == null)
        {
            Debug.LogError("Erro: Rigidbody n�o encontrado no prato.");
            return;
        }

        // Obt�m o ponto de conex�o na haste (PontoEsquerda ou PontoDireita)
        string nomePontoConexao = transform.name.Equals("Suporte prato esquerda") ? "PontoEsquerda" : "PontoDireita";
        GameObject pontoConexao = transform.parent.Find(nomePontoConexao).gameObject;
        if (pontoConexao == null)
        {
            Debug.LogError("Erro: Ponto de conex�o n�o encontrado.");
            return;
        }

        // Obt�m o Rigidbody do ponto de conex�o na haste
        Rigidbody pontoRigidbody = pontoConexao.GetComponent<Rigidbody>();
        if (pontoRigidbody == null)
        {
            Debug.LogError("Erro: Rigidbody n�o encontrado no ponto de conex�o.");
            return;
        }

        // Configura a conex�o do suporte ao ponto de conex�o na haste
        ConfigurarJoint(pratoRigidbody, pontoRigidbody);
    }

    private void ConfigurarJoint(Rigidbody pratoRigidbody, Rigidbody pontoRigidbody)
    {
        // Calcula a posi��o relativa do ponto de conex�o na haste em rela��o ao prato
        Vector3 conexaoLocalPosition = pratoRigidbody.transform.InverseTransformPoint(pontoRigidbody.position);

        // Configura uma junta fixa entre o suporte e o ponto de conex�o na haste
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = pontoRigidbody;
        joint.anchor = conexaoLocalPosition;
    }
}



