using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class checagemQuestao : MonoBehaviour
{
    //private int unidade, dezena, centena;
    //public checagem unidadeChecagem, dezenaChecagem, centenaChecagem;
    public DetectaIndividual DetectaUnidade, DetectaDezena, DetectaCentena;
    private int unidade, dezena, centena;

    /*
     if (smallCollider1 == null)
            smallCollider1 = GameObject.Find("SmallCollider1").GetComponent<SmallColliderTrigger>();
        if (smallCollider2 == null)
            smallCollider2 = GameObject.Find("SmallCollider2").GetComponent<SmallColliderTrigger>();
        if (smallCollider3 == null)
            smallCollider3 = GameObject.Find("SmallCollider3").GetComponent<SmallColliderTrigger>();
    }
     */

    // Start is called before the first frame update
    void Start()
    {
        if(DetectaUnidade != null)
        {
            DetectaUnidade = GameObject.Find("AreaUnidade").GetComponent<DetectaIndividual>();
        }
        if(DetectaDezena != null)
        {
            DetectaDezena = GameObject.Find("AreaDezena").GetComponent<DetectaIndividual>();
        }
        if(DetectaCentena != null)
        {
            DetectaCentena = GameObject.Find("AreaCentena").GetComponent<DetectaIndividual>();
        }

        unidade = DetectaUnidade.getMedida();
        dezena = DetectaDezena.getMedida();
        centena = DetectaCentena.getMedida();
    }

    // Update is called once per frame
    void Update()
    {
        //Se houverem mudanças na unidade, dezena ou centena, atualiza o valor alterado.
        if(unidade != DetectaUnidade.getMedida())
        {
            unidade = DetectaUnidade.getMedida();
            Debug.Log("Unidade: " + unidade);
        }

        if(dezena != DetectaDezena.getMedida())
        {
            dezena = DetectaDezena.getMedida();
            Debug.Log("Dezena: " + dezena);
        }

        if(centena != DetectaCentena.getMedida())
        {
            centena = DetectaCentena.getMedida();
            Debug.Log("Centena: " + centena);
        }


    }

}
