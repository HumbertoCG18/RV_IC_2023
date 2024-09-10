using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

/*Arquivo bruto dos controles da mesa e das questões, necessita de várias otimizações, que podem incluir:
 * - Separar o sistema de questões em um script distinto (opcional)
 * - Arrumar a força bruta no metodo gerador das frações (não me recordo ao que se refere, mas provavelmente já está em ordem)
 * - Encontrar um jeito mais eficiente de realizar o sistema de questões (lembrar que o sistema está sujeito a alterações)
 * - Otimizar o código para legibilidade e compreensão (urgentemente)
 * - A geração das frações não está relativa à mesa (arrumar)
 * - Fazer com que um botão físico ative as funções -- Interactable Events -- possivelmente usar Activated == Chamar função no event
 * - Arrumar bug em que as frações são geradas apenas no eixo X
 * - 
*/
public class ControleMesa : MonoBehaviour
{
    public XRBaseInteractable NumAdd, NumDim, DenAdd, DenDim, FracConfirm;
    public float Razao, RazãoQuestão;
    public int Numerador, Denominador, NumeradorInicial, NumeradorX, DenominadorX, NumeradorY, DenominadorY, Acertos;
    public bool Certo, CertoX;
    public Vector3 Scale, Position;
    public GameObject FracaoVariavel, ParteVazia, FracaoInteira, FracaoInteiraQuestao;
    public TMP_Text ViewNumerador, ViewDenominador, Enunciado, Detalhamento;
    private GameObject[] pecasGeradasInput, pecasGeradasQuestao;


    //Adicionar referências ao script questoes.cs e adaptar suas funcionalidades(funções do questoes.cs) neste script.

    // Start is called before the first frame update
    void Start()
    {

        if (NumAdd != null)
        {
            NumAdd.activated.AddListener(AddNumerador);
        }
        if (NumDim != null)
        {
            NumDim.activated.AddListener(DimNumerador);
        }
        if (DenAdd != null)
        {
            DenAdd.activated.AddListener(AddDenominador);
        }
        if (DenDim != null)
        {
            DenDim.activated.AddListener(DimDenominador);
        }
        if (FracConfirm != null)
        {
            FracConfirm.activated.AddListener(FracaoConfirmar);
        }

        NumeradorInicial = 1;
        NumeradorY = 1;
        DenominadorY = 1;
        Numerador = 1;
        Denominador = 1;
        pecasGeradasInput = new GameObject[12];
        pecasGeradasQuestao = new GameObject[12];
        RazãoQuestão = 1f;
        Certo = false;
        CertoX = false;
        Acertos = 0;

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador);
        Nivel(FracaoInteiraQuestao);
    }

    // Update is called once per frame
    void Update()
    {
        Razao = (float)Numerador / Denominador;

        ViewFracao();

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador); //--Função que gera as frações, levando como input a peça inteira

        if(Certo != CertoX)
        {
            Nivel(FracaoInteiraQuestao);
        }

    }

    //Aqui começam as funções responsáveis por pegar o input do usuário
    void AddNumerador(ActivateEventArgs args) //Se pressionado o botão de acrescentar ao numerador, soma 1 ao contador
    {
        if (Numerador < Denominador)//O numerador não deve ultrapassar o denominador, evitando frações impróprias
        {
            Numerador++;
            Debug.Log("AddNumerador pressionado");
        }
    }
    void DimNumerador(ActivateEventArgs args)
    {
        if (Numerador - 1 > 0) //O numerador não pode ser nulo --> não decresce, caso o (valor - 1) seja menor que 0
        {
            Numerador--;
            Debug.Log("DimNumerador pressionado");
        }
    }
    void DimDenominador(ActivateEventArgs args)
    {
        if (Denominador - 1 > 0 && Denominador > Numerador)//O Denominador não pode ser nulo e deve ser maior que o numerador
        {
            Denominador--;
            Debug.Log("DimDenominador pressionado");
        }
    }
    void AddDenominador(ActivateEventArgs args)
    {
        if (Denominador > 0 && Denominador < 12) //O denominador deve ser maior que 0 e ter o valor máximo de 12
        {
            Denominador++;
            Debug.Log("AddDenominador pressionado");
        }
    }
    void FracaoConfirmar(ActivateEventArgs args)//Botão de confirmação
    {
        DenominadorY = Denominador;//Denominador no momento Y assume o valor inserido pelo usuário
        NumeradorY = Numerador; //Numerador no momento Y assume o valor inserido pelo usuário
        CertoX = checaResultado();//Variável assume o valor booleano de checaResultado();
        Debug.Log("Fração Confirmada!" + CertoX);//É retornado pelo terminal se a resposta está correta

        //Executa função que checa o resultado --> Update deverá ter uma função que checa se há mudança no acerto e executa um retorno positivo ao usuário, mudando a fração
    }
    //Aqui se encerram as funções responsáveis por pegar o input do usuário

    void ViewFracao() //Exibe a fração no painel da mesa.
    {
        ViewNumerador.text = Numerador.ToString();
        ViewDenominador.text = Denominador.ToString();
    }

    void GeraFracao(GameObject Inteira, GameObject[] pecasGeradas, int Numerador, int Denominador)//Considerando inputs do usuário que vão até 12, gerar múltiplas (com base no numerador) peças do tamanho Inteira/Denominador.
    {//A geração das frações não está relativa à mesa no eixo X --> arrumar
        if (DenominadorX != Denominador || NumeradorX != Numerador) //Caso haja mudanças no numerador, ou denominador, o código é executado.
        {
            ResetaFracao(pecasGeradas);

            Vector3 PosicaoOriginal = Inteira.transform.localPosition; //PosiçãoOriginal em relação a peça maior
            Vector3 EscalaOriginal = FracaoVariavel.transform.localScale; //EscalaOriginal por si só!!!!
            Quaternion MesaAngulo = Inteira.transform.rotation;

            float TamXPeca = EscalaOriginal.x / Denominador;

            for (int i = 0; i < Numerador; i++)
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z);
                Vector3 NovaEscala = FracaoVariavel.transform.localScale;

                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0);

                GameObject novaPeca = Instantiate(FracaoVariavel, NovaPosicao, MesaAngulo, Inteira.transform); //Criando peças em posições diferentes
                //                                 Prefab       , Position   , Rotation           , Parenting

                NovaEscala.x = TamXPeca; //Mudando o tamanho das peças
                novaPeca.transform.localScale = NovaEscala; //Setando o tamanho das peças

                pecasGeradas[i] = novaPeca;
            }
            for(int i = Numerador; i < Denominador; i++)
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z);
                Vector3 NovaEscala = FracaoVariavel.transform.localScale;
                
                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0);
                
                GameObject novaPeca = Instantiate(ParteVazia, NovaPosicao, MesaAngulo, Inteira.transform);

                NovaEscala.x = TamXPeca;
                novaPeca.transform.localScale = NovaEscala;

                pecasGeradas[i] = novaPeca;
            }
        }
        //Numerador e Denominador em um momento anterior
        NumeradorX = Numerador;
        DenominadorX = Denominador;
    }

    void ResetaFracao(GameObject[] pecasGeradas)
    {
        foreach(GameObject peca in pecasGeradas)
        {
            if (peca != null) 
            {
                Destroy(peca); //Fazer de um jeito reutilizável
            }
        }
    }

    //Aqui começa o sistema de questões (Mais pra frente deverão ser colocados em um outro arquivo por motivos de organização)
    void Nivel(GameObject FracaoInteira) // Nível I --> Denominador limitado a 4
    {
        CertoX = false;
        Enunciado.text = ("Identifique a fração");
        Detalhamento.text = ("Uma fração aleatória será gerada. Utilize a interface da mesa para identificar qual é a fração!");

        int DenominadorBase = 0;

        if(Acertos == 0)
        {
            DenominadorBase = 4;
        }
        else if(Acertos > 1 && Acertos < 4)
        {
            DenominadorBase = 4 * Acertos;
        }
        else
        {
            DenominadorBase = 12;
        }
                
        int Denominador = UnityEngine.Random.Range(1, DenominadorBase);// Seleciona um denominador aleatório <= 4                           
        NumeradorInicial = UnityEngine.Random.Range(1, Denominador); // Seleciona um numerador aleatório <= denominador

        GeraFracao(FracaoInteira, pecasGeradasQuestao, NumeradorInicial, Denominador);

        RazãoQuestão = (float)NumeradorInicial / Denominador; // Pega a razão da fração
        Debug.Log("Razão da Questão :" + RazãoQuestão);

        
    }// Talvez fazer uma função só que limita o denominador atravéz de um contador;

    bool checaResultado()
    {
        Debug.Log("Razão Questão: " + RazãoQuestão + " Razão Input: " + Razao);
        if (RazãoQuestão == Razao && Numerador == NumeradorY) //Razão Questão está como variável global e alguma coisa a faz retornar falso.
        {         
            Acertos++;
            return true;
        }
        else
        {
            return false;
        }
    }

}