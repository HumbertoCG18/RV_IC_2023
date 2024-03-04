using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarPratoAoSuporte : MonoBehaviour
{
    public GameObject suportePrato;

    void Start()
    {
        if (suportePrato == null)
        {
            Debug.LogError("O suporte do prato não está definido!");
            return;
        }

        ConfigurarJoint(gameObject, suportePrato);
    }

    void ConfigurarJoint(GameObject conectado, GameObject conectadoA)
    {
        // Adiciona um ConfigurableJoint ao objeto do prato
        ConfigurableJoint joint = conectado.AddComponent<ConfigurableJoint>();

        // Conecta o prato ao suporte
        joint.connectedBody = conectadoA.GetComponent<Rigidbody>();

        // Configurações da junta
        joint.anchor = Vector3.zero;
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector3.zero;
        joint.xMotion = ConfigurableJointMotion.Locked;
        joint.yMotion = ConfigurableJointMotion.Locked;
        joint.zMotion = ConfigurableJointMotion.Locked;
        joint.angularXMotion = ConfigurableJointMotion.Free;
        joint.angularYMotion = ConfigurableJointMotion.Free;
        joint.angularZMotion = ConfigurableJointMotion.Free;
    }
}
