using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class caixaSpawn : MonoBehaviour
{
    public GameObject caixa, peca;
    Vector3 altura;

    // Start is called before the first frame update
    void Start()
    {
        altura = new Vector3(0, 0.4f, 0);
        Instantiate(peca, (caixa.transform.position + altura), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //REMOVER o método auxiliar (Foi colocado só por questão de tempo)
    void spawnPecaAux()
    {
        Instantiate(peca, (caixa.transform.position + altura), Quaternion.identity);
    }

    void spawnPeca()
    {
        Invoke("spawnPecaAux", 0.5f);
    }
}
