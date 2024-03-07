using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConectarHasteAoSuporte : MonoBehaviour
{
    void Start()
    {
        GameObject Base = transform.parent.Find("MeioSuporte").gameObject;

        ConfigurarJoint(Base.GetComponent<Rigidbody>());
    }

    private void ConfigurarJoint(Rigidbody suporteRigidbody)
    {
        FixedJoint joint = gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = suporteRigidbody;
        joint.anchor = Vector3.zero;
    }
}


