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
 * - Otimizar o código para legibilidade e compreensão
 * - A geração das frações não está relativa à mesa (arrumar)
 * - Fazer com que um botão físico ative as funções -- Interactable Events -- possivelmente usar Activated == Chamar função no event
 * - Arrumar bug em que as frações são geradas apenas no eixo X
 * - Qualquer dúvida, referir ao doc da régua, ou perguntar.
 * - O MÉTODO bool checaQ1() --> ESTÁ CRASHANDO A UNITY (URGENTE)
*/
public class ControleMesa : MonoBehaviour
{
    public XRBaseInteractable NumAdd, NumDim, DenAdd, DenDim, FracConfirm;
    public float Razao, RazaoY, RazãoQuestão;
    public int Numerador, Denominador, NumeradorInicial, NumeradorX, DenominadorX, NumeradorY, DenominadorY, Acertos, Questao, QuestaoAnt, ContagemEquivalencia;
    public bool Certo, CertoX, confirmarDebug;
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
        ContagemEquivalencia = 0;

        Numerador = 1; //Estas veriáveis são "flúidas". Se atualizam a todo o momento, com base nos inputs do usuário, por meio de contadores no código.
        Denominador = 1;

        Questao = 1; //Variável que marca a questáo
        QuestaoAnt = 0; //Variável auxiliar responsável por detectar progressão nas questões (Inicializa QuestaoAnt como diferente de Questao, pois este é checado a cada frame [Checar Update()])

        equivalenciaUmMeio = new int[5]; //Equivalências de 1/2 com numerador máximo de 12
        pecasGeradasInput = new GameObject[12];
        pecasGeradasQuestao = new GameObject[12];

        RazãoQuestão = 1f;

        confirmarDebug = false; //Variável utilizada para testar as entradas sem o VR conectado
        Certo = false;
        CertoX = false;

        Acertos = 0;

        RazaoY = 1f;

        GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador);
        //Aleatoria(FracaoInteiraQuestao);
    }

    // Update is called once per frame
    void Update()
    {
        Razao = (float)Numerador / Denominador; //Calcula a fração atual a cada frame (Ver como otimizar)

        ViewFracao(); //Atualiza a fração no display a cada frame

        //if(Certo != CertoX)
        //{
        //    Aleatoria(FracaoInteiraQuestao); //Se a resposta se tornar correta, executa a questão novamente
        //}

        if (DenominadorX != Denominador || NumeradorX != Numerador) //Se houverem mudanças no numerador ou denominador o método é executado
        {
            GeraFracao(FracaoInteira, pecasGeradasInput, Numerador, Denominador); //Função que gera as frações 
        }

        if (QuestaoAnt != Questao) //Devem ser distintos para executar (Se uma questão ainda está sendo executada, estes serão iguais)
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

        if (confirmarDebug == true) //Utilizado para definir os inputs sem o VR conectado
        {
            FracaoConfirmarDebug();
            confirmarDebug = false;
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
        CertoX = false;

        //Executa função que checa o resultado --> Update deverá ter uma função que checa se há mudança no acerto e executa um retorno positivo ao usuário, mudando a fração
    }

    void FracaoConfirmarDebug() //Método de confirmação Debug (Sem o VR Conectado)
    {
        DenominadorY = Denominador;
        NumeradorY = Numerador;
        RazaoY = (float)NumeradorY / DenominadorY;

        CertoX = checaResultado();
        Debug.Log("Fração Confirmada pelo Debug: " + CertoX);
    }

    //Aqui se encerram as funções responsáveis por pegar o input do usuário

    void ViewFracao() //Exibe a fração no painel da mesa.
    {
        ViewNumerador.text = Numerador.ToString();
        ViewDenominador.text = Denominador.ToString();
    }

    void GeraFracao(GameObject Inteira, GameObject[] pecasGeradas, int Numerador, int Denominador)//Considerando inputs do usuário que vão até 12, gerar múltiplas (com base no numerador) peças do tamanho Inteira/Denominador.
    {
        ResetaFracao(pecasGeradas);

        Vector3 PosicaoOriginal = Inteira.transform.localPosition; //Armazena a posição da peça inteira
        Vector3 EscalaOriginal = FracaoVariavel.transform.localScale; //Armazena a escala da peça inteira
        Quaternion MesaAngulo = Inteira.transform.rotation; //Armazena a rotação da peça original

        float TamXPeca = EscalaOriginal.x / Denominador; //É calculado o tamanho das peças que serão geradas com base no denominador

        for (int i = 0; i < Numerador; i++) //Iniciando em zero, gera as peças com base no numerador
        {
            Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z); //Pega a posição da peça inteira
            Vector3 NovaEscala = FracaoVariavel.transform.localScale; //Pega a escala da peça variável

            NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0); //Cálculo da posição da peça i

            GameObject novaPeca = Instantiate(FracaoVariavel, NovaPosicao, MesaAngulo, Inteira.transform); //Criando peças em posições diferentes
                                                                                                           //                                 Prefab       , Position   , Rotation           , Parenting

            NovaEscala.x = TamXPeca; //Mudando o tamanho das peças
            novaPeca.transform.localScale = NovaEscala; //Setando o tamanho das peças

            pecasGeradas[i] = novaPeca; //Armazena as "peças geradas em um array, possibilitando excluí-las depois
        }
        for (int i = Numerador; i < Denominador; i++) //Iniciando no numerador, gera as peças "vazias" com base no denominador
        {
            Vector3 NovaPosicao = new Vector3(Inteira.transform.position.x, Inteira.transform.position.y, Inteira.transform.position.z);
            Vector3 NovaEscala = FracaoVariavel.transform.localScale; //Pega a escala da peça que será modificada

            NovaPosicao = NovaPosicao + new Vector3((TamXPeca * i) / 50 - (EscalaOriginal.x / 100) + (TamXPeca / 100), 0, 0);

            GameObject novaPeca = Instantiate(ParteVazia, NovaPosicao, MesaAngulo, Inteira.transform); //Gera a nova peça

            NovaEscala.x = TamXPeca; //Ajusta o X do Vector3 (Posição) da peça
            novaPeca.transform.localScale = NovaEscala; //Define a escala da peça que será modificada para a nova escala

            pecasGeradas[i] = novaPeca; //Armazena as peças geradas em um array, possibilitando excluí-las depois
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
                return checaQ1();
            case 2:
                //Condições Q2
                return checaQ2();
            case 3:
                //Condições Q3
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
        Enunciado.text = ("Identifique 1/2");
        Detalhamento.text = ("Utilizando os botões da interface, selecione a fração que representa 1/2 da unidade");
        //Deixar peças não usadas invisíveis
        // <Inserir código aqui>
    }

    //Criar um método bool que checa as condições de cada questão e, será usado simultaneamente com o Questao1() e o fracaoConfirm()
    bool checaQ1() //Está crashando a unity
    {
        if (ContagemEquivalencia < 5) //Se o o número de frações equivalentes encontradas não for == 6
        {
            if (RazaoY == 0.5f && pertenceAoArray(equivalenciaUmMeio, DenominadorY) == false)
            {
                Debug.Log("ChecandoQ1");
                //Mais tarde haverá o retorno auditivo aqui
                Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2\n" + ContagemEquivalencia + "de" + 7 + "Descobertas");
                Debug.Log("ContagemEquivalencia: " + ContagemEquivalencia);
                equivalenciaUmMeio[ContagemEquivalencia] = DenominadorY;
                ContagemEquivalencia++;
            }
            if (pertenceAoArray(equivalenciaUmMeio, 2) == true)
            {
                Detalhamento.text = ("Agora descubra todas as frações que equivalentes a 1/2");
            }
        }
        else
        {
            Detalhamento.text = ("Muito bem! Carregando a próxima questão...");
            Questao++;
            return true;
        }
        return false;
    }

    bool pertenceAoArray(int[] equivalentes, int DenominadorY) //Método temporário que eu criei pra comparar os elementos de um array
    {
        foreach (int valor in equivalentes)
        {
            if (valor == DenominadorY)
            {
                return true;
            }
        }
        return false;
    }

    void Questao2() //Utilizando os botões da interface, seleciona todas as frações equivalentes a 1/3.
    {
        Enunciado.text = ("Identifique 1/2");
        Detalhamento.text = ("Utilizando os botões da interface, selecione todas frações que representam 1/3 da unidade");
    }

    bool checaQ2()
    {



        return true;
    }

    void Questao3() //Observe o valor desconhecido gerado acima. Utilizando os botões da interface e testando outros valores, encontre todas as frações equivalentes a este valor desconhecido.
    {

    }

    bool checa3()
    {
        return true;
    }

}