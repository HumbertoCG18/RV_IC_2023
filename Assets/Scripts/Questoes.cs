using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Questões : MonoBehaviour
{
    public TMP_Text Enunciado, Detalhamento;
    public GameObject FracaoVariavel, FracaoInteira, Mesa;
    public ControleMesa mesa;
    



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    void ViewFracao() //Exibe a fração no painel da mesa.
    {
        ViewNumerador.text = Numerador.ToString();
        ViewDenominador.text = Denominador.ToString();
    }
    */

    //Instantiate(FracaoInteira, ) --> Criar a fração inteira que e 

    //Limitar o denominador pra 4, exercícios de identificação
    //Muda Enunciado e Detalhamento
    //Seleciona uma fração aleatória cujo denominador (e por consequência o numerador) é < 4.
    //Identifique a fração

    void GeraInteira(int Numerador, int Denominador) //por hora o protótipo não vai usar a função geradora
    {
        Vector3 Offset = new Vector3(0, 0, 0);
        Vector3 PosicaoInteira = Mesa.transform.position + Offset;

        //Instantiate(FracaoInteira, ) --> Gerar a fração inteira na posiçao correta da mesa
    }

    public bool NivelI(GameObject FracaoInteira, float RazãoInput, int NumeradorInput) // Nível I --> Denominador limitado a 4
    {
        Enunciado.text = ("Identifique a fração");
        Detalhamento.text = ("Uma fração aleatória será gerada. Utilize a interface da mesa para identificar qual é a fração!");

        int Denominador = Random.Range(1, 4); // Seleciona um denominador aleatório <= 4
        int Numerador = Random.Range(1, Denominador); // Seleciona um numerador aleatório <= denominador

        //GeraFracao(FracaoInteira, Numerador, Denominador);

        float RazãoQuestão = Numerador / Denominador; // Pega a razão da fração

        if(RazãoInput == RazãoQuestão && Numerador == NumeradorInput) //Se a razão e o numerador(Excluir as frações equivalentes) são iguais ao input, então resposta correta
        {
            return true;
        }
        else // Senão, falso. Tente de novo
        {
            return false;
        }
    }

    public void Printar()
    {
        Debug.Log("banana");
    }

    void NivelII() // Nível II --> Denominador limitado a 8
    {
        //Limitar o denominador a 8, exercícios de identificação
    }

    void NivelIII() // Nível II --> Denominador limitado a 12
    {
        //Denominador == 12, exercícios de identificação
    }

    void NivelIV() // Nível IV --> Denominador 4, exercícios de comparação
    {
        //Denominador 4, exercícios de comparação
    }

}
