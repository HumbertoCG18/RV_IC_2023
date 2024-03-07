using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarJoints : MonoBehaviour
{
    public GameObject extremidadeHaste;
    public GameObject suportePrato;

    public bool conectarAsteAoSuporte = true;

    void Start()
    {
        if (extremidadeHaste == null || suportePrato == null)
        {
            Debug.LogError("Extremidade da haste ou suporte do prato não está definido!");
            return;
        }

        if (conectarAsteAoSuporte)
        {
            ConfigurarJoint(extremidadeHaste, suportePrato);
        }
        else
        {
            ConfigurarJoint(suportePrato, extremidadeHaste);
        }
    }

    void ConfigurarJoint(GameObject conectado, GameObject conectadoA)
    {
        ConfigurableJoint joint = conectado.AddComponent<ConfigurableJoint>();
        joint.connectedBody = conectadoA.GetComponent<Rigidbody>();
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
