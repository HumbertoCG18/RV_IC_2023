using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeRotation : MonoBehaviour{
    float lockPos = 0;

    // Update is called once per frame
    void Update(){

        transform.rotation = Quaternion.Euler(lockPos, lockPos, lockPos);

    }
}
