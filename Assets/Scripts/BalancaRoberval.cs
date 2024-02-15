using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[DebuggerDisplay("{" + nameof(GetDebuggerDisplay) + "(),nq}")]
public class BalancaRoberval : MonoBehaviour
{
    public GameObject haste;
    public GameObject pontoConexaoEsquerda;
    public GameObject pontoConexaoDireita;
    public GameObject suportePratoEsquerda;
    public GameObject suportePratoDireita;
    public float forcaTorque = 10f;

    private ConfigurableJoint jointEsquerda;
    private ConfigurableJoint jointDireita;

    void Start()
    {
        // Configurar junta para o ponto de conexão virtual da extremidade esquerda
        jointEsquerda = pontoConexaoEsquerda.AddComponent<ConfigurableJoint>();
        ConfigurarJunta(jointEsquerda, suportePratoEsquerda);

        // Configurar junta para o ponto de conexão virtual da extremidade direita
        jointDireita = pontoConexaoDireita.AddComponent<ConfigurableJoint>();
        ConfigurarJunta(jointDireita, suportePratoDireita);
    }

    void ConfigurarJunta(ConfigurableJoint joint, GameObject suporte)
    {
        joint.connectedBody = suporte.GetComponent<Rigidbody>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularYMotion = ConfigurableJointMotion.Limited;
        joint.angularZMotion = ConfigurableJointMotion.Locked;
    }

    void Update()
    {
        // Calcula a diferença de peso entre os pratos
        float diferencaPeso = Mathf.Abs(suportePratoEsquerda.GetComponent<Rigidbody>().mass - suportePratoDireita.GetComponent<Rigidbody>().mass);

        // Aplica torque na haste para simular o movimento da balança
        Vector3 torque = diferencaPeso * forcaTorque * Vector3.up;
        haste.GetComponent<Rigidbody>().AddTorque(torque);
    }

    private string GetDebuggerDisplay()
    {
        return ToString();
    }
}
