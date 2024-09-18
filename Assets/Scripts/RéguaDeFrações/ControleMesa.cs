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
 * - Qualquer dúvida, referir ao doc da régua, ou perguntar.
*/
public class ControleMesa : MonoBehaviour
{
    public XRBaseInteractable NumAdd, NumDim, DenAdd, DenDim, FracConfirm;
    public float Razao, RazãoQuestão;
    public int Numerador, Denominador, NumeradorInicial, NumeradorX, DenominadorX, NumeradorY, DenominadorY, Acertos, Questao, QuestaoAnt;
    public bool Certo, CertoX;
    public Vector3 Scale, Position;
    public GameObject FracaoVariavel, ParteVazia, FracaoInteira, FracaoInteiraQuestao;
    public TMP_Text ViewNumerador, ViewDenominador, Enunciado, Detalhamento;
    private GameObject[] pecasGeradasInput, pecasGeradasQuestao;
    private int[] equivalenciaUmMeio;


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
        NumeradorY = 1; //NumeradorY e DenominadorY são as variáveis auxiliares responsáveis por armazenar o input do usuário ao pressionar o botão de confirmar
        DenominadorY = 1;

        Numerador = 1; //Estas veriáveis são "flúidas". Se atualizam a todo o momento, com base nos inputs do usuário, por meio de contadores no código.
        Denominador = 1;

        Questao = 1; //Variável que marca a questáo
        QuestaoAnt = 0; //Variável auxiliar responsável por detectar progressão nas questões (Inicializa QuestaoAnt como diferente de Questao, pois este é checado a cada frame [Checar Update()])

        equivalenciaUmMeio = new int[6]; //Equivalências de 1/2 com numerador máximo de 12
        pecasGeradasInput = new GameObject[12];
        pecasGeradasQuestao = new GameObject[12];

        RazãoQuestão = 1f;

        Certo = false;
        CertoX = false;

        Acertos = 0;

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador);
        //Aleatoria(FracaoInteiraQuestao);
    }

    // Update is called once per frame
    void Update()
    {
        Razao = (float)Numerador / Denominador; //Calcula a fração atual a cada frame (Ver como otimizar)

        ViewFracao(); //Atualiza a fração no display a cada frame

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador); //Função que gera as frações 

        //if(Certo != CertoX)
        //{
        //    Aleatoria(FracaoInteiraQuestao); //Se a resposta se tornar correta, executa a questão novamente
        //}

        if(QuestaoAnt != Questao) //Devem ser distintos para executar (Se uma questão ainda está sendo executada, estes serão iguais)
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
                    Debug.Log("Erro, valor de questão inválido");
                    break;
            }
        }

    }

    //Aqui começam as funções responsáveis por pegar o input do usuário
    void AddNumerador(ActivateEventArgs args) //Se pressionado o botão de acrescentar ao numerador, soma 1 ao contador
    {
        if (Numerador < Denominador)//O numerador não deve ultrapassar o denominador, evitando frações impróprias
        {
            Numerador++; //Contador
            Debug.Log("AddNumerador pressionado"); //Debug.Log para checar a funcionalidade do input (Não está funcionando por hora)
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

            for (int i = 0; i < Numerador; i++) //Iniciando em zero, gera as peças com base no numerador
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z); //Pega a posição da peça inteira
                Vector3 NovaEscala = FracaoVariavel.transform.localScale; //Pega a escala da peça variável

                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0); //Cálculo da posição da peça i

                GameObject novaPeca = Instantiate(FracaoVariavel, NovaPosicao, MesaAngulo, Inteira.transform); //Criando peças em posições diferentes
                //                                 Prefab       , Position   , Rotation           , Parenting

                NovaEscala.x = TamXPeca; //Mudando o tamanho das peças
                novaPeca.transform.localScale = NovaEscala; //Setando o tamanho das peças

                pecasGeradas[i] = novaPeca; //Armazena as peças geradas em um array, possibilitando excluí-las depois
            }
            for(int i = Numerador; i < Denominador; i++) //Iniciando no numerador, gera as peças "vazias" com base no denominador
            {
                Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z);
                Vector3 NovaEscala = FracaoVariavel.transform.localScale; //Pega a escala da peça que será modificada
                
                NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0);
                
                GameObject novaPeca = Instantiate(ParteVazia, NovaPosicao, MesaAngulo, Inteira.transform); //Gera a nova peça

                NovaEscala.x = TamXPeca; //Ajusta o X do Vector3 (Posição) da peça
                novaPeca.transform.localScale = NovaEscala; //Define a escala da peça que será modificada para a nova escala

                pecasGeradas[i] = novaPeca; //Armazena as peças geradas em um array, possibilitando excluí-las depois
            }
        }
        //Numerador e Denominador em um momento anterior
        NumeradorX = Numerador; //Armazena o valor do numerador no fim da execução
        DenominadorX = Denominador; //Armazena o valor do numerador no fim da execução
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

    //Aqui começa o sistema de questões (Mais pra frente poderão ser colocados em um outro arquivo por motivos de organização)

    bool checaResultado() // Checa se todas as condições foram cumpridas
    {
        switch (Questao)
        {
            case 1:
                //Condições Q1 --> Fração 1/2 encontrada e todas as equivalentes dentro de (1 <= Denominador <= 12)
                checaQ1();
                return true;
            case 2:
                //Condições Q2
                Questao++;
                return true;
            case 3:
                //Condições Q3
                Questao++;
                return true;
            default:
                return false;

        }
    }

    void Aleatoria(GameObject FracaoInteira)
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
                
        int Denominador = UnityEngine.Random.Range(2, DenominadorBase);// Seleciona um denominador aleatório <= 4                           
        NumeradorInicial = UnityEngine.Random.Range(1, Denominador); // Seleciona um numerador aleatório <= denominador

        GeraFracao(FracaoInteira, pecasGeradasQuestao, NumeradorInicial, Denominador);

        RazãoQuestão = (float)NumeradorInicial / Denominador; // Pega a razão da fração
        Debug.Log("Razão da Questão :" + RazãoQuestão);

        
    }//Talvez fazer uma função só que limita o denominador atravéz de um contador;

    void Questao1() //Utilizando os botões da interface, selecione a fração que representa 1/2 da unidade. Após, selecione todas as outras frações equivalentes a 1/2.
    {
        //Setar textos
        Enunciado.text = ("Identifique a fração");
        Detalhamento.text = ("Uma fração aleatória será gerada. Utilize a interface da mesa para identificar qual é a fração!");
        //Deixar peças não usadas invisíveis
        // <Inserir código aqui>
        if (CertoX == true)
        {
            Questao++;
        }
    }

    //Criar um método bool que checa as condições de cada questão e, será usado simultaneamente com o Questao1() e o fracaoConfirm()
    bool checaQ1()
    {
        if(NumeradorY == 1 && DenominadorY == 2) //Se o input for 1/2
        {
            Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2"); //Descubra as frações equivalentes
            int i = 0;
            while(equivalenciaUmMeio[5] == null) //Enquanto o vetor não estiver cheio
            {
                float RazaoY = (float)NumeradorY / DenominadorY;

                if(RazaoY == 0.5 && pertenceAoArray(equivalenciaUmMeio, DenominadorY) == false)
                {
                    //Mais tarde haverá o retorno auditivo aqui
                    Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2\n" + i + "de" + 6 + "Descobertas");

                    equivalenciaUmMeio[i] = DenominadorY;
                    i++;
                }

            }
            Detalhamento.text = ("Muito bem! Carregando a próxima questão...");
            //Passa a questão? (Atualmente no método Questao1())
            //Pausar execução do código por X segundos
            return true;
        }
        else
        {
            return false;
        }
    }

    bool pertenceAoArray(int[] equivalentes, int DenominadorY) //Método temporário que eu criei pra comparar os elementos de um array
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

    void Questao2() //Utilizando os botões da interface, seleciona todas as frações equivalentes a 1/3.
    {

    }

    void Questao3() //Observe o valor desconhecido gerado acima. Utilizando os botões da interface e testando outros valores, encontre todas as frações equivalentes a este valor desconhecido.
    {

    }

}