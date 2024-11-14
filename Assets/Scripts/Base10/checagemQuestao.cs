using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class checagemQuestao : MonoBehaviour
{
    private int unidade, dezena, centena;
    public checagem unidadeChecagem, dezenaChecagem, centenaChecagem;
    // Start is called before the first frame update
    void Start()
    {
        unidade = 0;
        dezena = 0;
        centena = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void incUnidade()
    {
        if (unidade < 10)
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

    int getUnidade()
    {
        return unidade;
    }
    int getDezena()
    {
        return dezena;
    }
    int getCentena()
    {
        return centena;
    }
}
