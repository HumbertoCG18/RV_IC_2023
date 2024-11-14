using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class checagem : MonoBehaviour
{

    public int unidade, dezena, centena, contador;
    public Collider unidadeCollider, dezenaCollider, centenaCollider;
    public TMP_Text unidadeDisplay, dezenaDisplay, centenaDisplay;

    // Start is called before the first frame update
    void Start()
    {
        contador = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other == unidadeCollider)
        {
            incUnidade();
            unidadeDisplay.text = ("U\n" + unidade);
            Debug.Log("Unidade Incrementada: " + unidade);
        }
        else if (other == dezenaCollider) 
        {
            incDezena();
            dezenaDisplay.text = ("U\n" + dezena);
            Debug.Log("Dezena Incrementada: " + dezena);
        }
        else
        {
            incCentena();
            centenaDisplay.text = ("U\n" + centena);
            Debug.Log("Centena Incrementada: " + centena);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == unidadeCollider)
        {
            decUnidade();
            unidadeDisplay.text = ("U\n" + unidade);
            Debug.Log("Unidade Incrementada: " + unidade);
        }
        else if (other == dezenaCollider)
        {
            decDezena();
            dezenaDisplay.text = ("U\n" + dezena);
            Debug.Log("Dezena Incrementada: " + dezena);
        }
        else
        {
            decCentena();
            centenaDisplay.text = ("U\n" + centena);
            Debug.Log("Centena Incrementada: " + centena);
        }
    }

    void incUnidade()
    {
        if(unidade < 10)
        {
            unidade++;
            Debug.Log("Unidade Incrementada");
        }
    }

    void decUnidade()
    {
        if (unidade >= 0)
        {
            unidade--;
            Debug.Log("Unidade Decrementada");
        }
    }

    void incDezena()
    {
        if (dezena <= 10)
        {
            dezena++;
            Debug.Log("Dezena Incrementada");
        }
    }

    void decDezena()
    {
        if (dezena >= 0)
        {
            dezena--;
            Debug.Log("Dezena Decrementada");
        }
    }

    void incCentena()
    {
        if (centena <= 10)
        {
            centena++;
            Debug.Log("Centena Incrementada");
        }
    }

    void decCentena() 
    {
        if (centena >= 0)
        {
            centena--;
            Debug.Log("Centena Decrementada");
        }
    }

    //public bool checagemResposta()
    //{
        //Checa se as condições são verdadeiras e retorna 
    //}

    //if (other.gameObject.CompareTag(caixa.tag))
        
}
