using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class caixaSpawn : MonoBehaviour
{
    public GameObject caixa, peca;
    public Vector3 centroCaixa;
    public Quaternion rotacaoCaixa;
    public bool dentroDaCaixa;
    public XRSocketInteractor interactor;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 Altura = new Vector3(0, 0.4f, 0);
        centroCaixa = caixa.transform.position + Altura;
        rotacaoCaixa = caixa.transform.rotation;
        dentroDaCaixa = false;

    }
    /*
     * 
     * 
     * 
     * 
     */

    // Update is called once per frame
    void Update()
    {

    }

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag(caixa.tag)) 
        {
            dentroDaCaixa = true;
        }
    }*/

    /*private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(caixa.tag))
        {
            dentroDaCaixa = false;
        }
    }*/

    //REMOVER o método auxiliar (Foi colocado só por questão de tempo)
    void spawnPecaAux()
    {
        Instantiate(peca, centroCaixa, rotacaoCaixa);
    }

    void spawnPeca()
    {
        Invoke("spawnPecaAux", 0.5f);
    }
}
