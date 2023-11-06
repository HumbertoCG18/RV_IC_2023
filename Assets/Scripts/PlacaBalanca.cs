using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacaBalanca : MonoBehaviour{
    public float Peso;
    public float PesoAtivarMin;
    public float PesoAtivarMax;
    public GameObject Ativar;

    void Update()
    {
        if(Peso < PesoAtivarMin || Peso> PesoAtivarMax)
        {
            Ativar.SetActive(true);    
        }
    }

    void OnTriggerEnter(Collider Coll)
    {
        if (Coll.GetComponent<Rigidbody>())
        {
            Peso += Coll.GetComponent<Rigidbody>().mass;
        }
    }

    void OnTriggerExit(Collider Coll)
    {
        if (!Coll.GetComponent<Rigidbody>())
        {
            Peso -= Coll.GetComponent<Rigidbody>().mass;
        }
    }
}
