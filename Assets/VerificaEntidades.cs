using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerificaEntidades : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int X = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool ChecarBagulhoDoido()
    {
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat") || other.CompareTag("Dog") || other.CompareTag("Hamster"))
        {
            //animalsInBox.Add(other.tag);
            CheckCombination();
        }
    }

    void CheckCombination()
    {
        //Debug.log("Denis");
    }

    void ResetPositions()
    {

    }
}
