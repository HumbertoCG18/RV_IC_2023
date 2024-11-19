using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DetectaIndividual : MonoBehaviour
{
    public int medida;
    public GameObject areaMedida;
    public TMP_Text medidaDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(areaMedida.tag))
        {
            if(medida >= 0 && medida < 9)
            {
                incMedida();
                medidaDisplay.text = (medida.ToString());
            }
            else if(medida >= 10)
            {
                incMedida();
                medidaDisplay.text = ("Peças demais!");
            }
            else
            {
                incMedida();
                medidaDisplay.text = ("Erro: Valor inválido");
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(other.tag) && medida > 0)
        {
            decMedida();
            medidaDisplay.text = (medida.ToString());
        }
    }

    void incMedida()
    {
        medida++;
        Debug.Log("Unidade Incrementada");
    }

    void decMedida()
    {
        if (medida > 0)
        {
            medida--;
            Debug.Log("Unidade Decrementada");
        }
    }

    public int getMedida()
    {
        return medida;
    }
}
