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
 * - Qualquer d�vida, referir ao doc da r�gua, ou perguntar.
*/
public class ControleMesa : MonoBehaviour
{
    public XRBaseInteractable NumAdd, NumDim, DenAdd, DenDim, FracConfirm;
    public float Razao, Raz�oQuest�o;
    public int Numerador, Denominador, NumeradorInicial, NumeradorX, DenominadorX, NumeradorY, DenominadorY, Acertos, Questao, QuestaoAnt;
    public bool Certo, CertoX;
    public Vector3 Scale, Position;
    public GameObject FracaoVariavel, ParteVazia, FracaoInteira, FracaoInteiraQuestao;
    public TMP_Text ViewNumerador, ViewDenominador, Enunciado, Detalhamento;
    private GameObject[] pecasGeradasInput, pecasGeradasQuestao;
    private int[] equivalenciaUmMeio;


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
        NumeradorY = 1; //NumeradorY e DenominadorY s�o as vari�veis auxiliares respons�veis por armazenar o input do usu�rio ao pressionar o bot�o de confirmar
        DenominadorY = 1;

        Numerador = 1; //Estas veri�veis s�o "fl�idas". Se atualizam a todo o momento, com base nos inputs do usu�rio, por meio de contadores no c�digo.
        Denominador = 1;

        Questao = 1; //Vari�vel que marca a quest�o
        QuestaoAnt = 0; //Vari�vel auxiliar respons�vel por detectar progress�o nas quest�es (Inicializa QuestaoAnt como diferente de Questao, pois este � checado a cada frame [Checar Update()])

        equivalenciaUmMeio = new int[6]; //Equival�ncias de 1/2 com numerador m�ximo de 12
        pecasGeradasInput = new GameObject[12];
        pecasGeradasQuestao = new GameObject[12];

        Raz�oQuest�o = 1f;

        Certo = false;
        CertoX = false;

        Acertos = 0;

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador);
        //Aleatoria(FracaoInteiraQuestao);
    }

    // Update is called once per frame
    void Update()
    {
        Razao = (float)Numerador / Denominador; //Calcula a fra��o atual a cada frame (Ver como otimizar)

        ViewFracao(); //Atualiza a fra��o no display a cada frame

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador); //Fun��o que gera as fra��es 

        //if(Certo != CertoX)
        //{
        //    Aleatoria(FracaoInteiraQuestao); //Se a resposta se tornar correta, executa a quest�o novamente
        //}

        if(QuestaoAnt != Questao) //Devem ser distintos para executar (Se uma quest�o ainda est� sendo executada, estes ser�o iguais)
        {
            Debug.Log("Atualmente executando a questao " + Questao);
            QuestaoAnt = Questao;
            switch (Questao)
            {
                case 1:
                    Questao1();
                    break;
                case 2:
                    Questao2(); 
                    break;
                case 3:
                    Questao3();
                    break;
                default:
                    Debug.Log("Erro, valor de quest�o inv�lido");
                    break;
            }
        }

    }

    //Aqui come�am as fun��es respons�veis por pegar o input do usu�rio
    void AddNumerador(ActivateEventArgs args) //Se pressionado o bot�o de acrescentar ao numerador, soma 1 ao contador
    {
        if (Numerador < Denominador)//O numerador n�o deve ultrapassar o denominador, evitando fra��es impr�prias
        {
            Numerador++; //Contador
            Debug.Log("AddNumerador pressionado"); //Debug.Log para checar a funcionalidade do input (N�o est� funcionando por hora)
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

            for (int i = 0; i < Numerador; i++) //Iniciando em zero, gera as pe�as com base no numerador
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z); //Pega a posi��o da pe�a inteira
                Vector3 NovaEscala = FracaoVariavel.transform.localScale; //Pega a escala da pe�a vari�vel

                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0); //C�lculo da posi��o da pe�a i

                GameObject novaPeca = Instantiate(FracaoVariavel, NovaPosicao, MesaAngulo, Inteira.transform); //Criando pe�as em posi��es diferentes
                //                                 Prefab       , Position   , Rotation           , Parenting

                NovaEscala.x = TamXPeca; //Mudando o tamanho das pe�as
                novaPeca.transform.localScale = NovaEscala; //Setando o tamanho das pe�as

                pecasGeradas[i] = novaPeca; //Armazena as pe�as geradas em um array, possibilitando exclu�-las depois
            }
            for(int i = Numerador; i < Denominador; i++) //Iniciando no numerador, gera as pe�as "vazias" com base no denominador
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z);
                Vector3 NovaEscala = FracaoVariavel.transform.localScale; //Pega a escala da pe�a que ser� modificada
                
                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0);
                
                GameObject novaPeca = Instantiate(ParteVazia, NovaPosicao, MesaAngulo, Inteira.transform); //Gera a nova pe�a

                NovaEscala.x = TamXPeca; //Ajusta o X do Vector3 (Posi��o) da pe�a
                novaPeca.transform.localScale = NovaEscala; //Define a escala da pe�a que ser� modificada para a nova escala

                pecasGeradas[i] = novaPeca; //Armazena as pe�as geradas em um array, possibilitando exclu�-las depois
            }
        }
        //Numerador e Denominador em um momento anterior
        NumeradorX = Numerador; //Armazena o valor do numerador no fim da execu��o
        DenominadorX = Denominador; //Armazena o valor do numerador no fim da execu��o
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

    //Aqui come�a o sistema de quest�es (Mais pra frente poder�o ser colocados em um outro arquivo por motivos de organiza��o)

    bool checaResultado() // Checa se todas as condi��es foram cumpridas
    {
        switch (Questao)
        {
            case 1:
                //Condi��es Q1 --> Fra��o 1/2 encontrada e todas as equivalentes dentro de (1 <= Denominador <= 12)
                checaQ1();
                return true;
            case 2:
                //Condi��es Q2
                Questao++;
                return true;
            case 3:
                //Condi��es Q3
                Questao++;
                return true;
            default:
                return false;

        }
    }

    void Aleatoria(GameObject FracaoInteira)
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
                
        int Denominador = UnityEngine.Random.Range(2, DenominadorBase);// Seleciona um denominador aleat�rio <= 4                           
        NumeradorInicial = UnityEngine.Random.Range(1, Denominador); // Seleciona um numerador aleat�rio <= denominador

        GeraFracao(FracaoInteira, pecasGeradasQuestao, NumeradorInicial, Denominador);

        Raz�oQuest�o = (float)NumeradorInicial / Denominador; // Pega a raz�o da fra��o
        Debug.Log("Raz�o da Quest�o :" + Raz�oQuest�o);

        
    }//Talvez fazer uma fun��o s� que limita o denominador atrav�z de um contador;

    void Questao1() //Utilizando os bot�es da interface, selecione a fra��o que representa 1/2 da unidade. Ap�s, selecione todas as outras fra��es equivalentes a 1/2.
    {
        //Setar textos
        Enunciado.text = ("Identifique a fra��o");
        Detalhamento.text = ("Uma fra��o aleat�ria ser� gerada. Utilize a interface da mesa para identificar qual � a fra��o!");
        //Deixar pe�as n�o usadas invis�veis
        // <Inserir c�digo aqui>
        if (CertoX == true)
        {
            Questao++;
        }
    }

    //Criar um m�todo bool que checa as condi��es de cada quest�o e, ser� usado simultaneamente com o Questao1() e o fracaoConfirm()
    bool checaQ1()
    {
        if(NumeradorY == 1 && DenominadorY == 2) //Se o input for 1/2
        {
            Detalhamento.text = ("Agora descubra todas as fra��es que equivalentes a 1/2"); //Descubra as fra��es equivalentes
            int i = 0;
            while(equivalenciaUmMeio[5] == null) //Enquanto o vetor n�o estiver cheio
            {
                float RazaoY = (float)NumeradorY / DenominadorY;

                if(RazaoY == 0.5 && pertenceAoArray(equivalenciaUmMeio, DenominadorY) == false)
                {
                    //Mais tarde haver� o retorno auditivo aqui
                    Detalhamento.text = ("Agora descubra todas as fra��es que equivalentes a 1/2\n" + i + "de" + 6 + "Descobertas");

                    equivalenciaUmMeio[i] = DenominadorY;
                    i++;
                }

            }
            Detalhamento.text = ("Muito bem! Carregando a pr�xima quest�o...");
            //Passa a quest�o? (Atualmente no m�todo Questao1())
            //Pausar execu��o do c�digo por X segundos
            return true;
        }
        else
        {
            return false;
        }
    }

    bool pertenceAoArray(int[] equivalentes, int DenominadorY) //M�todo tempor�rio que eu criei pra comparar os elementos de um array
    {
        foreach (int i in equivalentes)
        {
            if (equivalentes[i] == DenominadorY)
            {
                return true;
            }
        }
        return false;
    }

    void Questao2() //Utilizando os bot�es da interface, seleciona todas as fra��es equivalentes a 1/3.
    {

    }

    void Questao3() //Observe o valor desconhecido gerado acima. Utilizando os bot�es da interface e testando outros valores, encontre todas as fra��es equivalentes a este valor desconhecido.
    {

    }

}