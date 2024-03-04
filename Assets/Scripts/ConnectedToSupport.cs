using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarSuporteBase : MonoBehaviour
{
    public GameObject suporteBase;
    public GameObject meioAste;

    void Start()
    {
        if (suporteBase == null || meioAste == null)
        {
            Debug.LogError("Suporte da base ou meio da aste não definidos!");
            return;
        }

        // Adiciona o componente FixedJoint ao suporte da base
        FixedJoint fixedJoint = suporteBase.AddComponent<FixedJoint>();

        // Conecta o suporte da base ao meio da aste
        fixedJoint.connectedBody = meioAste.GetComponent<Rigidbody>();

        // Configura o FixedJoint
        fixedJoint.anchor = Vector3.zero;
        fixedJoint.autoConfigureConnectedAnchor = false;
        fixedJoint.connectedAnchor = Vector3.zero;
        fixedJoint.breakForce = Mathf.Infinity; // Evita que a junta se quebre devido a forças externas
        fixedJoint.breakTorque = Mathf.Infinity; // Evita que a junta se quebre devido a torque externo
    }
}
