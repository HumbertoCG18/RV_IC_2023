using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.XR.Interaction.Toolkit;

/*Arquivo bruto dos controles da mesa e das quest�es, necessita de v�rias otimiza��es, que podem incluir:
 * - Separar o sistema de quest�es em um script distinto (opcional)
 * - Arrumar a for�a bruta no metodo gerador das fra��es (n�o me recordo ao que se refere, mas provavelmente j� est� em ordem)
 * - Encontrar um jeito mais eficiente de realizar o sistema de quest�es (lembrar que o sistema est� sujeito a altera��es)
 * - Otimizar o c�digo para legibilidade e compreens�o (urgentemente)
 * - A gera��o das fra��es n�o est� relativa � mesa (arrumar)
 * - Fazer com que um bot�o f�sico ative as fun��es -- Interactable Events -- possivelmente usar Activated == Chamar fun��o no event
 * - Arrumar bug em que as fra��es s�o geradas apenas no eixo X
 * - 
*/
public class ControleMesa : MonoBehaviour
{
    public XRBaseInteractable NumAdd, NumDim, DenAdd, DenDim, FracConfirm;
    public float Razao, Raz�oQuest�o;
    public int Numerador, Denominador, NumeradorInicial, NumeradorX, DenominadorX, NumeradorY, DenominadorY, Acertos;
    public bool Certo, CertoX;
    public Vector3 Scale, Position;
    public GameObject FracaoVariavel, ParteVazia, FracaoInteira, FracaoInteiraQuestao;
    public TMP_Text ViewNumerador, ViewDenominador, Enunciado, Detalhamento;
    private GameObject[] pecasGeradasInput, pecasGeradasQuestao;


    //Adicionar refer�ncias ao script questoes.cs e adaptar suas funcionalidades(fun��es do questoes.cs) neste script.

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
        Raz�oQuest�o = 1f;
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

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador); //--Fun��o que gera as fra��es, levando como input a pe�a inteira

        if(Certo != CertoX)
        {
            Nivel(FracaoInteiraQuestao);
        }

    }

    //Aqui come�am as fun��es respons�veis por pegar o input do usu�rio
    void AddNumerador(ActivateEventArgs args) //Se pressionado o bot�o de acrescentar ao numerador, soma 1 ao contador
    {
        if (Numerador < Denominador)//O numerador n�o deve ultrapassar o denominador, evitando fra��es impr�prias
        {
            Numerador++;
            Debug.Log("AddNumerador pressionado");
        }
    }
    void DimNumerador(ActivateEventArgs args)
    {
        if (Numerador - 1 > 0) //O numerador n�o pode ser nulo --> n�o decresce, caso o (valor - 1) seja menor que 0
        {
            Numerador--;
            Debug.Log("DimNumerador pressionado");
        }
    }
    void DimDenominador(ActivateEventArgs args)
    {
        if (Denominador - 1 > 0 && Denominador > Numerador)//O Denominador n�o pode ser nulo e deve ser maior que o numerador
        {
            Denominador--;
            Debug.Log("DimDenominador pressionado");
        }
    }
    void AddDenominador(ActivateEventArgs args)
    {
        if (Denominador > 0 && Denominador < 12) //O denominador deve ser maior que 0 e ter o valor m�ximo de 12
        {
            Denominador++;
            Debug.Log("AddDenominador pressionado");
        }
    }
    void FracaoConfirmar(ActivateEventArgs args)//Bot�o de confirma��o
    {
        DenominadorY = Denominador;//Denominador no momento Y assume o valor inserido pelo usu�rio
        NumeradorY = Numerador; //Numerador no momento Y assume o valor inserido pelo usu�rio
        CertoX = checaResultado();//Vari�vel assume o valor booleano de checaResultado();
        Debug.Log("Fra��o Confirmada!" + CertoX);//� retornado pelo terminal se a resposta est� correta

        //Executa fun��o que checa o resultado --> Update dever� ter uma fun��o que checa se h� mudan�a no acerto e executa um retorno positivo ao usu�rio, mudando a fra��o
    }
    //Aqui se encerram as fun��es respons�veis por pegar o input do usu�rio

    void ViewFracao() //Exibe a fra��o no painel da mesa.
    {
        ViewNumerador.text = Numerador.ToString();
        ViewDenominador.text = Denominador.ToString();
    }

    void GeraFracao(GameObject Inteira, GameObject[] pecasGeradas, int Numerador, int Denominador)//Considerando inputs do usu�rio que v�o at� 12, gerar m�ltiplas (com base no numerador) pe�as do tamanho Inteira/Denominador.
    {//A gera��o das fra��es n�o est� relativa � mesa no eixo X --> arrumar
        if (DenominadorX != Denominador || NumeradorX != Numerador) //Caso haja mudan�as no numerador, ou denominador, o c�digo � executado.
        {
            ResetaFracao(pecasGeradas);

            Vector3 PosicaoOriginal = Inteira.transform.localPosition; //Posi��oOriginal em rela��o a pe�a maior
            Vector3 EscalaOriginal = FracaoVariavel.transform.localScale; //EscalaOriginal por si s�!!!!
            Quaternion MesaAngulo = Inteira.transform.rotation;

            float TamXPeca = EscalaOriginal.x / Denominador;

            for (int i = 0; i < Numerador; i++)
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z);
                Vector3 NovaEscala = FracaoVariavel.transform.localScale;

                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0);

                GameObject novaPeca = Instantiate(FracaoVariavel, NovaPosicao, MesaAngulo, Inteira.transform); //Criando pe�as em posi��es diferentes
                //                                 Prefab       , Position   , Rotation           , Parenting

                NovaEscala.x = TamXPeca; //Mudando o tamanho das pe�as
                novaPeca.transform.localScale = NovaEscala; //Setando o tamanho das pe�as

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
                Destroy(peca); //Fazer de um jeito reutiliz�vel
            }
        }
    }

    //Aqui come�a o sistema de quest�es (Mais pra frente dever�o ser colocados em um outro arquivo por motivos de organiza��o)
    void Nivel(GameObject FracaoInteira) // N�vel I --> Denominador limitado a 4
    {
        CertoX = false;
        Enunciado.text = ("Identifique a fra��o");
        Detalhamento.text = ("Uma fra��o aleat�ria ser� gerada. Utilize a interface da mesa para identificar qual � a fra��o!");

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
                
        int Denominador = UnityEngine.Random.Range(1, DenominadorBase);// Seleciona um denominador aleat�rio <= 4                           
        NumeradorInicial = UnityEngine.Random.Range(1, Denominador); // Seleciona um numerador aleat�rio <= denominador

        GeraFracao(FracaoInteira, pecasGeradasQuestao, NumeradorInicial, Denominador);

        Raz�oQuest�o = (float)NumeradorInicial / Denominador; // Pega a raz�o da fra��o
        Debug.Log("Raz�o da Quest�o :" + Raz�oQuest�o);

        
    }// Talvez fazer uma fun��o s� que limita o denominador atrav�z de um contador;

    bool checaResultado()
    {
        Debug.Log("Raz�o Quest�o: " + Raz�oQuest�o + " Raz�o Input: " + Razao);
        if (Raz�oQuest�o == Razao && Numerador == NumeradorY) //Raz�o Quest�o est� como vari�vel global e alguma coisa a faz retornar falso.
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