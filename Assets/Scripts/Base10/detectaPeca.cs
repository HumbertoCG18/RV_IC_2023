using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class checagem : MonoBehaviour
{

    public int unidade, dezena, centena, questaoCount;
    public Collider unidadeCollider, dezenaCollider, centenaCollider;
    public TMP_Text unidadeDisplay, dezenaDisplay, centenaDisplay;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)//Collider.bounds.Contains() checar quando ele entra e, se está no collider certo
    {

        if (other.gameObject.CompareTag("Unidade") /*&& unidadeCollider.bounds.Contains(other.gameObject.transform.position)*/)
        {
            incUnidade();

            if(unidade >= 0 && unidade < 10)
            {
                unidadeDisplay.text = (unidade.ToString());
                Debug.Log("Unidade Incrementada: " + unidade);
            }
            else if(unidade <= 10)
            {
                unidadeDisplay.text = ("Peças demais!");
            }
            else
            {
                unidadeDisplay.text = ("Erro: Contagem negativa detectada, reinicie o exercício");
            }
        }
        else if (other.gameObject.CompareTag("Dezena") /*&& dezenaCollider.bounds.Contains(other.gameObject.transform.position)*/)
        {
            incDezena();

            if (dezena >= 0 && dezena < 10)
            {
                dezenaDisplay.text = (dezena.ToString());
                Debug.Log("Unidade Incrementada: " + dezena);
            }
            else if (dezena <= 10)
            {
                dezenaDisplay.text = ("Peças demais!");
            }
            else
            {
                dezenaDisplay.text = ("Erro: Contagem negativa detectada, reinicie o exercício");
            }
        }
        else if (other.gameObject.CompareTag("Centena") /*&& centenaCollider.bounds.Contains(other.gameObject.transform.position)*/)
        {
            incCentena();
            if (centena >= 0 && centena < 10)
            {
                centenaDisplay.text = (centena.ToString());
                Debug.Log("Unidade Incrementada: " + centena);
            }
            else if (centena <= 10)
            {
                centenaDisplay.text = ("Peças demais!");
            }
            else
            {
                centenaDisplay.text = ("Erro: Contagem negativa detectada, reinicie o exercício");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Unidade"))
        {
            decUnidade();
            unidadeDisplay.text = (unidade.ToString());
            Debug.Log("Unidade Incrementada: " + unidade);
        }
        else if (other.gameObject.CompareTag("Dezena"))
        {
            decDezena();
            dezenaDisplay.text = (dezena.ToString());
            Debug.Log("Dezena Incrementada: " + dezena);
        }
        else if (other.gameObject.CompareTag("Centena"))
        {
            decCentena();
            centenaDisplay.text = (centena.ToString());
            Debug.Log("Centena Incrementada: " + centena);
        }
    }

    void incUnidade()
    {
        unidade++;
        Debug.Log("Unidade Incrementada");
    }

    void decUnidade()
    {
        if (unidade > 0)
        {
            unidade--;
            Debug.Log("Unidade Decrementada");
        }
    }

    void incDezena()
    {
        dezena++;
        Debug.Log("Dezena Incrementada");

    }

    void decDezena()
    {
        if (dezena > 0)
        {
            dezena--;
            Debug.Log("Dezena Decrementada");
        }
    }

    void incCentena()
    {
        centena++;
        Debug.Log("Centena Incrementada");
    }

    void decCentena() 
    {
        if (centena > 0)
        {
            centena--;
            Debug.Log("Centena Decrementada");
        }
    }

    void questoes()
    {

    }
        
}
