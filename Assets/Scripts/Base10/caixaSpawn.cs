using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class caixaSpawn : MonoBehaviour
{
    public GameObject caixa, peca;
    public Vector3 centroCaixa;
    public Quaternion rotacaoCaixa;
    public bool dentroDaCaixa;
    public Collider caixaCollider, pecaCollider;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Altura = new Vector3(0, 1.2f, 0);
        centroCaixa = caixa.transform.position + Altura;
        rotacaoCaixa = caixa.transform.rotation;
        dentroDaCaixa = false;
        spawnPeca();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dentroDaCaixa)
        {
            spawnPeca();
            dentroDaCaixa = true;
        }
    }

    bool foraDaCaixa()
    {
        

        if (1 == 1)//Fora da caixa
        {
            //return true;
        }
        //Se dentro da caixa, false
        return false;
    }

    void spawnPeca()
    {
        //Instantiate(Prefab, Position, Rotation, Parenting)
        Instantiate(peca, centroCaixa, rotacaoCaixa);

    }
}
