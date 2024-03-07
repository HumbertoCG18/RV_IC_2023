using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarMeioAoSuporte : MonoBehaviour
{
    public GameObject meio;
    public GameObject suporteBase;

    void Start()
    {
        ConectarComponentes(meio, suporteBase);
        LimitarMovimento(meio.GetComponent<Rigidbody>());
    }

    void ConectarComponentes(GameObject obj1, GameObject obj2)
    {
        FixedJoint joint = obj1.AddComponent<FixedJoint>();
        joint.connectedBody = obj2.GetComponent<Rigidbody>();
        joint.autoConfigureConnectedAnchor = false;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
    }

    void LimitarMovimento(Rigidbody rb)
    {
        // Limitar movimento angular
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }
}

