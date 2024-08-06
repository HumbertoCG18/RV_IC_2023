using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

/*Arquivo bruto dos controles da mesa e das quest�es, necessita de v�rias otimiza��es, que podem incluir:
 * - Separar o sistema de quest�es em um script distinto
 * - Arrumar a for�a bruta no metodo gerador das fra��es
 * - Encontrar um jeito mais eficiente de realizar o sistema de quest�es
 * - Otimizar o c�digo para legibilidade e compreens�o (urgentemente)
 * - A gera��o das fra��es n�o est� relativa � mesa (Arrumar URGENTE)
*/
public class ControleMesa : MonoBehaviour
{
    public UnityEngine.UI.Button NumAdd, NumDim, DenAdd, DenDim, FracConfirm;
    public float Razao, Raz�oQuest�o;
    public int Numerador, Denominador, NumeradorInicial, NumeradorX, DenominadorX, NumeradorY, DenominadorY, Acertos;
    public bool Certo, CertoX;
    public Vector3 Scale, Position;
    public GameObject FracaoVariavel, FracaoInteira, FracaoInteiraQuestao;
    public TMP_Text ViewNumerador, ViewDenominador, Enunciado, Detalhamento;
    private GameObject[] pecasGeradasInput, pecasGeradasQuestao;


    //Adicionar refer�ncias ao script questoes.cs e adaptar suas funcionalidades(fun��es do questoes.cs) neste script.

    // Start is called before the first frame update
    void Start()
    {

        if (NumAdd != null)
        {
            NumAdd.onClick.AddListener(AddNumerador);
        }
        if (NumDim != null)
        {
            NumDim.onClick.AddListener(DimNumerador);
        }
        if (DenAdd != null)
        {
            DenAdd.onClick.AddListener(AddDenominador);
        }
        if (DenDim != null)
        {
            DenDim.onClick.AddListener(DimDenominador);
        }
        if (FracConfirm != null)
        {
            FracConfirm.onClick.AddListener(FracaoConfirmar);
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
    void AddNumerador()
    {
        if (Numerador < Denominador)
        {
            Numerador++;
        }
    }
    void DimNumerador()
    {
        if (Numerador - 1 > 0)
        {
            Numerador--;
        }
    }
    void DimDenominador()
    {
        if (Denominador - 1 > 0 && Denominador > Numerador)
        {
            Denominador--;
        }
    }
    void AddDenominador()
    {
        if (Denominador > 0 && Denominador < 12)
        {
            Denominador++;
        }
    }
    void FracaoConfirmar()
    {
        DenominadorY = Denominador;
        NumeradorY = Numerador;
        CertoX = checaResultado();
        Debug.Log("Fra��o Confirmada!" + CertoX);

        //Executa fun��o que checa o resultado --> Update dever� ter uma fun��o que checa se h� mudan�a no acerto e executa um retorno positivo ao usu�rio, mudando a fra��o
    }
    //Aqui se encerram

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

            float TamXPeca = EscalaOriginal.x / Denominador;

            for (int i = 0; i < Numerador; i++)
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z);
                Vector3 NovaEscala = FracaoVariavel.transform.localScale;

                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i)/50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0);

                GameObject novaPeca = Instantiate(FracaoVariavel, NovaPosicao, Quaternion.identity, Inteira.transform); //Criando pe�as em posi��es diferentes
                //                                 Prefab       , Position   , Rotation           , Parenting

                NovaEscala.x = TamXPeca; //Mudando o tamanho das pe�as
                novaPeca.transform.localScale = NovaEscala;//Setando o tamanho das pe�as

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

        Raz�oQuest�o =(float)NumeradorInicial / Denominador; // Pega a raz�o da fra��o
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